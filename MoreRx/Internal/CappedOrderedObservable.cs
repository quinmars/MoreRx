using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using MoreRx.Internal;

namespace MoreRx
{
    internal abstract class CappedOrderedObservable<TSource> : IOrderedObservable<TSource>
    {
        private readonly IScheduler _scheduler;
        private readonly IObservable<TSource> _source;
        private readonly int _count;

        protected CappedOrderedObservable<TSource>? Parent { get; }

        public CappedOrderedObservable(IObservable<TSource> source, CappedOrderedObservable<TSource>? parent, int count, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler is null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            _source = source;
            _scheduler = scheduler;
            _count = count;

            Parent = parent;
        }

        public IOrderedObservable<TSource> CreateOrderedObservable<TSelect>(Func<TSource, TSelect> selector, IComparer<TSelect>? comparer, bool descending)
        {
            return new CappedOrderedObservable<TSource, TSelect>(_source, selector, comparer, descending, this, _count, _scheduler);
        }

        internal abstract IEvaluatingComparer<TSource> GetComparer(IEvaluatingComparer<TSource>? child = null);

        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            var disposable = new SerialDisposable();
            var comparer = GetComparer();
            var heap = new Heap<int>(comparer, _count + 1);
            var values = new TSource[_count];

            var obs = Observer.Create<TSource>(
                onNext: v =>
                {
                    var count = heap.Count;

                    if (_count == 0)
                    {
                        ; // Do nothing
                    }
                    else if (count == _count)
                    {
                        comparer.Evaluate(v, count + 1);
                        var pos1 = heap.Peak();

                        if (comparer.Compare(pos1, count + 1) < 0)
                        {
                            heap.Pop();
                            comparer.Move(count + 1, pos1);
                            values[pos1] = v;
                            heap.Push(pos1);
                        }
                        else
                        {
                            comparer.Remove(count + 1);
                        }
                    }
                    else
                    {
                        comparer.Evaluate(v, count);
                        values[count] = v;
                        heap.Push(count);
                    }
                },
                onError: e =>
                {
                    observer.OnError(e);
                    disposable.Dispose();
                },
                onCompleted: () =>
                {
                    var array = new TSource[heap.Count];
                    var i = 0;

                    while (heap.Count > 0)
                    {
                        var index = heap.Pop();
                        array[i] = values[index];
                        i++;
                    }

                    disposable.Disposable = array.ToObservable(_scheduler).Subscribe(observer);

                });

            disposable.Disposable = _source.Subscribe(obs);

            return disposable;
        }
    }

    internal sealed class CappedOrderedObservable<TSource, TSelect> : CappedOrderedObservable<TSource>
    {
        private readonly Func<TSource, TSelect> _selector;
        private readonly IComparer<TSelect> _comparer;
        private readonly bool _descending;

        public CappedOrderedObservable(IObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect>? comparer, bool descending, int count, IScheduler scheduler)
            : this(source, selector, comparer, descending, null, count, scheduler)
        {
        }

        public CappedOrderedObservable(IObservable<TSource> source,
                                  Func<TSource, TSelect> selector,
                                  IComparer<TSelect>? comparer,
                                  bool descending,
                                  CappedOrderedObservable<TSource>? parent,
                                  int count,
                                  IScheduler scheduler)
            : base(source, parent, count, scheduler)
        {
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            _selector = selector;
            _comparer = comparer ?? Comparer<TSelect>.Default;
            _descending = descending;
        }

        internal override IEvaluatingComparer<TSource> GetComparer(IEvaluatingComparer<TSource>? child = null)
        {
            IEvaluatingComparer<TSource> c = _descending
                ? new DescendingEvaluatingComparer<TSource, TSelect>(_selector, _comparer, 14, child)
                : new AscendingEvaluatingComparer<TSource, TSelect>(_selector, _comparer, 14, child);

            if (Parent == null)
            {
                return c;
            }
            else
            {
                return Parent.GetComparer(c);
            }
        }
    }
}

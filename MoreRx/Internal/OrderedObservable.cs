using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;

namespace MoreRx
{
    internal abstract class OrderedObservable<TSource> : IOrderedObservable<TSource>
    {
        private readonly IScheduler _scheduler;
        private readonly IObservable<TSource> _source;

        protected OrderedObservable<TSource>? Parent { get; }

        public OrderedObservable(IObservable<TSource> source, OrderedObservable<TSource>? parent, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;

            Parent = parent;
        }

        public IOrderedObservable<TSource> CreateOrderedObservable<TSelect>(Func<TSource, TSelect> selector, IComparer<TSelect>? comparer, bool descending)
        {
            return new OrderedObservable<TSource, TSelect>(_source, selector, comparer, descending, this, _scheduler);
        }

        internal abstract IOrderedEnumerable<TSource> GetOrderedEnumerable(IList<TSource> list);

        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            return _source
                .ToList()
                .SelectMany(l => GetOrderedEnumerable(l).ToObservable(_scheduler))
                .Subscribe(observer);
        }
    }

    internal sealed class OrderedObservable<TSource, TSelect> : OrderedObservable<TSource>
    {
        private readonly Func<TSource, TSelect> _selector;
        private readonly IComparer<TSelect>? _comparer;
        private readonly bool _descending;

        public OrderedObservable(IObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect>? comparer, bool descending, IScheduler scheduler)
            : this(source, selector, comparer, descending, null, scheduler)
        {
        }

        public OrderedObservable(IObservable<TSource> source,
                                  Func<TSource, TSelect> selector,
                                  IComparer<TSelect>? comparer,
                                  bool descending,
                                  OrderedObservable<TSource>? parent,
                                  IScheduler scheduler)
            : base(source, parent, scheduler)
        {
            _selector = selector;
            _comparer = comparer;
            _descending = descending;
        }

        internal override IOrderedEnumerable<TSource> GetOrderedEnumerable(IList<TSource> list)
        {
            if (Parent == null)
            {
                if (_descending)
                {
                    return list.OrderByDescending(_selector, _comparer);
                }
                else
                {
                    return list.OrderBy(_selector, _comparer);
                }
            }

            return Parent
                .GetOrderedEnumerable(list)
                .CreateOrderedEnumerable(_selector, _comparer, _descending);
        }
    }
}

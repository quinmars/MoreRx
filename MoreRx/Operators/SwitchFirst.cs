using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        public static IObservable<TSource> SwitchFirst<TSource>(this IObservable<IObservable<TSource>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Observable.Create<TSource>(observer =>
            {
                var gate = new object();
                var innerIsRunning = false;
                var last = default(IObservable<TSource>);
                var outerCompleted = false;

                var innerSubscription = default(IDisposable);

                var outerObserver = Observer.Create<IObservable<TSource>>(
                    xs =>
                    {
                        lock (gate)
                        {
                            if (innerIsRunning)
                            {
                                last = xs;
                                return;
                            }

                            innerIsRunning = true;
                            var innerObserver = CreateInnerObserver();
                            innerSubscription = xs.SubscribeSafe(innerObserver);
                        }
                    },
                    ex =>
                    {
                        lock (gate)
                        {
                            observer.OnError(ex);
                        }
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            outerCompleted = true;
                            if (!innerIsRunning)
                            {
                                observer.OnCompleted();
                            }
                        }

                    });

                return new CompositeDisposable
                {
                    Disposable.Create(() => innerSubscription?.Dispose()),
                    source.Subscribe(outerObserver)
                };

                IObserver<TSource> CreateInnerObserver()
                {
                    return Observer.Create<TSource>(
                        value =>
                        {
                            lock (gate)
                            {
                                observer.OnNext(value);
                            }
                        },
                        ex =>
                        {
                            lock (gate)
                            {
                                observer.OnError(ex);
                            }
                        },
                        () =>
                        {
                            lock (gate)
                            {
                                innerIsRunning = false;
                                innerSubscription.Dispose();
                                innerSubscription = null;

                                if (outerCompleted && last == null)
                                {
                                    observer.OnCompleted();
                                }
                                else if (last != null)
                                {
                                    var o = CreateInnerObserver();
                                    innerSubscription = last.SubscribeSafe(o);
                                    last = null;
                                }
                            }

                        });
                }
            });
        }
    }
}

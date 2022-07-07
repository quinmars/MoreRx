using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        public static IObservable<bool> DelayOn(this IObservable<bool> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (timeSpan == TimeSpan.Zero)
            {
                return source.DistinctUntilChanged();
            }

            return source
                .DistinctUntilChanged()
                .Select(b => b 
                    ? Observable.Timer(timeSpan, scheduler).Select(_ => true)
                    : Observable.Return(false, scheduler))
                .Switch()
                .DistinctUntilChanged();
        }
        
        public static IObservable<TSource> DelayOn<TSource>(this IObservable<TSource> source, Func<TSource, bool> selector, TimeSpan timeSpan, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (timeSpan == TimeSpan.Zero)
            {
                return source.DistinctUntilChanged();
            }

            return source
                .Select(item => (item, state: selector(item)))
                .DistinctUntilChanged(t => t.state)
                .Select(t => t.state 
                    ? Observable.Timer(timeSpan, scheduler).Select(_ => t)
                    : Observable.Return(t, scheduler))
                .Switch()
                .DistinctUntilChanged(t => t.state)
                .Select(t => t.item);
        }
    }
}

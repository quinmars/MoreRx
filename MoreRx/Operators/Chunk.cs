using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        public static IObservable<TSource[]> Chunk<TSource>(this IObservable<TSource> source, int size)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            return source.Buffer(size).Select(item => item.ToArray());
        }

        public static IObservable<TSource[]> Chunk<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler is null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            // see: https://stackoverflow.com/a/35611155/18687
            return source
                .GroupByUntil(_ => true, _ => Observable.Timer(timeSpan, scheduler))
                .SelectMany(item => item.ToArray());
        }
    }
}

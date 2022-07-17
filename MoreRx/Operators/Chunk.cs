using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MoreRx
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class MoreObservable
    {
        /// <summary>
        /// Splits the source sequence into chunks of the given size. The last chunk may have
        /// a smaller size.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <param name="source">The observable to chunk.</param>
        /// <param name="size">The desired chunk size.</param>
        /// <returns>A sequence of chunks.</returns>
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

        /// <summary>
        /// Splits the source sequence into chunks, where all chunk element come from the
        /// same time range. The first element of a new chunk starts the time range.
        /// Unlike the <c>Observable.Buffer</c>, the chunk operator does not produce
        /// empty chunks.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <param name="source">The observable to chunk.</param>
        /// <param name="timeSpan">The desired chunk size.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>A sequence of chunks.</returns>
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

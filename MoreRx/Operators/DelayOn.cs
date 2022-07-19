using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        /// <summary>
        /// Delays the switching from <c>false</c> to <c>true</c> by the specified amount of
        /// time. Switching back happens immediately. This operator is inspired by the <c>TON</c>
        /// function of some PLCs.
        /// </summary>
        /// <note>
        /// The scheduler will only be used to delay the on (<c>true</c>) value. <c>false</c> will be
        /// forwarded directly without being scheduled on the scheduler.
        /// </note>
        /// <param name="source">The source observable.</param>
        /// <param name="timeSpan">The time span to delay the on signal.</param>
        /// <param name="scheduler">The scheduler to be used for the delay.</param>
        /// <returns>The new observable instance.</returns>
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
                    : False)
                .Switch()
                .DistinctUntilChanged();
        }

        /// <summary>
        /// Delays the switching from <c>false</c> to <c>true</c> by the specified amount of
        /// time. Switching back happens immediately. This operator is inspired by the <c>TON</c>
        /// function of some PLCs.
        /// Unlike <see cref="DelayOn(IObservable{bool}, TimeSpan, IScheduler)"/>
        /// this operator does not work directly on the values of the source sequence, but use the
        /// selector to get the boolean value.
        /// </summary>
        /// <note>
        /// The scheduler will only be used to delay the on (<c>true</c>) value. <c>false</c> will be
        /// forwarded directly without being scheduled on the scheduler.
        /// </note>
        /// <param name="source">The source observable.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="timeSpan">The time span to delay the on signal.</param>
        /// <param name="scheduler">The scheduler to be used for the delay.</param>
        /// <returns>The new observable instance.</returns>
        public static IObservable<TSource> DelayOn<TSource>(this IObservable<TSource> source, Func<TSource, bool> selector, TimeSpan timeSpan, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (timeSpan == TimeSpan.Zero)
            {
                return source.DistinctUntilChanged(item => selector(item));
            }

            return source
                .Select(item => (item, state: selector(item)))
                .DistinctUntilChanged(t => t.state)
                .Select(t => t.state
                    ? Observable.Timer(timeSpan, scheduler).Select(_ => t)
                    : Observable.Return(t))
                .Switch()
                .DistinctUntilChanged(t => t.state)
                .Select(t => t.item);
        }
    }
}

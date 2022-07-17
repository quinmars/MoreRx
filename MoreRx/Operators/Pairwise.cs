using System;
using System.Linq;
using System.Reactive.Linq;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        /// <summary>
        /// Returns a sequence where the previous and the current value of the source sequence are paired as a tuple.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <param name="source">The source observable.</param>
        /// <returns>The new observable instance.</returns>
        public static IObservable<(TSource Previous, TSource Current)> Pairwise<TSource>(this IObservable<TSource> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source
                .Buffer(2, 1)
                .Where(list => list.Count > 1)
                .Select(list => (Previous: list[0], Current: list[1]));
        }
    }
}

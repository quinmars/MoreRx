using System;
using System.Linq;
using System.Reactive.Linq;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        public static IObservable<(TSource Last, TSource Current)> Pairwise<TSource>(this IObservable<TSource> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source
                .Buffer(2, 1)
                .Where(list => list.Count > 1)
                .Select(list => (Last: list[0], Current: list[1]));
        }
    }
}

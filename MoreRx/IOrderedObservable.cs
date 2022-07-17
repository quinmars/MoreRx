using System;
using System.Collections.Generic;
using System.Text;

namespace MoreRx
{
    /// <summary>
    /// The result of an <c>OrderBy</c> or <c>ThenBy</c> operation.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IOrderedObservable<out TSource> : IObservable<TSource>
    {
        /// <summary>
        /// Creates a derived <see cref="IOrderedObservable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSelect">The selector return type.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="descending"><c>true</c> if the resulting sequence should be descending, otherwise <c>false</c>.</param>
        /// <returns>The order sequence.</returns>
        IOrderedObservable<TSource> CreateOrderedObservable<TSelect>(Func<TSource, TSelect> selector, IComparer<TSelect> comparer, bool descending);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        /// <summary>
        /// Orders the ordered observable by a further sorting criterion ascendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> ThenBy<TSource, TSelect>(this IOrderedObservable<TSource> source, Func<TSource, TSelect> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.CreateOrderedObservable(selector, null, false);
        }

        /// <summary>
        /// Orders the ordered observable by a further sorting criterion ascendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> ThenBy<TSource, TSelect>(this IOrderedObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect>? comparer)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.CreateOrderedObservable(selector, comparer, false);
        }

        /// <summary>
        /// Orders the ordered observable by a further sorting criterion descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> ThenByDescending<TSource, TSelect>(this IOrderedObservable<TSource> source, Func<TSource, TSelect> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.CreateOrderedObservable(selector, null, true);
        }

        /// <summary>
        /// Orders the ordered observable by a further sorting criterion descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> ThenByDescending<TSource, TSelect>(this IOrderedObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect>? comparer)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.CreateOrderedObservable(selector, comparer, true);
        }
    }
}

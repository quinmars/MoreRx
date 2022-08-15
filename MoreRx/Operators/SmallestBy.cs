using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        /// <summary>
        /// Returns a sequence of the smallest elements in the source sequence. The elements are ordered
        /// descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to take the elements.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="count">The number of elements to take.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> SmallestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, null, descending: true, count, DefaultSortScheduler);
        }

        /// <summary>
        /// Returns a sequence of the smallest elements in the source sequence. The elements are ordered
        /// descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to take the elements.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="count">The number of elements to take.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> SmallestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count, IScheduler scheduler)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, null, descending: true, count, scheduler);
        }

        /// <summary>
        /// Returns a sequence of the smallest elements in the source sequence. The elements are ordered
        /// descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to take the elements.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="count">The number of elements to take.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> SmallestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count, IComparer<TSelect>? comparer)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, comparer, descending: true, count, DefaultSortScheduler);
        }

        /// <summary>
        /// Returns a sequence of the smallest elements in the source sequence. The elements are ordered
        /// descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to take the elements.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="count">The number of elements to take.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> SmallestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count, IComparer<TSelect>? comparer, IScheduler scheduler)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, comparer, descending: true, count, scheduler);
        }
    }
}

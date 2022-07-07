using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        private static IScheduler DefaultSortScheduler { get; } = CurrentThreadScheduler.Instance;

        /// <summary>
        /// Orders the source by means of the given element selector ascendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="K">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var comparer = Comparer<TSelect>.Default;

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: false, DefaultSortScheduler);
        }
        
        /// <summary>
        /// Orders the source by means of the given element selector ascendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="K">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (scheduler is null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            var comparer = Comparer<TSelect>.Default;

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: false, scheduler);
        }

        /// <summary>
        /// Orders the source by means of the given element selector and comparer ascendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect> comparer)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: false, DefaultSortScheduler);
        }
        
        /// <summary>
        /// Orders the source by means of the given element selector and comparer ascendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect> comparer, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            if (scheduler is null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: false, scheduler);
        }

        /// <summary>
        /// Orders the source by means of the given element selector descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderByDescending<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var comparer = Comparer<TSelect>.Default;

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: true, DefaultSortScheduler);
        }
        
        /// <summary>
        /// Orders the source by means of the given element selector descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderByDescending<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (scheduler is null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            var comparer = Comparer<TSelect>.Default;

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: true, scheduler);
        }

        /// <summary>
        /// Orders the source by means of the given element selector and comparer descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderByDescending<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect> comparer)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: true, DefaultSortScheduler);
        }
        
        /// <summary>
        /// Orders the source by means of the given element selector and comparer descendingly.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <typeparam name="TSelect">The type of the selected sorting criterion.</typeparam>
        /// <param name="source">The observable to sort.</param>
        /// <param name="selector">The selector to select the sorting criterion.</param>
        /// <param name="comparer">The comparer to compare the sorting criterion.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>The new observable instance.</returns>
        public static IOrderedObservable<TSource> OrderByDescending<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, IComparer<TSelect> comparer, IScheduler scheduler)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            if (scheduler is null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new OrderedObservable<TSource, TSelect>(source, selector, comparer, descending: true, scheduler);
        }
    }
}

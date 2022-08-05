using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        public static IOrderedObservable<TSource> LargestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, null, descending: false, count, DefaultSortScheduler);
        }

        public static IOrderedObservable<TSource> LargestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count, IScheduler scheduler)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, null, descending: false, count, scheduler);
        }

        public static IOrderedObservable<TSource> LargestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count, IComparer<TSelect>? comparer)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, comparer, descending: false, count, DefaultSortScheduler);
        }

        public static IOrderedObservable<TSource> LargestBy<TSource, TSelect>(this IObservable<TSource> source, Func<TSource, TSelect> selector, int count, IComparer<TSelect>? comparer, IScheduler scheduler)
        {
            return new CappedOrderedObservable<TSource, TSelect>(source, selector, comparer, descending: false, count, scheduler);
        }
    }
}

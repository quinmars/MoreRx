using System;
using System.Collections.Generic;
using System.Text;

namespace MoreRx
{
    public interface IOrderedObservable<out TSource> : IObservable<TSource>
    {
        IOrderedObservable<TSource> CreateOrderedObservable<TSelect>(Func<TSource, TSelect> selector, IComparer<TSelect> comparer, bool descending);
    }
}

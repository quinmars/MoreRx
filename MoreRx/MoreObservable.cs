using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace MoreRx
{
    /// <summary>
    /// A collection of extensions methods for the <see cref="IObservable{T}"/> interface.
    /// </summary>
    public static partial class MoreObservable
    {
        /// <summary>
        /// An observable returning <c>true</c> immediately.
        /// </summary>
        public static IObservable<bool> True { get; } = Observable.Return(true);

        /// <summary>
        /// An observable returning <c>false</c> immediately.
        /// </summary>
        public static IObservable<bool> False { get; } = Observable.Return(false);
    }
}

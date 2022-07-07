using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        public static IObservable<bool> True { get; } = Observable.Return(true);
        public static IObservable<bool> False { get; } = Observable.Return(false);
    }
}

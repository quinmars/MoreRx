using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace MoreRx
{
    public static partial class MoreObservable
    {
        /// <summary>
        /// Forwards the elements of the source sequence until the cancellation gets canceled.
        /// When the cancellation token was canceled the sequence will finish with a
        /// completion notification.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result observables.</typeparam>
        /// <param name="source">The source observable.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The new observable instance.</returns>
        // inspired by https://stackoverflow.com/a/65202543/18687
        public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, CancellationToken cancellationToken)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return Observable.Empty<TSource>();
            }
            else if (!cancellationToken.CanBeCanceled)
            {
                // Skip, with a count of 0, will hide the source.
                return source.Skip(0);
            }

            var cancellation = Observable.Create<Unit>(observer =>
            {
                return cancellationToken.Register(() => observer.OnNext(default));
            });

            return source
                .TakeUntil(cancellation);
        }
    }
}

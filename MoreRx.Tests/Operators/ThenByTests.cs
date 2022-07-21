using System.Reactive.Concurrency;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace MoreRx.Tests.Operators
{
    public class ThenByTests : ReactiveTest
    {
        public static readonly IComparer<int> CustomComparer = new CustomInverseIntComparer();

        [Fact]
        public void NullArgs()
        {
            var empty = Observable.Empty<string>().OrderBy(s => s);

            var a1 = () => MoreObservable.ThenBy(default(IOrderedObservable<string>)!, s => s);
            a1
                .Should()
                .Throw<ArgumentNullException>();

            var a2 = () => MoreObservable.ThenBy(empty, default(Func<string, string>)!);
            a2
                .Should()
                .Throw<ArgumentNullException>();

            var b1 = () => MoreObservable.ThenBy(default(IOrderedObservable<string>)!, s => s, Comparer<string>.Default);
            b1
                .Should()
                .Throw<ArgumentNullException>();

            var b2 = () => MoreObservable.ThenBy(empty, default(Func<string, string>)!, Comparer<string>.Default);
            b2
                .Should()
                .Throw<ArgumentNullException>();

            var b3 = () => MoreObservable.ThenBy(empty, s => s, default(IComparer<string>));
            b3
                .Should()
                .NotThrow<ArgumentNullException>();

            var d1 = () => MoreObservable.ThenBy(default(IOrderedObservable<string>)!, s => s);
            d1
                .Should()
                .Throw<ArgumentNullException>();

            var d2 = () => MoreObservable.ThenBy(empty, default(Func<string, string>)!);
            d2
                .Should()
                .Throw<ArgumentNullException>();

            var e1 = () => MoreObservable.ThenByDescending(default(IOrderedObservable<string>)!, s => s, Comparer<string>.Default);
            e1
                .Should()
                .Throw<ArgumentNullException>();

            var e2 = () => MoreObservable.ThenByDescending(empty, default(Func<string, string>)!, Comparer<string>.Default);
            e2
                .Should()
                .Throw<ArgumentNullException>();

            var e3 = () => MoreObservable.ThenByDescending(empty, s => s, default(IComparer<string>));
            e3
                .Should()
                .NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void Random_Ascending()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(220, 6),
                OnNext(230, 3),
                OnNext(240, 7),
                OnNext(250, 2),
                OnNext(260, 5),
                OnNext(270, 8),
                OnNext(280, 4),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs
                    .OrderBy(x => x % 2, scheduler)
                    .ThenBy(x => x)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 2),
                    OnNext(402, 4),
                    OnNext(403, 6),
                    OnNext(404, 8),
                    OnNext(405, 3),
                    OnNext(406, 5),
                    OnNext(407, 7),
                    OnCompleted<int>(408)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Random_Ascending_WithCustomComparer()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(220, 6),
                OnNext(230, 3),
                OnNext(240, 7),
                OnNext(250, 2),
                OnNext(260, 5),
                OnNext(270, 8),
                OnNext(280, 4),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs
                    .OrderBy(x => x % 2, scheduler)
                    .ThenBy(x => x, CustomComparer)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 8),
                    OnNext(402, 6),
                    OnNext(403, 4),
                    OnNext(404, 2),
                    OnNext(405, 7),
                    OnNext(406, 5),
                    OnNext(407, 3),
                    OnCompleted<int>(408)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Random_Descending()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(220, 6),
                OnNext(230, 3),
                OnNext(240, 7),
                OnNext(250, 2),
                OnNext(260, 5),
                OnNext(270, 8),
                OnNext(280, 4),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs
                    .OrderBy(x => x % 2, scheduler)
                    .ThenByDescending(x => x)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 8),
                    OnNext(402, 6),
                    OnNext(403, 4),
                    OnNext(404, 2),
                    OnNext(405, 7),
                    OnNext(406, 5),
                    OnNext(407, 3),
                    OnCompleted<int>(408)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Random_Descending_WithCustomComparer()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(220, 6),
                OnNext(230, 3),
                OnNext(240, 7),
                OnNext(250, 2),
                OnNext(260, 5),
                OnNext(270, 8),
                OnNext(280, 4),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs
                    .OrderBy(x => x % 2, scheduler)
                    .ThenByDescending(x => x, CustomComparer)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 2),
                    OnNext(402, 4),
                    OnNext(403, 6),
                    OnNext(404, 8),
                    OnNext(405, 3),
                    OnNext(406, 5),
                    OnNext(407, 7),
                    OnCompleted<int>(408)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Empty_Ascending()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs
                    .OrderBy(x => x % 2, scheduler)
                    .ThenBy(x => x)
            );

            res.Messages
                .Should()
                .Equal(
                    OnCompleted<int>(401)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Empty_Descending()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs
                    .OrderBy(x => x % 2, scheduler)
                    .ThenByDescending(x => x)
            );

            res.Messages
                .Should()
                .Equal(
                    OnCompleted<int>(401)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }
    }
}

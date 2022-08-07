using System.Reactive.Concurrency;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace MoreRx.Tests.Operators
{
    public class LargestByThenByTests : ReactiveTest
    {
        public static readonly IComparer<int> CustomComparer = new CustomInverseIntComparer();

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
                    .LargestBy(x => x % 2, 20, scheduler)
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
                    .LargestBy(x => x % 2, 20, scheduler)
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
                    .LargestBy(x => x % 2, 20, scheduler)
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
                    .LargestBy(x => x % 2, 20, scheduler)
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
                    .LargestBy(x => x % 2, 20, scheduler)
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
                    .LargestBy(x => x % 2, 20, scheduler)
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

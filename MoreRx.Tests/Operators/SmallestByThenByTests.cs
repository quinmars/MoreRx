using System.Reactive.Concurrency;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace MoreRx.Tests.Operators
{
    public class SmallestByThenByTests : ReactiveTest
    {
        [Fact]
        public void Random()
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
                    .SmallestBy(x => x % 2, 20, scheduler)
                    .ThenByDescending(x => x)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 7),
                    OnNext(402, 5),
                    OnNext(403, 3),
                    OnNext(404, 8),
                    OnNext(405, 6),
                    OnNext(406, 4),
                    OnNext(407, 2),
                    OnCompleted<int>(408)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Empty()
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
                    .SmallestBy(x => x % 2, 20, scheduler)
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
    }
}

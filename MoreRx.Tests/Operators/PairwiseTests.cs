using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace MoreRx.Tests.Operators
{
    public class PairwiseTests : ReactiveTest
    {
        [Fact]
        public void Length4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Pairwise()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, (2, 3)),
                    OnNext(240, (3, 4)),
                    OnNext(250, (4, 5)),
                    OnCompleted<(int, int)>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Length2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Pairwise()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, (2, 3)),
                    OnCompleted<(int, int)>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Length1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 2),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Pairwise()
            );

            res.Messages
                .Should()
                .Equal(
                    OnCompleted<(int, int)>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Length0()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Pairwise()
            );

            res.Messages
                .Should()
                .Equal(
                    OnCompleted<(int, int)>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void NullArgs()
        {
            var a = () => MoreObservable.Pairwise(default(IObservable<string>)!);

            a
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}

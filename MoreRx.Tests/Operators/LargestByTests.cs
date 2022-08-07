using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace MoreRx.Tests.Operators
{
    public class LargestByTests : ReactiveTest
    {
        public static readonly IComparer<int> CustomComparer = new CustomInverseIntComparer();

        [Fact]
        public void NullArgs()
        {
            var a1 = () => MoreObservable.LargestBy(default(IObservable<string>)!, s => s, 10);
            a1
                .Should()
                .Throw<ArgumentNullException>();

            var a2 = () => MoreObservable.LargestBy(Observable.Empty<string>(), default(Func<string, string>)!, 1);
            a2
                .Should()
                .Throw<ArgumentNullException>();

            var b1 = () => MoreObservable.LargestBy(default(IObservable<string>)!, s => s, 1, Scheduler.Default);
            b1
                .Should()
                .Throw<ArgumentNullException>();

            var b2 = () => MoreObservable.LargestBy(Observable.Empty<string>(), default(Func<string, string>)!, 1, Scheduler.Default);
            b2
                .Should()
                .Throw<ArgumentNullException>();

            var b3 = () => MoreObservable.LargestBy(Observable.Empty<string>(), s => s, 1, default(IScheduler)!);
            b3
                .Should()
                .Throw<ArgumentNullException>();

            var c1 = () => MoreObservable.LargestBy(default(IObservable<string>)!, s => s, 1, Comparer<string>.Default);
            c1
                .Should()
                .Throw<ArgumentNullException>();

            var c2 = () => MoreObservable.LargestBy(Observable.Empty<string>(), default(Func<string, string>)!, 1, Comparer<string>.Default);
            c2
                .Should()
                .Throw<ArgumentNullException>();

            var c3 = () => MoreObservable.LargestBy(Observable.Empty<string>(), s => s, 1, default(IComparer<string>));
            c3
                .Should()
                .NotThrow<ArgumentNullException>();

            var d1 = () => MoreObservable.LargestBy(default(IObservable<string>)!, s => s, 1, Comparer<string>.Default, Scheduler.Default);
            d1
                .Should()
                .Throw<ArgumentNullException>();

            var d2 = () => MoreObservable.LargestBy(Observable.Empty<string>(), default(Func<string, string>)!, 1, Comparer<string>.Default, Scheduler.Default);
            d2
                .Should()
                .Throw<ArgumentNullException>();

            var d3 = () => MoreObservable.LargestBy(Observable.Empty<string>(), s => s, 1, default(IComparer<string>), Scheduler.Default);
            d3
                .Should()
                .NotThrow<ArgumentNullException>();

            var d4 = () => MoreObservable.LargestBy(Observable.Empty<string>(), s => s, 1, Comparer<string>.Default, default!);
            d4
                .Should()
                .Throw<ArgumentNullException>();
        }


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
                xs.LargestBy(x => x, 20, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 2),
                    OnNext(402, 3),
                    OnNext(403, 4),
                    OnNext(404, 5),
                    OnNext(405, 6),
                    OnNext(406, 7),
                    OnNext(407, 8),
                    OnCompleted<int>(408)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Random_Capped()
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
                xs.LargestBy(x => x, 5, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 4),
                    OnNext(402, 5),
                    OnNext(403, 6),
                    OnNext(404, 7),
                    OnNext(405, 8),
                    OnCompleted<int>(406)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Random_ZeroElements()
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
                xs.LargestBy(x => x, 0, scheduler)
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
        public void Random_Stability()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, (1, 1)),
                OnNext(220, (2, 2)),
                OnNext(230, (2, 3)),
                OnNext(240, (1, 4)),
                OnNext(250, (2, 5)),
                OnNext(260, (1, 6)),
                OnNext(270, (1, 7)),
                OnNext(280, (2, 8)),
                OnCompleted<(int, int)>(400),
                OnNext(410, (-1, 0)),
                OnCompleted<(int, int)>(420),
                OnError<(int, int)>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.LargestBy(x => x.Item1, 20, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, (1, 4)),
                    OnNext(402, (1, 6)),
                    OnNext(403, (1, 7)),
                    OnNext(404, (2, 2)),
                    OnNext(405, (2, 3)),
                    OnNext(406, (2, 5)),
                    OnNext(407, (2, 8)),
                    OnCompleted<(int, int)>(408)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Random_WithCustomComparer()
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
                xs.LargestBy(x => x, 10, CustomComparer, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 8),
                    OnNext(402, 7),
                    OnNext(403, 6),
                    OnNext(404, 5),
                    OnNext(405, 4),
                    OnNext(406, 3),
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
        public void Random_WithCustomComparer_Capped()
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
                xs.LargestBy(x => x, 5, CustomComparer, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 6),
                    OnNext(402, 5),
                    OnNext(403, 4),
                    OnNext(404, 3),
                    OnNext(405, 2),
                    OnCompleted<int>(406)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Sorted_Capped()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.LargestBy(x => x, 5, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(401, 4),
                    OnNext(402, 5),
                    OnNext(403, 6),
                    OnNext(404, 7),
                    OnNext(405, 8),
                    OnCompleted<int>(406)
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
                xs.LargestBy(x => x, 10, scheduler)
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

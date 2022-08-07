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
    public class OrderByTests : ReactiveTest
    {
        public static readonly IComparer<int> CustomComparer = new CustomInverseIntComparer();

        [Fact]
        public void NullArgs()
        {
            var a1 = () => MoreObservable.OrderBy(default(IObservable<string>)!, s => s);
            a1
                .Should()
                .Throw<ArgumentNullException>();

            var a2 = () => MoreObservable.OrderBy(Observable.Empty<string>(), default(Func<string, string>)!);
            a2
                .Should()
                .Throw<ArgumentNullException>();

            var b1 = () => MoreObservable.OrderBy(default(IObservable<string>)!, s => s, Scheduler.Default);
            b1
                .Should()
                .Throw<ArgumentNullException>();

            var b2 = () => MoreObservable.OrderBy(Observable.Empty<string>(), default(Func<string, string>)!, Scheduler.Default);
            b2
                .Should()
                .Throw<ArgumentNullException>();

            var b3 = () => MoreObservable.OrderBy(Observable.Empty<string>(), s => s, default(IScheduler)!);
            b3
                .Should()
                .Throw<ArgumentNullException>();

            var c1 = () => MoreObservable.OrderBy(default(IObservable<string>)!, s => s, Comparer<string>.Default);
            c1
                .Should()
                .Throw<ArgumentNullException>();

            var c2 = () => MoreObservable.OrderBy(Observable.Empty<string>(), default(Func<string, string>)!, Comparer<string>.Default);
            c2
                .Should()
                .Throw<ArgumentNullException>();

            var c3 = () => MoreObservable.OrderBy(Observable.Empty<string>(), s => s, default(IComparer<string>));
            c3
                .Should()
                .NotThrow<ArgumentNullException>();

            var d1 = () => MoreObservable.OrderBy(default(IObservable<string>)!, s => s, Comparer<string>.Default, Scheduler.Default);
            d1
                .Should()
                .Throw<ArgumentNullException>();

            var d2 = () => MoreObservable.OrderBy(Observable.Empty<string>(), default(Func<string, string>)!, Comparer<string>.Default, Scheduler.Default);
            d2
                .Should()
                .Throw<ArgumentNullException>();

            var d3 = () => MoreObservable.OrderBy(Observable.Empty<string>(), s => s, default(IComparer<string>), Scheduler.Default);
            d3
                .Should()
                .NotThrow<ArgumentNullException>();

            var d4 = () => MoreObservable.OrderBy(Observable.Empty<string>(), s => s, Comparer<string>.Default, default!);
            d4
                .Should()
                .Throw<ArgumentNullException>();

            var e1 = () => MoreObservable.OrderByDescending(default(IObservable<string>)!, s => s);
            e1
                .Should()
                .Throw<ArgumentNullException>();

            var e2 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), default(Func<string, string>)!);
            e2
                .Should()
                .Throw<ArgumentNullException>();

            var f1 = () => MoreObservable.OrderByDescending(default(IObservable<string>)!, s => s, Scheduler.Default);
            f1
                .Should()
                .Throw<ArgumentNullException>();

            var f2 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), default(Func<string, string>)!, Scheduler.Default);
            f2
                .Should()
                .Throw<ArgumentNullException>();

            var f3 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), s => s, default(IScheduler)!);
            f3
                .Should()
                .Throw<ArgumentNullException>();

            var g1 = () => MoreObservable.OrderByDescending(default(IObservable<string>)!, s => s, Comparer<string>.Default);
            g1
                .Should()
                .Throw<ArgumentNullException>();

            var g2 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), default(Func<string, string>)!, Comparer<string>.Default);
            g2
                .Should()
                .Throw<ArgumentNullException>();

            var g3 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), s => s, default(IComparer<string>));
            g3
                .Should()
                .NotThrow<ArgumentNullException>();

            var h1 = () => MoreObservable.OrderByDescending(default(IObservable<string>)!, s => s, Comparer<string>.Default, Scheduler.Default);
            h1
                .Should()
                .Throw<ArgumentNullException>();

            var h2 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), default(Func<string, string>)!, Comparer<string>.Default, Scheduler.Default);
            h2
                .Should()
                .Throw<ArgumentNullException>();

            var h3 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), s => s, default(IComparer<string>), Scheduler.Default);
            h3
                .Should()
                .NotThrow<ArgumentNullException>();

            var h4 = () => MoreObservable.OrderByDescending(Observable.Empty<string>(), s => s, Comparer<string>.Default, default!);
            h4
                .Should()
                .Throw<ArgumentNullException>();
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
                xs.OrderBy(x => x, scheduler)
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
                xs.OrderBy(x => x.Item1, scheduler)
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
                xs.OrderBy(x => x, CustomComparer, scheduler)
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
                xs.OrderByDescending(x => x, scheduler)
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
                xs.OrderByDescending(x => x, CustomComparer, scheduler)
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
        public void Sorted_Ascending()
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
                xs.OrderBy(x => x, scheduler)
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
        public void Sorted_Descending()
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
                xs.OrderByDescending(x => x, scheduler)
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
                xs.OrderBy(x => x, scheduler)
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
                xs.OrderByDescending(x => x, scheduler)
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

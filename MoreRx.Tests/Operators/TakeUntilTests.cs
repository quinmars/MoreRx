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
    public class TakeUntilTests : ReactiveTest
    {
        [Fact]
        public void NullArgs()
        {
            var a = () => MoreObservable.TakeUntil(default(IObservable<string>)!, default);

            a
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Inbetween()
        {
            var scheduler = new TestScheduler();
            using var cts = new CancellationTokenSource();

            scheduler.Schedule(TimeSpan.FromTicks(241), () => cts.Cancel());

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
                xs.TakeUntil(cts.Token)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnCompleted<int>(241)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 241)
                );
        }

        [Fact]
        public void AfterLast()
        {
            var scheduler = new TestScheduler();
            using var cts = new CancellationTokenSource();

            scheduler.Schedule(TimeSpan.FromTicks(290), () => cts.Cancel());

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
                xs.TakeUntil(cts.Token)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnCompleted<int>(290)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 290)
                );
        }
        
        [Fact]
        public void Never()
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
                xs.TakeUntil(default)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void AfterCompletion()
        {
            var scheduler = new TestScheduler();
            using var cts = new CancellationTokenSource();

            scheduler.Schedule(TimeSpan.FromTicks(410), () => cts.Cancel());

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
                xs.TakeUntil(cts.Token)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void FromBeginning()
        {
            var scheduler = new TestScheduler();
            using var cts = new CancellationTokenSource();

            cts.Cancel();

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
                xs.TakeUntil(cts.Token)
            );

            res.Messages
                .Should()
                .Equal(
                    OnCompleted<int>(200)
                );

            // We cannot test the subscription here
            // because the hot observable is replaced
            // by an empty observable
        }

        [Fact]
        public void BeforeFirst()
        {
            var scheduler = new TestScheduler();
            using var cts = new CancellationTokenSource();

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
                xs.Do(_ => cts.Cancel()).TakeUntil(cts.Token)
            );

            res.Messages
                .Should()
                .Equal(
                    OnCompleted<int>(220)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 220)
                );
        }

        [Fact]
        public void AfterFirst()
        {
            var scheduler = new TestScheduler();
            using var cts = new CancellationTokenSource();

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
                xs.TakeUntil(cts.Token).Do(_ => cts.Cancel())
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 2),
                    OnCompleted<int>(220)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 220)
                );
        }
    }
}

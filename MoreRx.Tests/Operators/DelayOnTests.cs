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
    public class DelayOnTests : ReactiveTest
    {
        [Fact]
        public void NullArgs()
        {
            var a = () => MoreObservable.DelayOn(default, TimeSpan.FromSeconds(1), CurrentThreadScheduler.Instance);

            a
                .Should()
                .Throw<ArgumentNullException>();

            var b = () => MoreObservable.DelayOn(default(IObservable<string>), s => s is null, TimeSpan.FromSeconds(1), CurrentThreadScheduler.Instance);

            b
                .Should()
                .Throw<ArgumentNullException>();
        }


        [Fact]
        public void Delayed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, false),
                OnNext(220, false),
                OnNext(230, true),
                OnNext(240, false),
                OnNext(250, false),
                OnCompleted<bool>(400),
                OnNext(410, false),
                OnCompleted<bool>(420),
                OnError<bool>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(TimeSpan.FromTicks(2), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, false),
                    OnNext(232, true),
                    OnNext(240, false),
                    OnCompleted<bool>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }
        
        [Fact]
        public void NotDelayed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, false),
                OnNext(220, false),
                OnNext(230, true),
                OnNext(240, false),
                OnNext(250, false),
                OnCompleted<bool>(400),
                OnNext(410, false),
                OnCompleted<bool>(420),
                OnError<bool>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(TimeSpan.Zero, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, false),
                    OnNext(230, true),
                    OnNext(240, false),
                    OnCompleted<bool>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Skipped()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, false),
                OnNext(220, false),
                OnNext(230, true),
                OnNext(240, false),
                OnNext(250, false),
                OnCompleted<bool>(400),
                OnNext(410, false),
                OnCompleted<bool>(420),
                OnError<bool>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, false),
                    OnCompleted<bool>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void AlwaysOn()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, true),
                OnNext(220, true),
                OnNext(230, true),
                OnNext(240, true),
                OnNext(250, true),
                OnCompleted<bool>(400),
                OnNext(410, false),
                OnCompleted<bool>(420),
                OnError<bool>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(TimeSpan.FromTicks(5), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(225, true),
                    OnCompleted<bool>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void AlwaysOff()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, false),
                OnNext(220, false),
                OnNext(230, false),
                OnNext(240, false),
                OnNext(250, false),
                OnCompleted<bool>(400),
                OnNext(410, false),
                OnCompleted<bool>(420),
                OnError<bool>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(TimeSpan.FromTicks(5), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, false),
                    OnCompleted<bool>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Delayed_WithSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 2),
                OnNext(220, 4),
                OnNext(230, 5),
                OnNext(240, 6),
                OnNext(250, 8),
                OnCompleted<int>(400),
                OnNext(410, 9),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(i => (i % 2) == 1, TimeSpan.FromTicks(2), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 4),
                    OnNext(232, 5),
                    OnNext(240, 6),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }
        
        [Fact]
        public void NotDelayed_WithSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 2),
                OnNext(220, 4),
                OnNext(230, 5),
                OnNext(240, 6),
                OnNext(250, 8),
                OnCompleted<int>(400),
                OnNext(410, 9),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(i => (i % 2) == 1, TimeSpan.Zero, scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 4),
                    OnNext(230, 5),
                    OnNext(240, 6),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void Skipped_WithSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 2),
                OnNext(220, 4),
                OnNext(230, 5),
                OnNext(240, 6),
                OnNext(250, 8),
                OnCompleted<int>(400),
                OnNext(410, 9),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(i => (i % 2) == 1, TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 4),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void AlwaysOn_WithSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(220, 3),
                OnNext(230, 5),
                OnNext(240, 7),
                OnNext(250, 9),
                OnCompleted<int>(400),
                OnNext(410, 11),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(i => (i % 2) == 1, TimeSpan.FromTicks(5), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(225, 3),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }
        
        [Fact]
        public void AlwaysOff_WithSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 2),
                OnNext(220, 4),
                OnNext(230, 6),
                OnNext(240, 8),
                OnNext(250, 10),
                OnCompleted<int>(400),
                OnNext(410, 12),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.DelayOn(i => (i % 2) == 1, TimeSpan.FromTicks(5), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, 4),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }
    }
}

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
    }
}

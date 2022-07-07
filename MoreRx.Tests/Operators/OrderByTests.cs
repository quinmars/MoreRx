using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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

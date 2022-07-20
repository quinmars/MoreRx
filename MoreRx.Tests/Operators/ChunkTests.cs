using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace MoreRx.Tests.Operators
{
    public class ChunkTests : ReactiveTest
    {
        [Fact]
        public void NullArgs_Size()
        {
            var a = () => MoreObservable.Chunk(default(IObservable<string>)!, 5);

            a
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void NullArgs_Time()
        {
            var scheduler = new TestScheduler();
            var source = Observable.Empty<string>();

            var a = () => MoreObservable.Chunk(default(IObservable<string>)!, TimeSpan.FromTicks(5), scheduler);
            var b = () => MoreObservable.Chunk(source, TimeSpan.FromTicks(5), default!);

            a
                .Should()
                .Throw<ArgumentNullException>();
            b
                .Should()
                .Throw<ArgumentNullException>();
        }


        [Fact]
        public void ByCount_Size2()
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
                xs.Chunk(2)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, new[] { 2, 3 }),
                    OnNext(250, new[] { 4, 5 }),
                    OnCompleted<int[]>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void ByCount_Size1()
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
                xs.Chunk(1)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(220, new[] { 2 }),
                    OnNext(230, new[] { 3 }),
                    OnNext(240, new[] { 4 }),
                    OnNext(250, new[] { 5 }),
                    OnCompleted<int[]>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void ByCount_Size0()
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

            scheduler
                .Invoking(s => s.Start(() => xs.Chunk(0)))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ByTimet_Tick11()
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
                xs.Chunk(TimeSpan.FromTicks(11), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(231, new[] { 2, 3 }),
                    OnNext(251, new[] { 4, 5 }),
                    OnCompleted<int[]>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void ByTimet_Tick15()
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
                xs.Chunk(TimeSpan.FromTicks(15), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(235, new[] { 2, 3 }),
                    OnNext(255, new[] { 4, 5 }),
                    OnCompleted<int[]>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }

        [Fact]
        public void ByTimet_Tick20()
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
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Chunk(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(240, new[] { 2, 3, 4 }),
                    OnNext(270, new[] { 5, 6, 7 }),
                    OnCompleted<int[]>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );
        }
    }
}

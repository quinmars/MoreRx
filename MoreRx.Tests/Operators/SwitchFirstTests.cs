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
    public class SwitchFirstTests : ReactiveTest
    {
        [Fact]
        public void NullArgs()
        {
            var a = () => MoreObservable.SwitchFirst(default(IObservable<IObservable<string>>)!);

            a
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void OuterCompletes_NoOverlap()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(30, 103),
                    OnNext(40, 104),
                    OnCompleted<int>(50)
                );

            var ys2 = scheduler.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

            var ys3 = scheduler.CreateColdObservable(
                    OnNext(10, 301),
                    OnNext(20, 302),
                    OnNext(30, 303),
                    OnNext(40, 304),
                    OnCompleted<int>(50)
                );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(220, ys1),
                OnNext<IObservable<int>>(280, ys2),
                OnNext<IObservable<int>>(340, ys3),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
                xs.SwitchFirst()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, 101),
                    OnNext(240, 102),
                    OnNext(250, 103),
                    OnNext(260, 104),
                    OnNext(290, 201),
                    OnNext(300, 202),
                    OnNext(310, 203),
                    OnNext(320, 204),
                    OnNext(350, 301),
                    OnNext(360, 302),
                    OnNext(370, 303),
                    OnNext(380, 304),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );

            ys1.Subscriptions
                .Should()
                .Equal(
                    Subscribe(220, 270)
                );

            ys2.Subscriptions
                .Should()
                .Equal(
                    Subscribe(280, 330)
                );

            ys3.Subscriptions
                .Should()
                .Equal(
                    Subscribe(340, 390)
                );
        }

        [Fact]
        public void OuterCompletes_WithOverlap()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(30, 103),
                    OnNext(40, 104),
                    OnCompleted<int>(50)
                );

            var ys2 = scheduler.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

            var ys3 = scheduler.CreateColdObservable(
                    OnNext(10, 301),
                    OnNext(20, 302),
                    OnNext(30, 303),
                    OnNext(40, 304),
                    OnCompleted<int>(50)
                );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(220, ys1),
                OnNext<IObservable<int>>(230, ys2),
                OnNext<IObservable<int>>(240, ys3),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
                xs.SwitchFirst()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, 101),
                    OnNext(240, 102),
                    OnNext(250, 103),
                    OnNext(260, 104),
                    OnNext(280, 301),
                    OnNext(290, 302),
                    OnNext(300, 303),
                    OnNext(310, 304),
                    OnCompleted<int>(400)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 400)
                );

            ys1.Subscriptions
                .Should()
                .Equal(
                    Subscribe(220, 270)
                );

            ys2.Subscriptions
                .Should()
                .Equal(
                );

            ys3.Subscriptions
                .Should()
                .Equal(
                    Subscribe(270, 320)
                );
        }

        [Fact]
        public void InnerCompletes_NoOverlap()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(30, 103),
                    OnNext(40, 104),
                    OnCompleted<int>(50)
                );

            var ys2 = scheduler.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

            var ys3 = scheduler.CreateColdObservable(
                    OnNext(10, 301),
                    OnNext(20, 302),
                    OnNext(30, 303),
                    OnNext(40, 304),
                    OnCompleted<int>(50)
                );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(220, ys1),
                OnNext<IObservable<int>>(290, ys2),
                OnNext<IObservable<int>>(360, ys3),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
                xs.SwitchFirst()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, 101),
                    OnNext(240, 102),
                    OnNext(250, 103),
                    OnNext(260, 104),
                    OnNext(300, 201),
                    OnNext(310, 202),
                    OnNext(320, 203),
                    OnNext(330, 204),
                    OnNext(370, 301),
                    OnNext(380, 302),
                    OnNext(390, 303),
                    OnNext(400, 304),
                    OnCompleted<int>(410)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 410)
                );

            ys1.Subscriptions
                .Should()
                .Equal(
                    Subscribe(220, 270)
                );

            ys2.Subscriptions
                .Should()
                .Equal(
                    Subscribe(290, 340)
                );

            ys3.Subscriptions
                .Should()
                .Equal(
                    Subscribe(360, 410)
                );
        }

        [Fact]
        public void InnerCompletes_WithOverlap()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(30, 103),
                    OnNext(40, 104),
                    OnCompleted<int>(50)
                );

            var ys2 = scheduler.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

            var ys3 = scheduler.CreateColdObservable(
                    OnNext(10, 301),
                    OnNext(20, 302),
                    OnNext(30, 303),
                    OnNext(40, 304),
                    OnCompleted<int>(50)
                );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(220, ys1),
                OnNext<IObservable<int>>(230, ys2),
                OnNext<IObservable<int>>(240, ys3),
                OnCompleted<IObservable<int>>(250)
            );

            var res = scheduler.Start(() =>
                xs.SwitchFirst()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, 101),
                    OnNext(240, 102),
                    OnNext(250, 103),
                    OnNext(260, 104),
                    OnNext(280, 301),
                    OnNext(290, 302),
                    OnNext(300, 303),
                    OnNext(310, 304),
                    OnCompleted<int>(320)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 320)
                );

            ys1.Subscriptions
                .Should()
                .Equal(
                    Subscribe(220, 270)
                );

            ys2.Subscriptions
                .Should()
                .Equal(
                );

            ys3.Subscriptions
                .Should()
                .Equal(
                    Subscribe(270, 320)
                );
        }

        [Fact]
        public void InnerThrows_WithOverlap()
        {
            var scheduler = new TestScheduler();
            var exception = new Exception("Inner exception");

            var ys1 = scheduler.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(30, 103),
                    OnNext(40, 104),
                    OnError<int>(50, exception)
                );

            var ys2 = scheduler.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(220, ys1),
                OnNext<IObservable<int>>(230, ys2),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
                xs.SwitchFirst()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, 101),
                    OnNext(240, 102),
                    OnNext(250, 103),
                    OnNext(260, 104),
                    OnError<int>(270, exception)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 270)
                );

            ys1.Subscriptions
                .Should()
                .Equal(
                    Subscribe(220, 270)
                );

            ys2.Subscriptions
                .Should()
                .Equal(
                );
        }

        [Fact]
        public void OuterThrows_WithOverlap()
        {
            var scheduler = new TestScheduler();
            var exception = new Exception("Outer exception");

            var ys1 = scheduler.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(30, 103),
                    OnNext(40, 104),
                    OnCompleted<int>(50)
                );

            var ys2 = scheduler.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(220, ys1),
                OnError<IObservable<int>>(240, exception),
                OnNext<IObservable<int>>(250, ys1),
                OnCompleted<IObservable<int>>(260)
            );

            var res = scheduler.Start(() =>
                xs.SwitchFirst()
            );

            res.Messages
                .Should()
                .Equal(
                    OnNext(230, 101),
                    OnError<int>(240, exception)
                );

            xs.Subscriptions
                .Should()
                .Equal(
                    Subscribe(200, 240)
                );

            ys1.Subscriptions
                .Should()
                .Equal(
                    Subscribe(220, 240)
                );

            ys2.Subscriptions
                .Should()
                .Equal(
                );
        }
    }
}

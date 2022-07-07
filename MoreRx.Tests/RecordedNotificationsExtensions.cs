using FluentAssertions.Primitives;
using Microsoft.Reactive.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MoreRx.Tests
{
    public static class RecordedNotificationsExtensions
    {
        public static RecordedNotificationsAssertions<T> Should<T>(this IEnumerable<Recorded<Notification<T>>> actualValue)
        {
            return new RecordedNotificationsAssertions<T>(actualValue);
        }
    }
}

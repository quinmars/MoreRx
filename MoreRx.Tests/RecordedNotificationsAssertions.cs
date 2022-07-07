using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
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
    public class RecordedNotificationsAssertions<T> : GenericCollectionAssertions<Recorded<Notification<T>>>
    {
        public RecordedNotificationsAssertions(IEnumerable<Recorded<Notification<T>>> actualValue) : base(actualValue)
        {
        }

        public new AndConstraint<GenericCollectionAssertions<Recorded<Notification<T>>>> Equal(params Recorded<Notification<T>>[] expectation)
        {
            return BeEquivalentTo(expectation,
                options => options
                   .WithStrictOrdering()
                   .ComparingByMembers<Recorded<Notification<int[]>>>()
                   .ComparingByMembers<Notification<int[]>>());
        }
    }
}

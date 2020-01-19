using BAOZ.Common;
using EventFlow.EventStores;
using System;
using System.Collections.Generic;
using System.Text;
using User.Core.Domain.Aggregates;

namespace User.Core.Domain.Events
{
    [EventVersion("UserCreated", 1)]

    public class UserCreatedEvent : BaseEvent<UserAggregate>
    {
        public UserCreatedEvent(Guid userId, string firstName)
        {
            UserId = userId;
            FirstName = firstName;
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
    }
}

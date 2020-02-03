using BAOZ.Common;
using EventFlow.EventStores;
using System;
using User.Core.Domain.Aggregates;

namespace User.Core.Domain.Events
{
    [EventVersion("UserUpdated", 1)]
    public class UserUpdatedEvent : BaseEvent<UserAggregate>
    {
        public UserUpdatedEvent(Guid id, string firstName)
        {
            Id = id;
            FirstName = firstName;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }

    }
}

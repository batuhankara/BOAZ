using BAOZ.Common;
using EventFlow.EventStores;
using System;
using User.Core.Domain.Aggregates;

namespace User.Core.Domain.Events
{
    [EventVersion("UserEmailTokenGenareted", 1)]
    public class UserEmailTokenGenareted : BaseEvent<UserAggregate>
    {
        public UserEmailTokenGenareted(Guid id, string token, DateTime expireDate)
        {
            Id = id;
            Token = token;
            ExpireDate = expireDate;
        }

        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

using BAOZ.Common;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using System;
using System.Collections.Generic;
using System.Text;
using User.Core.Domain.Aggregates;
using User.Core.Domain.Events;

namespace User.Application.ReadModels
{
   public class TestReadModel : IReadModel,
    IAmReadModelFor<UserAggregate, BaozId, UserCreatedEvent>
    {
        public string MagicNumber { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<UserAggregate, BaozId, UserCreatedEvent> domainEvent)
        {
            MagicNumber = domainEvent.AggregateEvent.FirstName;
        }
    }
}

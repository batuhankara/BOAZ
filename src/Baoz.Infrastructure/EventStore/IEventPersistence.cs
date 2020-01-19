using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Baoz.Infrastructure.EventStore
{
    public interface IEventPersistence
    {
        Task<AllCommittedEventsPage> LoadAllCommittedEvents(
            GlobalPosition globalPosition,
            int pageSize,
            CancellationToken cancellationToken);

        Task<IReadOnlyCollection<ICommittedDomainEvent>> CommitEventsAsync<TAggregate, TIdentity>(
            IIdentity id,
            IReadOnlyCollection<SerializedEvent> serializedEvents,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity;

        Task<IReadOnlyCollection<ICommittedDomainEvent>> LoadCommittedEventsAsync<TAggregate, TIdentity>(
            IIdentity id,
            int fromEventSequenceNumber,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity;

        Task DeleteEventsAsync<TAggregate, TIdentity>(IIdentity id, CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity;
    }

}

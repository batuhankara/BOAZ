﻿using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.Logs;
using EventFlow.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Baoz.Infrastructure.EventStore
{
    public class EventStoreBase : IEventStore
    {
        private readonly IAggregateFactory _aggregateFactory;
        private readonly IEventJsonSerializer _eventJsonSerializer;
        private readonly IEventPersistence _eventPersistence;
        private readonly ISnapshotStore _snapshotStore;
        private readonly IEventUpgradeManager _eventUpgradeManager;
        private readonly ILog _log;
        private readonly IReadOnlyCollection<IMetadataProvider> _metadataProviders;

        public EventStoreBase(
            ILog log,
            IAggregateFactory aggregateFactory,
            IEventJsonSerializer eventJsonSerializer,
            IEventUpgradeManager eventUpgradeManager,
            IEnumerable<IMetadataProvider> metadataProviders,
            IEventPersistence eventPersistence,
            ISnapshotStore snapshotStore)
        {
            _eventPersistence = eventPersistence;
            _snapshotStore = snapshotStore;
            _log = log;
            _aggregateFactory = aggregateFactory;
            _eventJsonSerializer = eventJsonSerializer;
            _eventUpgradeManager = eventUpgradeManager;
            _metadataProviders = metadataProviders.ToList();
        }

        public virtual async Task<IReadOnlyCollection<IDomainEvent<TAggregate, TIdentity>>> StoreAsync<TAggregate, TIdentity>(
            TIdentity id,
            IReadOnlyCollection<IUncommittedEvent> uncommittedDomainEvents,
            ISourceId sourceId,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (sourceId.IsNone()) throw new ArgumentNullException(nameof(sourceId));

            if (uncommittedDomainEvents == null || !uncommittedDomainEvents.Any())
            {
                return new IDomainEvent<TAggregate, TIdentity>[] { };
            }

            var aggregateType = typeof(TAggregate);
            _log.Verbose(() => $"Storing {uncommittedDomainEvents.Count} events for aggregate '{aggregateType.PrettyPrint()}' with ID '{id}'");

            var batchId = Guid.NewGuid().ToString();
            var storeMetadata = new[]
                {
                    new KeyValuePair<string, string>(MetadataKeys.BatchId, batchId),
                    new KeyValuePair<string, string>(MetadataKeys.SourceId, sourceId.Value)
                };

            var serializedEvents = uncommittedDomainEvents
                .Select(e =>
                {
                    var md = _metadataProviders
                        .SelectMany(p => p.ProvideMetadata<TAggregate, TIdentity>(id, e.AggregateEvent, e.Metadata))
                        .Concat(e.Metadata)
                        .Concat(storeMetadata);
                    return _eventJsonSerializer.Serialize(e.AggregateEvent, md);
                })
                .ToList();

            var committedDomainEvents = await _eventPersistence.CommitEventsAsync<TAggregate, TIdentity>(
                id,
                serializedEvents,
                cancellationToken)
                .ConfigureAwait(false);

            var domainEvents = committedDomainEvents
                .Select(e => _eventJsonSerializer.Deserialize<TAggregate, TIdentity>(id, e))
                .ToList();

            return domainEvents;
        }

        public async Task<AllEventsPage> LoadAllEventsAsync(
            GlobalPosition globalPosition,
            int pageSize,
            CancellationToken cancellationToken)
        {
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

            var allCommittedEventsPage = await _eventPersistence.LoadAllCommittedEvents(
                globalPosition,
                pageSize,
                cancellationToken)
                .ConfigureAwait(false);
            var domainEvents = (IReadOnlyCollection<IDomainEvent>)allCommittedEventsPage.CommittedDomainEvents
                .Select(e => _eventJsonSerializer.Deserialize(e))
                .ToList();
            domainEvents = _eventUpgradeManager.Upgrade(domainEvents);
            return new AllEventsPage(allCommittedEventsPage.NextGlobalPosition, domainEvents);
        }

        public Task<IReadOnlyCollection<IDomainEvent<TAggregate, TIdentity>>> LoadEventsAsync<TAggregate, TIdentity>(
            TIdentity id,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            return LoadEventsAsync<TAggregate, TIdentity>(
                id,
                1,
                cancellationToken);
        }

        public virtual async Task<IReadOnlyCollection<IDomainEvent<TAggregate, TIdentity>>> LoadEventsAsync<TAggregate, TIdentity>(
            TIdentity id,
            int fromEventSequenceNumber,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            if (fromEventSequenceNumber < 1) throw new ArgumentOutOfRangeException(nameof(fromEventSequenceNumber), "Event sequence numbers start at 1");

            var committedDomainEvents = await _eventPersistence.LoadCommittedEventsAsync<TAggregate, TIdentity>(
                id,
                fromEventSequenceNumber,
                cancellationToken)
                .ConfigureAwait(false);
            var domainEvents = (IReadOnlyCollection<IDomainEvent<TAggregate, TIdentity>>)committedDomainEvents
                .Select(e => _eventJsonSerializer.Deserialize<TAggregate, TIdentity>(id, e))
                .ToList();

            if (!domainEvents.Any())
            {
                return domainEvents;
            }

            domainEvents = _eventUpgradeManager.Upgrade(domainEvents);

            return domainEvents;
        }

        public virtual async Task<TAggregate> LoadAggregateAsync<TAggregate, TIdentity>(
            TIdentity id,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            var aggregate = await _aggregateFactory.CreateNewAggregateAsync<TAggregate, TIdentity>(id).ConfigureAwait(false);
            await aggregate.LoadAsync(this, _snapshotStore, cancellationToken).ConfigureAwait(false);
            return aggregate;
        }

        public Task DeleteAggregateAsync<TAggregate, TIdentity>(
            TIdentity id,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            return _eventPersistence.DeleteEventsAsync<TAggregate, TIdentity>(
                id,
                cancellationToken);
        }
    }


}

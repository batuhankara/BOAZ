using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BAOZ.Common
{
    public abstract class SnapshotAggregateRootBase<TAggregate, TIdentity, TSnapshot> : SnapshotAggregateRoot<TAggregate, TIdentity, TSnapshot>
        where TAggregate : SnapshotAggregateRoot<TAggregate, TIdentity, TSnapshot>
        where TIdentity : IIdentity
        where TSnapshot : ISnapshot
    {
        protected SnapshotAggregateRootBase(TIdentity id, ISnapshotStrategy snapshotStrategy) : base(id, snapshotStrategy)
        {
        }

        public override Task<IReadOnlyCollection<IDomainEvent>> CommitAsync(IEventStore eventStore, ISnapshotStore snapshotStore, ISourceId sourceId, CancellationToken cancellationToken)
        {
            return base.CommitAsync(eventStore, snapshotStore, sourceId, cancellationToken);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Task LoadAsync(IEventStore eventStore, ISnapshotStore snapshotStore, CancellationToken cancellationToken)
        {
            return base.LoadAsync(eventStore, snapshotStore, cancellationToken);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void ApplyEvent(IAggregateEvent<TAggregate, TIdentity> aggregateEvent)
        {
            base.ApplyEvent(aggregateEvent);
        }

        protected override Task<TSnapshot> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<ISnapshotMetadata> CreateSnapshotMetadataAsync(CancellationToken cancellationToken)
        {
            return base.CreateSnapshotMetadataAsync(cancellationToken);
        }

        protected override void Emit<TEvent>(TEvent aggregateEvent, IMetadata metadata = null)
        {
            base.Emit(aggregateEvent, metadata);
        }

        protected override Task LoadSnapshotAsync(TSnapshot snapshot, ISnapshotMetadata metadata, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }

}

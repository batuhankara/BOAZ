using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using EventFlow.Snapshots;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BAOZ.Common
{
    public abstract class AggregateRootBase<TAggregate, TIdentity> : AggregateRoot<TAggregate, TIdentity>
      where TAggregate : AggregateRoot<TAggregate, TIdentity>
      where TIdentity : IIdentity
    {
        public new Guid Id => Guid.Parse(base.GetIdentity().Value?.ToString());

        protected AggregateRootBase(TIdentity id) : base(id)
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

        protected override void Emit<TEvent>(TEvent aggregateEvent, IMetadata metadata = null)
        {
            base.Emit(aggregateEvent, metadata);
        }
    }

}

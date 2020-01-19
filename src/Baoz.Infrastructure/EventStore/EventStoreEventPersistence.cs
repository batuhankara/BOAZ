using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using EventFlow.Exceptions;
using EventFlow.Logs;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baoz.Infrastructure.EventStore
{
    public class EventStoreEventPersistence : IEventPersistence
    {
        private readonly ILog _log;
        private readonly IEventStoreConnection _connection;
        private readonly IEventStoreStreamNameFactory _eventStoreStreamNameFactory;

        private class EventStoreEvent : ICommittedDomainEvent
        {
            public string AggregateId { get; set; }
            public string Data { get; set; }
            public string Metadata { get; set; }
            public int AggregateSequenceNumber { get; set; }
        }

        public EventStoreEventPersistence(
            ILog log,
            IEventStoreConnection connection,
            IEventStoreStreamNameFactory eventStoreStreamNameFactory = null)
        {
            _log = log;
            _connection = connection;
            _eventStoreStreamNameFactory = eventStoreStreamNameFactory;
        }

        public async Task<AllCommittedEventsPage> LoadAllCommittedEvents(
            GlobalPosition globalPosition,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var nextPosition = ParsePosition(globalPosition);
            var resolvedEvents = new List<ResolvedEvent>();
            AllEventsSlice allEventsSlice;

            do
            {
                allEventsSlice = await _connection.ReadAllEventsForwardAsync(nextPosition, pageSize, false).ConfigureAwait(false);
                resolvedEvents.AddRange(allEventsSlice.Events.Where(e => !e.OriginalStreamId.StartsWith("$")));
                nextPosition = allEventsSlice.NextPosition;

            }
            while (resolvedEvents.Count < pageSize && !allEventsSlice.IsEndOfStream);

            var eventStoreEvents = Map(resolvedEvents);

            return new AllCommittedEventsPage(
                new GlobalPosition(string.Format("{0}-{1}", nextPosition.CommitPosition, nextPosition.PreparePosition)),
                eventStoreEvents);
        }

        private static Position ParsePosition(GlobalPosition globalPosition)
        {
            if (globalPosition.IsStart)
            {
                return Position.Start;
            }

            var parts = globalPosition.Value.Split('-');
            if (parts.Length != 2)
            {
                throw new ArgumentException(string.Format(
                    "Unknown structure for global position '{0}'. Expected it to be empty or in the form 'L-L'",
                    globalPosition.Value));
            }

            var commitPosition = long.Parse(parts[0]);
            var preparePosition = long.Parse(parts[1]);

            return new Position(commitPosition, preparePosition);
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> CommitEventsAsync<TAggregate, TIdentity>(
            IIdentity id,
            IReadOnlyCollection<SerializedEvent> serializedEvents,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            var committedDomainEvents = serializedEvents
                .Select(e => new EventStoreEvent
                {
                    AggregateSequenceNumber = e.AggregateSequenceNumber,
                    Metadata = e.SerializedMetadata,
                    AggregateId = id.Value,
                    Data = e.SerializedData
                })
                .ToList();

            var expectedVersion = Math.Max(serializedEvents.Min(e => e.AggregateSequenceNumber) - 2, ExpectedVersion.NoStream);
            var eventDatas = serializedEvents
                .Select(e =>
                {
                    // While it might be tempting to use e.Metadata.EventId here, we can't
                    // as EventStore won't detect optimistic concurrency exceptions then
                    var guid = Guid.NewGuid();

                    var eventType = string.Format("{0}.{1}.{2}", _eventStoreStreamNameFactory.GetAggregateName(e.Metadata[MetadataKeys.AggregateName]), e.Metadata.EventName, e.Metadata.EventVersion);
                    var data = Encoding.UTF8.GetBytes(e.SerializedData);
                    var meta = Encoding.UTF8.GetBytes(e.SerializedMetadata);
                    return new EventData(guid, eventType, true, data, meta);
                })
                .ToList();

            try
            {
                var writeResult = await _connection.AppendToStreamAsync(
                    _eventStoreStreamNameFactory.GetStreamName(typeof(TAggregate), id),
                    expectedVersion,
                    eventDatas)
                    .ConfigureAwait(false);

                _log.Verbose(
                    "Wrote entity {0} with version {1} ({2},{3})",
                    id,
                    writeResult.NextExpectedVersion - 1,
                    writeResult.LogPosition.CommitPosition,
                    writeResult.LogPosition.PreparePosition);
            }
            catch (WrongExpectedVersionException e)
            {
                throw new OptimisticConcurrencyException(e.Message, e);
            }

            return committedDomainEvents;
        }

        public async Task<IReadOnlyCollection<ICommittedDomainEvent>> LoadCommittedEventsAsync<TAggregate, TIdentity>(
            IIdentity id,
            int fromEventSequenceNumber,
            CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            var streamEvents = new List<ResolvedEvent>();

            StreamEventsSlice currentSlice;
            var nextSliceStart = fromEventSequenceNumber <= 1
                ? StreamPosition.Start
                : fromEventSequenceNumber - 1; // Starts from zero

            do
            {
                currentSlice = await _connection.ReadStreamEventsForwardAsync(
                    _eventStoreStreamNameFactory.GetStreamName(typeof(TAggregate), id),
                    nextSliceStart,
                    200,
                    false)
                    .ConfigureAwait(false);
                nextSliceStart = (int)currentSlice.NextEventNumber;
                streamEvents.AddRange(currentSlice.Events);

            }
            while (!currentSlice.IsEndOfStream);

            return Map(streamEvents);
        }

        public Task DeleteEventsAsync<TAggregate, TIdentity>(IIdentity id, CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
        {
            return _connection.DeleteStreamAsync(_eventStoreStreamNameFactory.GetStreamName(typeof(TAggregate), id), ExpectedVersion.Any);
        }

        private static IReadOnlyCollection<EventStoreEvent> Map(IEnumerable<ResolvedEvent> resolvedEvents)
        {
            return resolvedEvents
                .Select(e => new EventStoreEvent
                {
                    AggregateSequenceNumber = (int)(e.Event.EventNumber + 1), // Starts from zero
                    Metadata = Encoding.UTF8.GetString(e.Event.Metadata),
                    AggregateId = e.OriginalStreamId,
                    Data = Encoding.UTF8.GetString(e.Event.Data),
                })
                .ToList();
        }
    }

}

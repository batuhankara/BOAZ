using EventFlow;
using EventFlow.Configuration;
using EventFlow.Core;
using EventFlow.EventStores;
using EventStore.ClientAPI;
using System;

namespace Baoz.Infrastructure.EventStore.Extensions
{
    public static class EventFlowOptionsExtensions
    {
        public static IEventFlowOptions UseEventStoreEventStore(
            this IEventFlowOptions eventFlowOptions,
            Uri uri,
            ConnectionSettings connectionSettings,
            string connectionNamePrefix = null)
        {
            var sanitizedConnectionNamePrefix = string.IsNullOrEmpty(connectionNamePrefix)
                ? string.Empty
                : connectionNamePrefix + " - ";

            var eventStoreConnection = EventStoreConnection.Create(
                connectionSettings,
                uri,
                $"{sanitizedConnectionNamePrefix}EventFlow v{typeof(EventFlowOptionsExtensions).Assembly.GetName().Version}");

            using (var a = AsyncHelper.Wait)
            {
                a.Run(eventStoreConnection.ConnectAsync());
            }

            return eventFlowOptions
                .RegisterServices(f => f.Register(r => eventStoreConnection, Lifetime.Singleton))
                .RegisterServices(f => f.Register<IEventPersistence, EventStoreEventPersistence>())
                .RegisterServices(f => f.Register<IEventStore, EventStoreBase>())
                .RegisterServices(f => f.Register<IEventPersistence, EventStoreEventPersistence>());
        }
    }
}

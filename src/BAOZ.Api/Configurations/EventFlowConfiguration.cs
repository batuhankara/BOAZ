using Autofac;
using Baoz.Infrastructure.EventStore.Extensions;
using Baoz.Infrastructure.Settings;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Autofac.Extensions;
using EventFlow.Extensions;
using EventFlow.MetadataProviders;
using EventFlow.MongoDB.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using EventFlow.Snapshots.Strategies;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Data.Common;
using User.Application;

namespace BAOZ.Api.Configurations
{
    public static class EventFlowConfiguration
    {
        public static void AddEventSourcing(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            var snapshotStoreSettings = configuration.GetSnapshotStoreSettings();
            var rabbitMQSettings = configuration.GetRabbitMQSettings();
            var eventStoreSettings = configuration.GetEventStoreSettings();

            var rabbitMQConfiguration = RabbitMqConfiguration.With(new Uri($"amqp://{rabbitMQSettings.UserName}:{rabbitMQSettings.Password}@{rabbitMQSettings.Host}:{rabbitMQSettings.Port}"), rabbitMQSettings.Persistent.Value, 5, rabbitMQSettings.ExchangeName);

            EventFlowOptions.New

                            .UseAutofacContainerBuilder(containerBuilder) // Must be the first line!
                            .Configure(c => c.ThrowSubscriberExceptions = true)
                            .RegisterModule<UserModule>()
                            .AddAspNetCore(options =>
                            {
                                options.AddUserClaimsMetadata();
                                options.AddDefaultMetadataProviders();
                            })
                            .ConfigureEventStore(eventStoreSettings)
                            .ConfigureMongoDb(new MongoClient(snapshotStoreSettings.ConnectionString), snapshotStoreSettings.Name)
                            .UseMongoDbSnapshotStore()
                            .RegisterServices(sr => sr.Register(i => SnapshotEveryFewVersionsStrategy.Default))
                            .PublishToRabbitMq(rabbitMQConfiguration);
        }

        public static IApplicationBuilder UseEventSourcing(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder;
        }

        public static IEventFlowOptions ConfigureEventStore(this IEventFlowOptions options, EventStoreSettings settings)
        {
            Uri eventStoreUri = GetUriFromConnectionString(settings.ConnectionString);

            var connectionSettings = ConnectionSettings.Create()
                .EnableVerboseLogging()
                .KeepReconnecting()
                .KeepRetrying()
                .SetDefaultUserCredentials(new UserCredentials(settings.Username, settings.Password))
                .Build();

            IEventFlowOptions eventFlowOptions = options
                .AddMetadataProvider<AddGuidMetadataProvider>()
                .UseEventStoreEventStore(eventStoreUri, connectionSettings);

            return eventFlowOptions;
        }

        private static Uri GetUriFromConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            string connectTo = (string)builder["ConnectTo"];

            return connectTo == null ? null : new Uri(connectTo);
        }
    }

}

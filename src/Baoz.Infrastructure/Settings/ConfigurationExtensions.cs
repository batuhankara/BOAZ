
using Microsoft.Extensions.Configuration;
using System;

namespace Baoz.Infrastructure.Settings
{
    public static class ConfigurationExtensions
    {
        public static RabbitMQSettings GetRabbitMQSettings(this IConfiguration config)
        {
            var rabbitMQSettings = new RabbitMQSettings();
            config.GetSection("RabbitMQ").Bind(rabbitMQSettings);

            if (rabbitMQSettings == null) throw new ArgumentException(nameof(rabbitMQSettings));
            if (string.IsNullOrWhiteSpace(rabbitMQSettings.Host)) throw new ArgumentNullException(nameof(rabbitMQSettings.Host));
            if (string.IsNullOrWhiteSpace(rabbitMQSettings.UserName)) throw new ArgumentNullException(nameof(rabbitMQSettings.UserName));
            if (string.IsNullOrWhiteSpace(rabbitMQSettings.Password)) throw new ArgumentNullException(nameof(rabbitMQSettings.Password));

            return rabbitMQSettings;
        }

        public static SnapshotStoreSettings GetSnapshotStoreSettings(this IConfiguration config)
        {
            var snapshotStoreSettings = new SnapshotStoreSettings();
            config.GetSection("SnapshotStore").Bind(snapshotStoreSettings);

            if (snapshotStoreSettings == null) throw new ArgumentException(nameof(snapshotStoreSettings));
            if (string.IsNullOrWhiteSpace(snapshotStoreSettings.ConnectionString)) throw new ArgumentNullException(nameof(snapshotStoreSettings.ConnectionString));
            if (string.IsNullOrWhiteSpace(snapshotStoreSettings.Name)) throw new ArgumentNullException(nameof(snapshotStoreSettings.Name));

            return snapshotStoreSettings;
        }

        public static EventStoreSettings GetEventStoreSettings(this IConfiguration config)
        {
            var eventStoreSettings = new EventStoreSettings();
            config.GetSection("EventStore").Bind(eventStoreSettings);

            if (eventStoreSettings == null) throw new ArgumentException(nameof(eventStoreSettings));
            if (string.IsNullOrWhiteSpace(eventStoreSettings.ConnectionString)) throw new ArgumentNullException(nameof(eventStoreSettings.ConnectionString));
            if (string.IsNullOrWhiteSpace(eventStoreSettings.Username)) throw new ArgumentNullException(nameof(eventStoreSettings.Username));
            if (string.IsNullOrWhiteSpace(eventStoreSettings.Password)) throw new ArgumentNullException(nameof(eventStoreSettings.Password));

            return eventStoreSettings;
        }

        public static string GetReadStoreConnectionString(this IConfiguration config)
        {
            string connectionString = config.GetValue<string>("BOAZDBConnection:ConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new NullReferenceException("MasterDb ConnectionString not found!");

            return config.GetValue<string>("BOAZDBConnection:ConnectionString");
        }

        public static string GetConfigStorageConnectionString(this IConfiguration config)
        {
            string connectionString = config.GetValue<string>("MasterAccountConnectionString:ConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new NullReferenceException("MasterAccountConnectionString not found!");

            return connectionString;
        }

        public static string GetEventStoreHttpConnectionString(this IConfiguration config)
        {
            string connectionString = config.GetValue<string>("EventStoreHttpConnectionString:ConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new NullReferenceException("EventStoreHttpConnectionString not found!");

            return connectionString;
        }

        public static string GetHangfireSqlConnectionString(this IConfiguration config)
        {
            string connectionString = config.GetValue<string>("HangfireSqlConnection:ConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new NullReferenceException("HangfireSqlConnection not found!");

            return connectionString;
        }
    }

}

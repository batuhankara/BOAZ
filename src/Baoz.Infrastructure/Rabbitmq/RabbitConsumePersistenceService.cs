using EventFlow.Aggregates;
using EventFlow.EventStores;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Integrations;
using EventFlow.Subscribers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace Baoz.Infrastructure.Rabbitmq
{
    public class RabbitConsumePersistenceService : IHostedService, IDisposable
    {
        private readonly IDispatchToEventSubscribers _dispatchToEventSubscribers;
        private readonly IEventJsonSerializer _eventJsonSerializer;
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IRabbitMqConnectionFactory _rabbitMqConnectionFactory;


        public RabbitConsumePersistenceService(
            IRabbitMqConnectionFactory rabbitMqConnectionFactory,
            IRabbitMqConfiguration rabbitMqConfiguration,
            IEventJsonSerializer eventJsonSerializer,
            IDispatchToEventSubscribers dispatchToEventSubscribers)
        {
            _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _eventJsonSerializer = eventJsonSerializer;
            _dispatchToEventSubscribers = dispatchToEventSubscribers;
        }

        public void Dispose()
        {
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connection =
                await _rabbitMqConnectionFactory.CreateConnectionAsync(_rabbitMqConfiguration.Uri, cancellationToken);
            await connection.WithModelAsync(model =>
            {
                model.ExchangeDeclare("wizlo", ExchangeType.Fanout);
                model.QueueDeclare("eventflow", false, false, true, null);
                model.QueueBind("eventflow", "wizlo", "");

                var consume = new EventingBasicConsumer(model);
                consume.Received += (obj, @event) =>
                {
                    var msg = CreateRabbitMqMessage(@event);
                    var domainEvent = _eventJsonSerializer.Deserialize(msg.Message, new Metadata(msg.Headers));

                    _dispatchToEventSubscribers.DispatchToAsynchronousSubscribersAsync(domainEvent, cancellationToken);
                };


                model.BasicConsume("eventflow", false, consume);
                return Task.CompletedTask;
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static RabbitMqMessage CreateRabbitMqMessage(BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var headers = basicDeliverEventArgs.BasicProperties.Headers.ToDictionary(kv => kv.Key,
                kv => Encoding.UTF8.GetString((byte[])kv.Value));
            var message = Encoding.UTF8.GetString(basicDeliverEventArgs.Body);

            return new RabbitMqMessage(
                message,
                headers,
                new Exchange(basicDeliverEventArgs.Exchange),
                new RoutingKey(basicDeliverEventArgs.RoutingKey),
                new MessageId(basicDeliverEventArgs.BasicProperties.MessageId));
        }
    }

}

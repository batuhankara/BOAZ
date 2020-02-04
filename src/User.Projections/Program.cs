
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Baoz.Infrastructure.Rabbitmq;
using BAOZ.Common;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.AspNetCore.Extensions;
using EventFlow.Configuration;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.Logs.Internals.Logging;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using EventFlow.RabbitMQ.Integrations;
using EventFlow.Subscribers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using User.Application;
using User.Core.Domain.Aggregates;
using User.Core.Domain.Events;
using User.Projections.Subscribers;

namespace User.Projections
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var builder = new HostBuilder()
                           .ConfigureServices(
                               (hostcontext, services) =>
                               {

                                   EventFlowOptions.New
                                       .Configure(cfg => cfg.IsAsynchronousSubscribersEnabled = true)
                                       .UseServiceCollection(services)
                                       .AddAspNetCore()
                                       .PublishToRabbitMq(RabbitMqConfiguration.With(new Uri($"amqp://test:test@127.0.0.1:5672"), true, 5, "wizlo"))
                                           .RegisterModule<UserModule>()
                                   .AddAsynchronousSubscriber<UserAggregate, BaozId, UserUpdatedEvent, UserSubscriberAsync>()
                                   .AddAsynchronousSubscriber<UserAggregate, BaozId, UserCreatedEvent, UserSubscriberAsync>()

                                           // subscribe services changed
                                           //
                                           .RegisterServices(s =>
                                           {
                                               s.Register<IHostedService, RabbitConsumePersistenceService>(Lifetime.Singleton);

                                               s.Register<IHostedService, UserSubscriberAsync>(Lifetime.Singleton);
                                           });
                               })
                           .ConfigureLogging((hostingContext, logging) => { });

            await builder.RunConsoleAsync();
        }

    }
   

}

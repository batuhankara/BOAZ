using Baoz.Infrastructure.Rabbitmq;
using BAOZ.Common;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Core.Domain.Aggregates;
using User.Core.Domain.Events;

namespace User.Projections.Subscribers
{
    public class UserSubscriberAsync : IHostedService, IRabbitMqConsumerPersistanceService,
        ISubscribeAsynchronousTo<UserAggregate, BaozId, UserUpdatedEvent>,
        ISubscribeAsynchronousTo<UserAggregate, BaozId, UserCreatedEvent>
    {



        public Task HandleAsync(IDomainEvent<UserAggregate, BaozId, UserUpdatedEvent> domainEvent, CancellationToken cancellationToken)
        {



            return Task.CompletedTask;
        }

        public Task HandleAsync(IDomainEvent<UserAggregate, BaozId, UserCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}

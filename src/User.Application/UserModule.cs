
using Baoz.Infrastructure.Rabbitmq;
using BAOZ.Common;
using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;
using Microsoft.Extensions.Hosting;
using User.Application.Subscribers;
using User.Core.Domain.Aggregates;
using User.Core.Domain.Events;

namespace User.Application
{
    public class UserModule : IModule
    {
        public void Register(IEventFlowOptions eventFlowOptions)
        {
            eventFlowOptions.AddDefaults(typeof(UserModule).Assembly);
            eventFlowOptions.AddDefaults(typeof(UserUpdatedEvent).Assembly);
            eventFlowOptions.AddDefaults(typeof(UserCreatedEvent).Assembly);
          
        }
    }
}

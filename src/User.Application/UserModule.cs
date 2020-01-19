
using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;
using User.Core.Domain.Events;

namespace User.Application
{
   public class UserModule : IModule
    {
        public void Register(IEventFlowOptions eventFlowOptions)
        {
            eventFlowOptions.AddDefaults(typeof(UserModule).Assembly);
            eventFlowOptions.AddDefaults(typeof(UserCreatedEvent).Assembly);
        }
    }
}

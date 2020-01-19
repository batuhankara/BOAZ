using BAOZ.Common;
using EventFlow.Aggregates.ExecutionResults;
using System;
using System.Collections.Generic;
using System.Text;
using User.Core.Domain.Commands;
using User.Core.Domain.Events;

namespace User.Core.Domain.Aggregates
{
    public class UserAggregate : AggregateRootBase<UserAggregate, BaozId>
    {
        public UserAggregate(BaozId id) : base(id)
        {
        }
        public string FirstName { get; set; }


        #region Aggregate methods
        public IExecutionResult Create(CreateUserCommand command)
        {

            var @event = new UserCreatedEvent(
               command.UserId,
               command.FirstName
                );

            Emit(@event);

            return ExecutionResult.Success();
        }

        #endregion
        #region Apply methods
        public void Apply(UserCreatedEvent domainEvent)
        {
            this.FirstName = domainEvent.FirstName;
        }
        #endregion
    }
}

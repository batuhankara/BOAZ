using User.Core.Domain;
using User.Core.Domain.Aggregates;
using User.Core.Domain.Commands;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

using BAOZ.Common;

namespace User.Application.CommandHandlers
{
    public class CreateUserCommandHandler : CommandHandler<UserAggregate, BaozId, CommandResult, CreateUserCommand>
    {
        public CreateUserCommandHandler()
        {

        }
        public override async Task<CommandResult> ExecuteCommandAsync(
           UserAggregate aggregate,
           CreateUserCommand command,
           CancellationToken cancellationToken)
        {
            IExecutionResult createResult = aggregate.Create(command);

            return await Task.FromResult(new CommandResult(createResult.IsSuccess, aggregate));
        }
    }
}

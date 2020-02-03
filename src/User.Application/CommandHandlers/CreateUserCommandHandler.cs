using User.Core.Domain;
using User.Core.Domain.Aggregates;
using User.Core.Domain.Commands;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using BAOZ.Common;
using BAOZ.Common.Helpers;
using User.Core.Domain.Repositories;

namespace User.Application.CommandHandlers
{
    public class UserCommandHandler :
        ICommandHandler<UserAggregate, BaozId, CommandResult, CreateUserCommand>,
        ICommandHandler<UserAggregate, BaozId, CommandResult, UpdateUserCommand>
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        public UserCommandHandler(IPasswordService passwordService, IUserRepository userRepository)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }
        public async Task<CommandResult> ExecuteCommandAsync(
           UserAggregate aggregate,
           CreateUserCommand command,
           CancellationToken cancellationToken)
        {
            if (_userRepository.Any(x => x.FullPhoneNumber == command.CountryCode + command.PhoneNumber))
            {
                throw new Exception("User Already Exist");
            }

            IExecutionResult createResult = aggregate.Create(command, _passwordService);

            return await Task.FromResult(new CommandResult(createResult.IsSuccess, aggregate));
        }

        public async Task<CommandResult> ExecuteCommandAsync(UserAggregate aggregate, UpdateUserCommand command, CancellationToken cancellationToken)
        {
            IExecutionResult createResult = aggregate.Create(command);
            return await Task.FromResult(new CommandResult(createResult.IsSuccess, aggregate));

        }
    }
}

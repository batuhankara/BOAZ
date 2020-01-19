using Baoz.Infrastructure.SqlServer.Contracts;
using BAOZ.Common;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Core.Domain.Aggregates;
using User.Core.Domain.Database;
using User.Core.Domain.Entities;
using User.Core.Domain.Events;
using User.Core.Domain.Repositories;

namespace User.Application.Subscribers
{

    class CreateUserEventSubscriber : ISubscribeSynchronousTo<UserAggregate, BaozId, UserCreatedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<IUserSqlDbContext> _unitOfWork;

        public CreateUserEventSubscriber(IUserRepository userRepository, IUnitOfWork<IUserSqlDbContext> unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(IDomainEvent<UserAggregate, BaozId, UserCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            var user = new UserView
            {
                Id = domainEvent.AggregateEvent.UserId,
                FirstName = domainEvent.AggregateEvent.FirstName
            };
            _userRepository.Add(user);
            await _unitOfWork.CommitAsync();
        }
    }
}

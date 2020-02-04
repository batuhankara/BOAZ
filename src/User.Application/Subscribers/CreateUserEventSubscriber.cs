using Baoz.Infrastructure.SqlServer.Contracts;
using BAOZ.Common;
using BAOZ.Common.Helpers;
using BAOZ.Common.Models.Dtos;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
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
    public class UserEventSubscriber :
       ISubscribeSynchronousTo<UserAggregate, BaozId, UserCreatedEvent>,
       ISubscribeSynchronousTo<UserAggregate, BaozId, UserUpdatedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<IUserSqlDbContext> _unitOfWork;

        public UserEventSubscriber(IUserRepository userRepository, IUnitOfWork<IUserSqlDbContext> unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(IDomainEvent<UserAggregate, BaozId, UserCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            var @event = domainEvent.AggregateEvent;
            var user = new UserView
            {
                Id = @event.UserId,
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                PasswordSalt = @event.PasswordSalt,
                PasswordHash = @event.PasswordHash,
                PhoneNumber = @event.PhoneNumber,
                CountryCode = @event.CountryCode,
                FullPhoneNumber = @event.FullPhoneNumber
            };
            _userRepository.Add(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task HandleAsync(IDomainEvent<UserAggregate, BaozId, UserUpdatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            var entity = await _userRepository.GetAsync(x => x.Id == domainEvent.AggregateEvent.Id);
            entity.FirstName = domainEvent.AggregateEvent.FirstName;

            _unitOfWork.ChangeAutoDetectChangesStatus(true);
            var res = await _unitOfWork.CommitAsync();
        }
    }
}

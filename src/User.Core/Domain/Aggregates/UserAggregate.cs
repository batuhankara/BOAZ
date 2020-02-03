using BAOZ.Common;
using BAOZ.Common.Helpers;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.ValueObjects;
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
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string FullPhoneNumber => CountryCode + PhoneNumber;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


        #region Aggregate methods
        public IExecutionResult Create(CreateUserCommand command, IPasswordService _service)
        {
            byte[] passwordHash, passwordSalt;

            _service.CreatePasswordHash(command.Password, out passwordHash, out passwordSalt);
            var @event = new UserCreatedEvent(
               command.UserId,
               command.FirstName,
               command.LastName,
               command.PhoneNumber,
               command.CountryCode,
               passwordHash,
               passwordSalt
                );

            Emit(@event);

            return ExecutionResult.Success();
        }
        public IExecutionResult Create(UpdateUserCommand command)
        {

            var @event = new UserUpdatedEvent(
               command.Id,
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
            this.CountryCode = domainEvent.CountryCode;
            this.LastName = domainEvent.LastName;
            this.PhoneNumber = domainEvent.PhoneNumber;
            this.PasswordHash = domainEvent.PasswordHash;
            this.PasswordSalt = domainEvent.PasswordSalt;
        }
        public void Apply(UserUpdatedEvent domainEvent)
        {
            this.FirstName = domainEvent.FirstName;
        }
        #endregion
    }
}

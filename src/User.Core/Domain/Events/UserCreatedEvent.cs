using BAOZ.Common;
using EventFlow.EventStores;
using System;
using System.Collections.Generic;
using System.Text;
using User.Core.Domain.Aggregates;

namespace User.Core.Domain.Events
{
    [EventVersion("UserCreated", 1)]

    public class UserCreatedEvent : BaseEvent<UserAggregate>
    {
        public UserCreatedEvent(Guid userId, string firstName, string lastName, string phoneNumber, string countryCode, byte[] passwordHash, byte[] passwordSalt)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            CountryCode = countryCode;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string FullPhoneNumber => CountryCode + PhoneNumber;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

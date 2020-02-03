using Baoz.Infrastructure.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace User.Core.Domain.Entities
{
    public class UserView : SqlPersistedAggregate<Guid>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string FullPhoneNumber { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}

using Baoz.Infrastructure.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace User.Core.Domain.Entities
{
    public class UserView : SqlPersistedAggregate<Guid>
    {
        public string FirstName { get; set; }
    }
}

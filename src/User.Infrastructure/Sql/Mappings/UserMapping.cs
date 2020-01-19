using Baoz.Infrastructure.SqlServer.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using User.Core.Domain.Entities;
namespace User.Infrastructure.Sql.Mappings
{
    public class UserMapping : BaseEntityMapping<Core.Domain.Entities.UserView, Guid>
    {
        public override void Configure(EntityTypeBuilder<Core.Domain.Entities.UserView> builder)
        {
            base.Configure(builder);

            builder.ToTable("Users");

            builder.HasIndex(b => b.FirstName).IsUnique(true);

        }
    }
}


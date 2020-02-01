using Baoz.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Baoz.Infrastructure.SqlServer.Mappings
{
    public abstract class BaseEntityMapping<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TId>
    where TId : struct
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(b => b.Id);

            if (typeof(TId) == typeof(Guid))
            {
                builder.Property(x => x.DbId).ValueGeneratedOnAdd();
                builder.Property(b => b.CreatedAtUTC);
                builder.Property(b => b.UpdatedAtUTC);
            }
            else
            {
                builder.Ignore(x => x.DbId);
            }

         
        }

    
    }

}

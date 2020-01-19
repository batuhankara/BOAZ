using Baoz.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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

            UseRowVersion(builder);
        }

        protected virtual void UseRowVersion(EntityTypeBuilder<TEntity> builder)
        {
            // Concurrency
            builder.Property(b => b.RowVersion)
                .IsRequired()
                .HasMaxLength(8)
                .IsRowVersion();
        }
    }

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baoz.Infrastructure.SqlServer.Models
{
       public abstract class BaseEntity<TId>
    {
        public BaseEntity()
        {
            CreatedAtUTC = DateTime.UtcNow;
        }

        public int DbId { get; set; }

        [Column(Order = 1)]
        public TId Id { get; set; }

        [Column(Order = 2)]
        public DateTime CreatedAtUTC { get; set; }

        [Column(Order = 3)]
        public DateTime? UpdatedAtUTC { get; set; }

        [JsonIgnore]
        public byte[] RowVersion { get; set; }

        public override bool Equals(object entity)
        {
            return Equals(entity as BaseEntity<TId>);
        }

        public override int GetHashCode()
        {
            return Equals(Id, default(TId)) ? base.GetHashCode() : Id.GetHashCode();
        }

        protected virtual bool Equals(BaseEntity<TId> otherEntity)
        {
            if (otherEntity == null)
                return false;

            if (ReferenceEquals(this, otherEntity))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(otherEntity) &&
                Equals(Id, otherEntity.Id))
            {
                var otherType = otherEntity.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public static bool operator ==(BaseEntity<TId> x, BaseEntity<TId> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity<TId> x, BaseEntity<TId> y)
        {
            return !(x == y);
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        private static bool IsTransient(BaseEntity<TId> entity)
        {
            return entity != null && Equals(entity.Id, default(TId));
        }
    }

}

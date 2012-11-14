using System;
using System.Runtime.Serialization;

namespace SHHH.Infrastructure
{
    [DataContract]
    public class Entity<TId>
    {
        [DataMember]
        public virtual TId Id { get; protected set; }

        public Entity()
        {
            Id = default(TId);
        }

        private static bool IsTransient(Entity<TId> entity)
        {
            return (entity != null && Equals(entity.Id, default(TId)));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public override bool Equals(object obj)
        {
            Entity<TId> other = obj as Entity<TId>;
            if (other == null) return false;

            return Equals(other);
        }
        public virtual bool Equals(Entity<TId> obj)
        {
            if (obj == null) return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (IsTransient(this) || IsTransient(obj) || !Equals(Id, obj.Id)) return false;

            Type otherType = obj.GetUnproxiedType();
            Type thisType = this.GetUnproxiedType();

            return otherType.IsAssignableFrom(thisType) || thisType.IsAssignableFrom(otherType);
        }

        public override int GetHashCode()
        {
            if (IsTransient(this))
                return base.GetHashCode();
            else
                return Id.GetHashCode();
        }
    }
}
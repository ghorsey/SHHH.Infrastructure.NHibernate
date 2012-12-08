// <copyright file="Entity.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a class that is persisted.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    [DataContract]
    public class Entity<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TId}" /> class.
        /// </summary>
        public Entity()
        {
            this.Id = default(TId);
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public virtual TId Id { get; protected set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Entity<TId> other = obj as Entity<TId>;
            if (other == null)
            {
                return false;
            }

            return this.Equals(other);
        }

        /// <summary>
        /// Equals the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>true of the specified object equals this object; otherwise false</returns>
        public virtual bool Equals(Entity<TId> obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (IsTransient(this) || IsTransient(obj) || !Entity<TId>.Equals(this.Id, obj.Id))
            {
                return false;
            }

            Type otherType = obj.GetUnproxiedType();
            Type thisType = this.GetUnproxiedType();

            return otherType.IsAssignableFrom(thisType) || thisType.IsAssignableFrom(otherType);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            if (IsTransient(this))
            {
                return base.GetHashCode();
            }
            else
            {
                return this.Id.GetHashCode();
            }
        }

        /// <summary>
        /// Determines whether the specified entity is transient.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is transient; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTransient(Entity<TId> entity)
        {
            return entity != null && Entity<TId>.Equals(entity.Id, default(TId));
        }

        /// <summary>
        /// Gets the type of the base class (non-proxy class).
        /// </summary>
        /// <returns><see cref="System.Type"/></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }
    }
}
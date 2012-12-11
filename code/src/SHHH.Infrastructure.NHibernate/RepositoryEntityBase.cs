// <copyright file="RepositoryEntityBase.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure.NHibernate
{
    using System.Data;
    using global::NHibernate;

    /// <summary>
    /// The base class for Entity based repositories
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity{TId}"/></typeparam>
    /// <typeparam name="TId">The type of the id if the <see cref="Entity{TID}" />.</typeparam>
    public class RepositoryEntityBase<T, TId> : RepositoryBase where T : Entity<TId>
    {
        /// <summary>
        /// Loads the specified <see cref="Entity{TId}"/> by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="lockMode">The lock mode.</param>
        /// <returns>The <see cref="Entity{TId}"/></returns>
        public T Load(TId id, IsolationLevel isolationLevel = IsolationLevel.Unspecified, LockMode lockMode = null)
        {
            lockMode = lockMode ?? LockMode.None;
            return this.RunInTransaction<T>(
                session =>
                {
                    return session.Load<T>(id, lockMode);
                }, 
                isolationLevel);
        }
    }
}

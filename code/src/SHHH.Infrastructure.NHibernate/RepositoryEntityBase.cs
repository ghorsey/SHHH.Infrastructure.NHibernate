using System.Data;
using NHibernate;

namespace SHHH.Infrastructure.NHibernate
{
    public class RepositoryEntityBase<T, TId> : RepositoryBase where T: Entity<TId>
    {
        public T Load(TId id, IsolationLevel isolationLevel = IsolationLevel.Unspecified, LockMode lockMode = null)
        {
            lockMode = lockMode ?? LockMode.None;
            return RunInTransaction<T>(session =>
            {
                return session.Load<T>(id, lockMode);
            }, isolationLevel);
        }
    }
}

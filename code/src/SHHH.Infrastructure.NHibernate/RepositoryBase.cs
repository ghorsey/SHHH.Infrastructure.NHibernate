using System;
using NHibernate;

namespace SHHH.Infrastructure.NHibernate
{
    public abstract class RepositoryBase
    {
        protected void RunInTransaction(Action<ISession> f)
        {
            var session = SessionSource.GetSession();
            using (var trans = session.BeginTransaction())
            {
                f(session);
                trans.Commit();
            }
        }

    }
}

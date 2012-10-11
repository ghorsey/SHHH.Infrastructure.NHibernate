using System;
using NHibernate;
using System.Data;

namespace SHHH.Infrastructure.NHibernate
{
    public abstract class RepositoryBase
    {
        protected void RunInTransaction(Action<ISession> f, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var session = SessionSource.GetSession();
            using (var trans = session.BeginTransaction(isolationLevel))
            {
                f(session);

                if (trans.IsActive)
                    trans.Commit();
            }
        }

    }
}

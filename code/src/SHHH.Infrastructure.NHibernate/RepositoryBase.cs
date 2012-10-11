using System;
using NHibernate;
using System.Data;
using Common.Logging;

namespace SHHH.Infrastructure.NHibernate
{
    public abstract class RepositoryBase
    {
        ILog Log = LogManager.GetCurrentClassLogger();
        protected void RunInTransaction(Action<ISession> f, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var session = SessionSource.GetSession();
            using (var trans = session.BeginTransaction(isolationLevel))
            {
                try
                {
                    f(session);

                    if (trans.IsActive)
                        trans.Commit();
                }
                catch (Exception x)
                {
                    if (trans.IsActive)
                        trans.Rollback();

                    Log.Error("Failed to run in transaction", x);
                    throw;
                }
            }
        }

        protected TOut RunInTransaction<TOut>( Func<ISession, TOut> f, IsolationLevel isolationLevel = IsolationLevel.Unspecified) where TOut : class
        {
            var session = SessionSource.GetSession();
            using( var trans = session.BeginTransaction(isolationLevel))
            {
                try
                {
                    var result = f(session);
                    if (trans.IsActive)
                        trans.Commit();

                    return result;
                }
                catch (Exception x)
                {
                    if (trans.IsActive)
                        trans.Rollback();

                    Log.Error("Failed to execute a result in a transaction", x);
                    throw;
                }
            }
        }
    }
}

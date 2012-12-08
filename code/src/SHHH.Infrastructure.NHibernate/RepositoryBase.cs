// <copyright file="RepositoryBase.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure.NHibernate
{
    using System;
    using System.Data;
    using Common.Logging;
    using global::NHibernate;

    /// <summary>
    /// A base class for Repositories
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILog logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Runs the in transaction.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        protected void RunInTransaction(Action<ISession> f, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var session = SessionSource.GetSession();
            using (var trans = session.BeginTransaction(isolationLevel))
            {
                try
                {
                    f(session);

                    if (trans.IsActive)
                    {
                        trans.Commit();
                    }
                }
                catch (Exception x)
                {
                    if (trans.IsActive)
                    {
                        trans.Rollback();
                    }

                    this.logger.Error("Failed to run in transaction", x);
                    throw;
                }
            }
        }

        /// <summary>
        /// Runs the in transaction.
        /// </summary>
        /// <typeparam name="TOut">The type of the out.</typeparam>
        /// <param name="f">The f.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The TOut</returns>
        protected TOut RunInTransaction<TOut>(Func<ISession, TOut> f, IsolationLevel isolationLevel = IsolationLevel.Unspecified) where TOut : class
        {
            var session = SessionSource.GetSession();
            using (var trans = session.BeginTransaction(isolationLevel))
            {
                try
                {
                    var result = f(session);

                    if (trans.IsActive)
                    {
                        trans.Commit();
                    }

                    return result;
                }
                catch (Exception x)
                {
                    if (trans.IsActive)
                    {
                        trans.Rollback();
                    }

                    this.logger.Error("Failed to execute a result in a transaction", x);
                    throw;
                }
            }
        }
    }
}
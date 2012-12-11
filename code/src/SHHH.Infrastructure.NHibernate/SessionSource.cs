// <copyright file="SessionSource.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure.NHibernate
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Common.Logging;
    using FluentNHibernate.Cfg;
    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NHibernate.Context;

    /// <summary>
    /// The NHibernate session source
    /// </summary>
    public static class SessionSource
    {
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The object used to lock threads
        /// </summary>
        private static object lck = new object();

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static Configuration Configuration { get; private set; }

        /// <summary>
        /// Gets or sets the factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        private static ISessionFactory Factory { get; set; }

        /// <summary>
        /// Builds the session factory.
        /// </summary>
        /// <param name="sources">The <see cref="IEnumerable{T}"/> of <see cref="System.Reflection.Assembly"/> sources.</param>
        /// <param name="configurationFile">The configuration file.</param>
        public static void BuildSessionFactory(IEnumerable<Assembly> sources, string configurationFile = null)
        {
            if (Factory != null)
            {
                return;
            }

            Log.Info("Building SessionFactory");
            
            lock (lck)
            {
                Configuration = new Configuration();
                Configuration.Configure(configurationFile ?? CreateConfigurationFile());

                Factory = Fluently.Configure(Configuration).Mappings(m =>
                {
                    foreach (var assembly in sources)
                    {
                        Log.Info(x => x("Adding Mappings from assembly: {0}", assembly.FullName));
                        m.FluentMappings.AddFromAssembly(assembly);
                    }
                })
                .ExposeConfiguration(xConf => xConf.SetListener(global::NHibernate.Event.ListenerType.Delete, new DeleteEventListener()))
                .BuildSessionFactory();
            }
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>An <see cref="global::NHibernate.ISession"/></returns>
        /// <exception cref="System.InvalidOperationException">NHibernate SessionFactory cannot be null</exception>
        public static ISession GetSession()
        {
            if (Factory == null)
            {
                throw new InvalidOperationException("NHibernate SessionFactory cannot be null");
            }

            ISession session;
            if (CurrentSessionContext.HasBind(Factory))
            {
                session = Factory.GetCurrentSession();
            }
            else
            {
                session = Factory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

            return session;
        }

        /// <summary>
        /// Ends the context session.
        /// </summary>
        public static void EndContextSession()
        {
            var session = CurrentSessionContext.Unbind(Factory);
            if (session != null && session.IsOpen)
            {
                try
                {
                    if (session.Transaction != null && session.Transaction.IsActive)
                    {
                        // an unhandled exception has occurred and no db commit should be made
                        session.Transaction.Rollback();
                    }
                }
                finally
                {
                    session.Close();
                    session.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates the configuration file.
        /// </summary>
        /// <returns>The default path to the configuration file</returns>
        private static string CreateConfigurationFile()
        {
            string configFile = typeof(SessionSource).Assembly.GetName().Name + ".cfg.xml";
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
        }
    }
}

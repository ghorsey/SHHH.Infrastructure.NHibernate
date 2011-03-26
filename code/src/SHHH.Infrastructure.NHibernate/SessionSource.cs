using System;
using System.Linq;
using System.IO;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using Common.Logging;

namespace SHHH.Infrastructure.NHibernate
{
    public static class SessionSource
    {
        readonly static ILog _log = LogManager.GetCurrentClassLogger();
        static string CreateConfigurationFile()
        {
            string configFile = typeof(SessionSource).Assembly.GetName().Name + ".cfg.xml";
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
        }
        static object lck = new object();
        private static void BuildSessionFactory()
        {
            if (_factory != null) return;

            _log.Info("Building SessionFactory");
            Configuration cfg = new Configuration();
            cfg.Configure(CreateConfigurationFile());
            
            IEnumerable<IMappingSource> sources = ServiceLocator.Current.GetAllInstances<IMappingSource>();
            lock (lck)
            {
                _factory = Fluently.Configure(cfg).Mappings(m =>
                {
                    foreach (IMappingSource source in sources)
                        foreach (Assembly assembly in source.MappingSources())
                        {
                            _log.Info(x => x("Adding Mappings from assembly: {0}", assembly.FullName)); 
                            m.FluentMappings.AddFromAssembly(assembly);
                        }
                })
                .BuildSessionFactory();
            }
        }

        private static ISessionFactory _factory;

        private static ISessionFactory Factory
        {
            get
            {
                if (_factory == null)
                    BuildSessionFactory();
                return _factory;
            }
        
        }

        public static ISession GetSession()
        {
            ISession session;
            if (CurrentSessionContext.HasBind(Factory))
                session = Factory.GetCurrentSession();
            else
            {
                session = Factory.OpenSession();
                CurrentSessionContext.Bind(session);
            }
            return session;
        }

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
    }
}

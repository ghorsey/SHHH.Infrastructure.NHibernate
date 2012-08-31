using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Common.Logging;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace SHHH.Infrastructure.NHibernate
{
    public static class SessionSource
    {
        readonly static ILog Log = LogManager.GetCurrentClassLogger();
        public static Configuration Configuration { get; private set; }
        static string CreateConfigurationFile()
        {
            string configFile = typeof(SessionSource).Assembly.GetName().Name + ".cfg.xml";
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
        }
        static object lck = new object();
        public static void BuildSessionFactory(IEnumerable<Assembly> sources, string configurationFile = null)
        {
            if (Factory != null) return;

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

        private static ISessionFactory Factory { get; set; }

        public static ISession GetSession()
        {
            if (Factory == null) throw new InvalidOperationException("NHibernate SessionFactory cannot be null");
            return Factory.OpenSession();
        }

        public static void EndContextSession()
        {
            //var session = CurrentSessionContext.Unbind(Factory);
            //if (session != null && session.IsOpen)
            //{
            //    try
            //    {
            //        if (session.Transaction != null && session.Transaction.IsActive)
            //        {
            //            // an unhandled exception has occurred and no db commit should be made
            //            session.Transaction.Rollback();
            //        }
            //    }
            //    finally
            //    {
            //        session.Close();
            //        session.Dispose();
            //    }
            //}
        }
    }
}

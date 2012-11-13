using System;
using System.Collections;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace SHHH.Infrastructure.NHibernate.Web
{
    [Serializable]
    public class HybridWebSessionContext : MapBasedSessionContext
    {
        private const string SessionFactoryMapKey = "SHHH.Infrastructure.NHibernate.Mvc.HybridWebSessionContextKey";
        [ThreadStatic]
        private static ISession _session;

        public HybridWebSessionContext(ISessionFactoryImplementor factory) : base(factory) { }

        protected override ISession Session
        {
            get
            {
                if (HttpContext.Current != null)
                    return base.Session;
                else
                    return _session;
            }
            set
            {
                if (HttpContext.Current != null)
                    base.Session = value;
                else
                    _session = value;
            }
        }

        
        protected override System.Collections.IDictionary GetMap()
        {
            return ReflectiveHttpContext.HttpContextCurrentItems[SessionFactoryMapKey] as IDictionary;
        }
        protected override void SetMap(IDictionary value)
        {
            ReflectiveHttpContext.HttpContextCurrentItems[SessionFactoryMapKey] = value;
        }
    }
}
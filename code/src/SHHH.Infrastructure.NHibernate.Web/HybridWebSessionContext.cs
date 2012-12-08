// <copyright file="HybridWebSessionContext.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure.NHibernate.Web
{
    using System;
    using System.Collections;
    using System.Web;
    using global::NHibernate;
    using global::NHibernate.Context;
    using global::NHibernate.Engine;

    /// <summary>
    /// A hybrid web session context that uses the standard web context and the thread static context
    /// </summary>
    [Serializable]
    public class HybridWebSessionContext : MapBasedSessionContext
    {
        /// <summary>
        /// The session factory map key
        /// </summary>
        private const string SessionFactoryMapKey = "SHHH.Infrastructure.NHibernate.Mvc.HybridWebSessionContextKey";

        /// <summary>
        /// The session
        /// </summary>
        [ThreadStatic]
        private static ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridWebSessionContext" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public HybridWebSessionContext(ISessionFactoryImplementor factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Gets or sets the currently bound session.
        /// </summary>
        protected override ISession Session
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return base.Session;
                }
                else
                {
                    return session;
                }
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    base.Session = value;
                }
                else
                {
                    session = value;
                }
            }
        }

        /// <summary>
        /// Get the dictionary mapping session factory to its current session.
        /// </summary>
        /// <returns>A <see cref="System.Collections.IDictionary"/></returns>
        protected override System.Collections.IDictionary GetMap()
        {
            return ReflectiveHttpContext.HttpContextCurrentItems[SessionFactoryMapKey] as IDictionary;
        }

        /// <summary>
        /// Set the map mapping session factory to its current session.
        /// </summary>
        /// <param name="value">The <see cref="IDictionary"/></param>
        protected override void SetMap(IDictionary value)
        {
            ReflectiveHttpContext.HttpContextCurrentItems[SessionFactoryMapKey] = value;
        }
    }
}
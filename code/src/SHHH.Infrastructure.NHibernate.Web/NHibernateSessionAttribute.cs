// <copyright file="NHibernateSessionAttribute.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure.NHibernate.Web
{
    using System.Web.Mvc;

    /// <summary>
    /// An <see cref="ActionFilterAttribute"/> that closes any open NHibernate <see cref="NHibernate.ISession"/>
    /// </summary>
    public class NHibernateSessionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateSessionAttribute" /> class.
        /// </summary>
        public NHibernateSessionAttribute()
        {
            this.Order = 100;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }

            SessionSource.EndContextSession();
        }
    }
}

// <copyright file="DeleteEventListener.cs" company="SHHH Innovations LLC">
// Copyright SHHH Innovations LLC
// </copyright>

namespace SHHH.Infrastructure.NHibernate
{
    using System.Diagnostics.CodeAnalysis;
    using global::NHibernate.Event.Default;

    /// <summary>
    /// An NHibernate event listener for the Delete event
    /// </summary>
    public class DeleteEventListener : DefaultDeleteEventListener
    {
        /// <summary>
        /// Perform the entity deletion.  Well, as with most operations, does not
        /// really perform it; just schedules an action/execution with the
        /// <see cref="T:NHibernate.Engine.ActionQueue" /> for execution during flush.
        /// </summary>
        /// <param name="session">The originating session</param>
        /// <param name="entity">The entity to delete</param>
        /// <param name="entityEntry">The entity's entry in the <see cref="T:NHibernate.ISession" /></param>
        /// <param name="isCascadeDeleteEnabled">Is delete cascading enabled?</param>
        /// <param name="persister">The entity persister.</param>
        /// <param name="transientEntities">A cache of already deleted entities.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        protected override void DeleteEntity(global::NHibernate.Event.IEventSource session, object entity, global::NHibernate.Engine.EntityEntry entityEntry, bool isCascadeDeleteEnabled, global::NHibernate.Persister.Entity.IEntityPersister persister, Iesi.Collections.ISet transientEntities)
        {
            ILogicalDelete logicalDelete = entity as ILogicalDelete;
            if (logicalDelete != null)
            {
                logicalDelete.IsDeleted = true;

                this.CascadeBeforeDelete(session, persister, entity, entityEntry, transientEntities);
                this.CascadeAfterDelete(session, persister, entity, transientEntities);
            }
            else
            {
                base.DeleteEntity(session, entity, entityEntry, isCascadeDeleteEnabled, persister, transientEntities);
            }
        }
    }
}

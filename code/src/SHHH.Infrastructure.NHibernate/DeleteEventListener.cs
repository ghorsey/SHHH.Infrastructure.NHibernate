using NHibernate.Event.Default;

namespace SHHH.Infrastructure.NHibernate
{
    public class DeleteEventListener : DefaultDeleteEventListener
    {
        protected override void DeleteEntity(global::NHibernate.Event.IEventSource session, object entity, global::NHibernate.Engine.EntityEntry entityEntry, bool isCascadeDeleteEnabled, global::NHibernate.Persister.Entity.IEntityPersister persister, Iesi.Collections.ISet transientEntities)
        {
            ILogicalDelete logicalDelete = entity as ILogicalDelete;
            if (logicalDelete != null)
            {
                logicalDelete.IsDeleted = true;

                CascadeBeforeDelete(session, persister, entity, entityEntry, transientEntities);
                CascadeAfterDelete(session, persister, entity, transientEntities);
            }
            else
                base.DeleteEntity(session, entity, entityEntry, isCascadeDeleteEnabled, persister, transientEntities);
        }
    }
}

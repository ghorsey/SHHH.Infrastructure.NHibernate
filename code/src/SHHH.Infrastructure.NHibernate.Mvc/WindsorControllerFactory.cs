using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SHHH.Infrastructure.NHibernate.Mvc.Resources;

namespace SHHH.Infrastructure.NHibernate.Mvc
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        public IWindsorContainer Container { get; private set; }

        public WindsorControllerFactory(WindsorContainer container, string[] assemblyNames)
        {
            Container = container;

            foreach (string assemblyName in assemblyNames)
            {
                var q = from x in Assembly.Load(assemblyName).GetTypes()
                        where typeof(IController).IsAssignableFrom(x)
                        select x;
                
                foreach (Type c in q)
                    container.Register(Component.For(c).Named(c.FullName).LifeStyle.Is(Castle.Core.LifestyleType.Transient));
            }
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                throw new HttpException(404, string.Format(
                    CultureInfo.CurrentUICulture,
                    MvcResources.WindsorControllerFactor_ControllerNotFound,
                    requestContext.HttpContext.Request.Path));

            return Container.Resolve(controllerType) as IController;
        }

        public override void ReleaseController(IController controller)
        {
            Container.Release(controller);
            base.ReleaseController(controller);
        }
    }
}

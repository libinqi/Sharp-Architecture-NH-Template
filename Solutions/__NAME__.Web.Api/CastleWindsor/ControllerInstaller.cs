using System.Web.Http;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SharpArch.Web.Http.Castle;

namespace __NAME__.Api.Web.CastleWindsor
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                   .BasedOn<ApiController>()
                   .LifestyleTransient());

            container.Register(
                    Component
                        .For(typeof(System.Web.Http.Dispatcher.IHttpControllerActivator))
                            .ImplementedBy(typeof(WindsorHttpControllerActivator))
                            .LifeStyle.Transient);

            container.Register(
                     Component
                          .For(typeof(System.Web.Http.Filters.IFilterProvider))
                             .ImplementedBy(typeof(WindsorFilterProvider))
                             .LifeStyle.Transient);
        }
    }
}
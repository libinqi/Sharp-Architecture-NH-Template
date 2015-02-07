using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace __NAME__.Api.Web.CastleWindsor
{
    public class TasksInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyNamed("__NAME__.Tasks")
                    .Pick().Unless(t => t.Namespace.EndsWith("Handlers"))
                    .WithService.DefaultInterfaces()
                    .Configure(c => c.LifestyleScoped())
                );
        }
    }
}
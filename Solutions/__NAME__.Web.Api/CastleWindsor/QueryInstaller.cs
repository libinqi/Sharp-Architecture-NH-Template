using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace __NAME__.Api.Web.CastleWindsor
{
    public class QueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyNamed("__NAME__.Web.Api")
                    .InNamespace("__NAME__.Web.Api.Controllers.Queries", true)
                    .WithService.DefaultInterfaces()
                    .LifestylePerWebRequest());
        }
    }
}
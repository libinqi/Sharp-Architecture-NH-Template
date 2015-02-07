using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SharpArch.Domain.Commands;
using SharpArch.Domain.Events;

namespace __NAME__.Api.Web.CastleWindsor
{
    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyNamed("__NAME__.Tasks")
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface().LifestyleScoped());

            container.Register(
                Classes.FromAssemblyNamed("__NAME__.Tasks")
                    .BasedOn(typeof(ICommandHandler<,>))
                    .WithService.FirstInterface().LifestyleScoped());

            container.Register(
                Classes.FromAssemblyNamed("__NAME__.Tasks")
                    .BasedOn(typeof(IHandles<>))
                    .WithService.FirstInterface().LifestyleScoped());
        }
    }
}
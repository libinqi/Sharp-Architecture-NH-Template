using Castle.Core;
using Castle.Core.Configuration;
using Castle.DynamicProxy;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;
using SharpArch.Web.Http.Castle;

namespace __NAME__.Api.Web.CastleWindsor
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddCustomRepositoriesTo(container);
            AddWcfServiceFactoriesTo(container);
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes.Pick().FromAssemblyNamed("__NAME__.Infrastructure").WithService.FirstNonGenericCoreInterface(
                    "__NAME__.Domain"));

            container.Register(
                AllTypes.Pick().FromAssemblyNamed("__NAME__.Tasks").WithService.FirstNonGenericCoreInterface(
                    "__NAME__.Domain.Contracts.Tasks"));
        }     

        private static void AddWcfServiceFactoriesTo(IWindsorContainer container)
        {
        }
    }
}
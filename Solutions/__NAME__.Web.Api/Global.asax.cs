using __NAME__.Api.Web.CastleWindsor;
using __NAME__.Infrastructure.NHibernateMaps;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CommonServiceLocator.WindsorAdapter;
using log4net.Config;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Domain.Events;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Web.Mvc;
using SharpArch.Web.Http.Castle;
using SharpArch.Web.Mvc.ModelBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace __NAME__.Web.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private WebSessionStorage webSessionStorage;
        public override void Init()
        {
            base.Init();
            this.webSessionStorage = new WebSessionStorage(this);
        }

        /// <summary>
        ///   Due to issues on IIS7, the NHibernate initialization cannot reside in Init() but
        ///   must only be called once.  Consequently, we invoke a thread-safe singleton class to 
        ///   ensure it's only initialized once.
        /// </summary>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(() => this.InitializeNHibernateSession());
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Useful for debugging
            var ex = this.Server.GetLastError();
            var reflectionTypeLoadException = ex as ReflectionTypeLoadException;
        }

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();
            ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());

            this.InitializeServiceLocator();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///   Instantiate the container and add all Controllers that derive from 
        ///   WindsorController to the container.  Also associate the Controller 
        ///   with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();

            container.Install(FromAssembly.This());

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorHttpControllerActivator(container));
            //ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            var windsorServiceLocator = new WindsorServiceLocator(container);
            DomainEvents.ServiceLocator = windsorServiceLocator;
            ServiceLocator.SetLocatorProvider(() => windsorServiceLocator);

            ComponentRegistrar.AddComponentsTo(container);
        }

        /// <summary>
        ///   If you need to communicate to multiple databases, you'd add a line to this method to
        ///   initialize the other database as well.
        /// </summary>
        private void InitializeNHibernateSession()
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();//查看HQL生成的SQL
            var cfg = NHibernateSession.Init(
                 this.webSessionStorage,
                 new[] { this.Server.MapPath("~/bin/__NAME__.Infrastructure.dll") },
                 new AutoPersistenceModelGenerator().Generate(),
                 this.Server.MapPath("~/NHibernate.config"));
            //new SchemaExport(cfg).Execute(false, true, false);
        }
    }
}

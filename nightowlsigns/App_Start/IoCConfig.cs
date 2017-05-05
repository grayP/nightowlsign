using Autofac;
using Autofac.Integration.Mvc;
using nightowlsign.data.Models.Stores;
using System.Linq;
using System.Web.Mvc;
using nightowlsign.data.Models.Image;
using nightowlsign.data.Models.Schedule;
using nightowlsign.data.Models.Signs;

namespace nightowlsign
{
    public static class IoCConfig
    {
        public static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            #region Register all controllers for the assembly
            // Note that ASP.NET MVC requests controllers by their concrete types, 
            // so registering them As<IController>() is incorrect. 
            // Also, if you register controllers manually and choose to specify 
            // lifetimes, you must register them as InstancePerDependency() or 
            // InstancePerHttpRequest() - ASP.NET MVC will throw an exception if 
            // you try to reuse a controller instance for multiple requests. 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            #endregion

            #region Setup a common pattern
            // placed here before RegisterControllers as last one wins
            builder.RegisterAssemblyTypes()
                   .Where(t => t.Name.EndsWith("Manager"))
                   .AsImplementedInterfaces()
                   .InstancePerRequest();
            builder.RegisterAssemblyTypes()
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerRequest();
            #endregion


            #region Model binder providers - excluded - not sure if need
            //builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            //builder.RegisterModelBinderProvider();
            #endregion

            #region Inject HTTP Abstractions
            /*
             The MVC Integration includes an Autofac module that will add HTTP request 
             lifetime scoped registrations for the HTTP abstraction classes. The 
             following abstract classes are included: 
            -- HttpContextBase 
            -- HttpRequestBase 
            -- HttpResponseBase 
            -- HttpServerUtilityBase 
            -- HttpSessionStateBase 
            -- HttpApplicationStateBase 
            -- HttpBrowserCapabilitiesBase 
            -- HttpCachePolicyBase 
            -- VirtualPathProvider 

            To use these abstractions add the AutofacWebTypesModule to the container 
            using the standard RegisterModule method. 
            */
           // builder.RegisterType<nightowlsign_Entities>().As<IDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<StoreViewModel>().As<IStoreViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleViewModel>().As<IScheduleViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<ImageViewModel>().As<IImageViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SignManager>().As<ISignManager>().InstancePerLifetimeScope();
            builder.RegisterModule<AutofacWebTypesModule>();


            #endregion

            //builder.RegisterModule(new DataModule("nightowlsign_Entities"));

            #region Set the MVC dependency resolver to use Autofac
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion

            return container;
        }

    }
}
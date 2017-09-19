using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using nightowlsign.data.Models;
using nightowlsign.Models;

namespace nightowlsign
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            IoCConfig.RegisterDependencies();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Add("__SessionSignSize", new SignSize());

 
        }

        private void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var httpException = ex as HttpException ?? ex.InnerException as HttpException;

            if (httpException?.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
            {
                //handle the error
                Response.Write("The file(s) you are trying to upload are too big.");
            }
        }

    }
}

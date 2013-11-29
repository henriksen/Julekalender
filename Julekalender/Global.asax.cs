using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace Julekalender
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<countDownHub>();
                    hubContext.Clients.All.upDateCountdown(
                        (new DateTime(2013, 12, 2, 12, 0, 0) -
                        DateTime.Now).ToString()
                        );
                    Thread.Sleep(1000);

                }
            });
        }
    }

    public class countDownHub : Hub
    {
    }
}

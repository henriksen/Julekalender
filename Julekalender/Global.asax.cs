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
                    var timeUntilOpening = new DateTime(2013, 11, 29, 21, 11, 15) -
                                           DateTime.Now;
                    var msg = timeUntilOpening.Days > 0 ? timeUntilOpening.Days + 
                        (timeUntilOpening.Days == 1 ? " dag, " : " dager, ") : "";
                    msg += timeUntilOpening.Hours > 0 ? timeUntilOpening.Hours + 
                        (timeUntilOpening.Hours == 1 ? " time, " : " timer, ") : "";
                    msg += timeUntilOpening.Minutes > 0 ? timeUntilOpening.Minutes + 
                        (timeUntilOpening.Minutes == 1 ? " minutt, " : " minutter, ") : "";
                    msg += timeUntilOpening.Seconds +
                        (timeUntilOpening.Seconds == 1 ? " sekund " : " sekunder ");

                    msg += " til pakkeåpning";

                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<CountDownHub>();
                    hubContext.Clients.All.upDateCountdown(msg);
                    if (timeUntilOpening.TotalSeconds > 0)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        hubContext.Clients.All.prepareDraw();
                        Thread.Sleep(6000);

                        var names = new string[] {"Per", "Pål", "Espen", "Kari", "Mari"};
                        int length = names.Length;
                        Random rnd = new Random();
                        var target = rnd.Next(100, 200);
                        var winner = "";
                        for (int i = 0; i < target ; i++)
                        {
                            var index = i%length;
                            var delay = 100 + 5*i;
                            winner = names[index];
                            hubContext.Clients.All.showName(winner, delay);
                            Thread.Sleep(delay);
                        }

                        hubContext.Clients.All.letItSnow(winner);

                        Thread.Sleep(10000);
                    }



                }
            });
        }
    }

    public class CountDownHub : Hub
    {
    }
}

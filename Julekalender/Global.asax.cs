using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Julekalender.App_Start;
using Julekalender.Models;
using Microsoft.Ajax.Utilities;
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
            UserConfig.CreateDefaultUsers();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<CountDownHub>();

                var db = new ApplicationDbContext();
                System.Diagnostics.Trace.TraceInformation(db.Database.Connection.ConnectionString);

                while (true)
                {
                    var drawings = db.Drawings.ToList();
                    System.Diagnostics.Trace.TraceInformation("There are {0} drawings", drawings.Count );

                    Drawing nextDrawing;

                    do
                    {
                        nextDrawing = (from d in db.Drawings
                            where d.Time > DateTime.Now
                            orderby d.Time
                            select d).FirstOrDefault();
                        if (nextDrawing == null)
                        {
                            System.Diagnostics.Trace.TraceError("Found no relevant drawings");
                            hubContext.Clients.All.upDateCountdown("Ingen trekninger planlagt.");
                            Thread.Sleep(5000);
                        }
                    } while (nextDrawing == null);

                    var timeUntilOpening = nextDrawing.Time - DateTime.Now;
                    var msg = timeUntilOpening.Days > 0 ? timeUntilOpening.Days + 
                        (timeUntilOpening.Days == 1 ? " dag, " : " dager, ") : "";
                    msg += timeUntilOpening.Hours > 0 ? timeUntilOpening.Hours + 
                        (timeUntilOpening.Hours == 1 ? " time, " : " timer, ") : "";
                    msg += timeUntilOpening.Minutes > 0 ? timeUntilOpening.Minutes + 
                        (timeUntilOpening.Minutes == 1 ? " minutt, " : " minutter, ") : "";
                    msg += timeUntilOpening.Seconds +
                        (timeUntilOpening.Seconds == 1 ? " sekund " : " sekunder ");

                    msg += " til pakkeåpning";
                    System.Diagnostics.Trace.TraceInformation(msg);

                    hubContext.Clients.All.upDateCountdown(msg);
                    if (timeUntilOpening.TotalSeconds > 0)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        hubContext.Clients.All.prepareDraw();
                        Thread.Sleep(6000);

                        var participants = db.Participants.ToList();
                        int length = participants.Count();
                        Random rnd = new Random();
                        var target = rnd.Next(100, 200);
                        Participant winner = null;
                        for (var i = 0; i < target ; i++)
                        {
                            var index = i%length;
                            var delay = 100 + 5*i;
                            winner = participants[index];
                            hubContext.Clients.All.showName(winner.Name, delay);
                            Thread.Sleep(delay);
                        }
                        nextDrawing.Winner = winner;
                        db.SaveChanges();
                        hubContext.Clients.All.letItSnow(winner.Name);

                        Thread.Sleep(10000);
                        hubContext.Clients.All.removeSnow();

                    }



                }
            });
        }
    }

    public class CountDownHub : Hub
    {
    }
}

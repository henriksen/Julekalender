using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Julekalender.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Julekalender.App_Start
{
    public class UserConfig
    {
        public static void CreateDefaultUsers()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var admin = userManager.FindByName("admin");
            if (admin == null)
            {

                var user = new ApplicationUser() { UserName = "admin" };
                userManager.Create(user, "TrylleHattKanin99");
            }

        }
    }
}
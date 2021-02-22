using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using poopi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace poopi.Helpers
{
    public class SeederDatabase
    {
        public static void SeedData()
        {
            var context = new ApplicationDbContext();
            SeedUsers(context);
        }

        private static void SeedUsers(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.Roles.Any())
            {
                IdentityRole roleAdmin = new IdentityRole() { Name = "Admin" };
                IdentityRole roleStudent = new IdentityRole() { Name = "Student" };
                IdentityRole roleUser = new IdentityRole() { Name = "User" };
                roleManager.Create(roleAdmin);
                roleManager.Create(roleStudent);
                roleManager.Create(roleUser);
            }
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            if (!userManager.Users.Any())
            {
                ApplicationUser user = new ApplicationUser() { UserName = "admin@gmail.com", Email = "admin@gmail.com" };
                userManager.Create(user, "Qwerty1-");
                userManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
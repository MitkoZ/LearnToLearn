namespace DataAccess.Migrations
{
    using DataAccess.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess.LearnToLearnContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        private UserManager<User> userManager;
        protected override void Seed(DataAccess.LearnToLearnContext context)
        {
            UserStore<User> userStore = new UserStore<User>(context)
            {
                AutoSaveChanges = false
            };
            this.userManager = new UserManager<User>(userStore);
            //  This method will be called after migrating to the latest version.
            User user = new User { Name = "admin", Email = "admin@abv.bg", IsTeacher = true, UserName = "admin" };
            User user2 = new User { Name = "pesho", Email = "pesho@abv.bg", IsTeacher = false, UserName = "pesho" };
            User user3 = new User { Name = "ivan", Email = "ivan@abv.bg", IsTeacher = false, UserName = "ivan" };
            if (!user.Courses.Any(x => x.Name == "C# basics"))
            {
                user.Courses.Add(new Course { Name = "C# basics", Description = "C# programming basics", Capacity = 20, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsVisible = true });
            }
            if (!user.Courses.Any(x => x.Name == "ASP.NET MVC basics"))
            {
                user.Courses.Add(new Course { Name = "ASP.NET MVC basics", Description = "Making dynamic websites with ASP.NET MVC", Capacity = 15, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsVisible = true });
            }
            if (!user.Courses.Any(x => x.Name == "Web API"))
            {
                user.Courses.Add(new Course { Name = "Web API", Description = "ASP.NET Web API 2", Capacity = 34, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsVisible = true });
            }

            if (!context.Users.Any(x => x.Name == user.Name))
            {
                IdentityResult identityResult = userManager.Create(user, "adminpass");

                if (!identityResult.Succeeded)
                {
                    return;
                }
            }
            if (!context.Users.Any(x => x.Name == user.Name))
            {
                IdentityResult identityResult2 = userManager.Create(user2, "123456");
                if (!identityResult2.Succeeded)
                {
                    return;
                }
            }
            if (!context.Users.Any(x => x.Name == user.Name))
            {
                IdentityResult identityResult3 = userManager.Create(user3, "123456");
                if (!identityResult3.Succeeded)
                {
                    return;
                }
            }

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string roleTeacher = "Teacher";
            if (!roleManager.RoleExists(roleTeacher))
            {
                IdentityResult identityResult4 = roleManager.Create(new IdentityRole(roleTeacher));
                if (!identityResult4.Succeeded)
                {
                    return;
                }
            }

            string roleNormalUser = "NormalUser";
            if (!roleManager.RoleExists(roleNormalUser))
            {
                IdentityResult identityResult5 = roleManager.Create(new IdentityRole(roleNormalUser));
                if (!identityResult5.Succeeded)
                {
                    return;
                }
            }

            context.SaveChanges();
            userManager.AddToRole(user.Id, "Teacher");
            userManager.AddToRole(user2.Id, "NormalUser");
            userManager.AddToRole(user3.Id, "NormalUser");
            context.SaveChanges();
        }
    }
}

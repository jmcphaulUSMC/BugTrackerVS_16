namespace BugTrackerVS_16.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTrackerVS_16.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTrackerVS_16.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }


            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }


            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "jmcphaul@aggies.ncat.edu"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jmcphaul@aggies.ncat.edu",
                    Email = "jmcphaul@aggies.ncat.edu",
                    FristName = "Johnthan",
                    LastName = "Mcphaul",
                    DisplayName = "jmcphaul"
                }, "John2905");
            }

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FristName = "Coder",
                    LastName = "Foundry",
                    DisplayName = "CoderFoundry"
                }, "Password-1");
            }

            if (!context.Users.Any(u => u.Email == "jmcphaul@aggies.ncat.edu"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jmcphaul@aggies.ncat.edu",
                    Email = "jmcphaul@aggies.ncat.edu",
                    FristName = "Johnthan",
                    LastName = "Mcphaul",
                    DisplayName = "jmcphaul"
                }, "John2905");
            }

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FristName = "Coder",
                    LastName = "Foundry",
                    DisplayName = "CoderFoundry"
                }, "Password-1");
            }


            var userId = userManager.FindByEmail("jmcphaul@aggies.ncat.edu").Id;
            userManager.AddToRole(userId, "Admin");

            var userId1 = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId1, "Submitter");

            var userId2 = userManager.FindByEmail("jmcphaul@aggies.ncat.edu").Id;
            userManager.AddToRole(userId2, "ProjectManager");

            var userId3 = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId3, "Developer");
        }
    }
}

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
                    FirstName = "Johnthan",
                    LastName = "Mcphaul",
                    DisplayName = "jmcphaul"
                }, "John2905");
            }

            if (!context.Users.Any(u => u.Email == "Developer@gustr.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer@gustr.com",
                    Email = "Developer@gustr.com",
                    FirstName = "Developer",
                    LastName = "Developer",
                    DisplayName = "Developer"
                }, "developer");
            }

            if (!context.Users.Any(u => u.Email == "submitter@gustr.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "submitter@gustr.com",
                    Email = "submitter@gustr.com",
                    FirstName = "submitter",
                    LastName = "submitter",
                    DisplayName = "submitter"
                }, "submitter");
            }

            if (!context.Users.Any(u => u.Email == "projectmanager@gustr.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "projectmanager@gustr.com",
                    Email = "projectmanager@gustr.com",
                    FirstName = "project",
                    LastName = "manager",
                    DisplayName = "PM"
                }, "projectmanager");
            }


            var userId = userManager.FindByEmail("jmcphaul@aggies.ncat.edu").Id;
            userManager.AddToRole(userId, "Admin");

            var userId1 = userManager.FindByEmail("submitter@gustr.com").Id;
            userManager.AddToRole(userId1, "Submitter");

            var userId2 = userManager.FindByEmail("projectmanager@gustr.com").Id;
            userManager.AddToRole(userId2, "ProjectManager");

            var userId3 = userManager.FindByEmail("Developer@gustr.com").Id;
            userManager.AddToRole(userId3, "Developer");

            if (!context.TicketPriorities.Any(u => u.Name == "High"))
            {
                context.TicketPriorities.Add(new TicketPriorities
                {
                    Name = "High"
                });
            }
            if (!context.TicketPriorities.Any(u => u.Name == "Medium"))
            {
                context.TicketPriorities.Add(new TicketPriorities
                {
                    Name = "Medium"
                });
            }
            if (!context.TicketPriorities.Any(u => u.Name == "Low"))
            {
                context.TicketPriorities.Add(new TicketPriorities
                {
                    Name = "Low"
                });
            }
            if (!context.TicketPriorities.Any(u => u.Name == "Urgent"))
            {
                context.TicketPriorities.Add(new TicketPriorities
                {
                    Name = "Urgent"
                });
            }

            if (!context.TicketTypes.Any(u => u.Name == "Production Fix"))
            { context.TicketTypes.Add(new TicketTypes { Name = "Production Fix" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Project Task"))
            { context.TicketTypes.Add(new TicketTypes { Name = "Project Task" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Software Update"))
            { context.TicketTypes.Add(new TicketTypes { Name = "Software Update" }); }


            if (!context.TicketStatuses.Any(u => u.Name == "Complete"))
            { context.TicketStatuses.Add(new TicketStatuses { Name = "Complete" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "In Development"))
            { context.TicketStatuses.Add(new TicketStatuses { Name = "In Development" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "New"))
            { context.TicketStatuses.Add(new TicketStatuses { Name = "New" }); }

        }
    }
}

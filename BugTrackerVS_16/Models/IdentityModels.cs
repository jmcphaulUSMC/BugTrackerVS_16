﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BugTrackerVS_16.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public ApplicationUser()
        {
            Projects = new HashSet<Projects>();
            TicketHistories = new HashSet<TicketHistories>();
            TicketNotifications = new HashSet<TicketNotifications>();
            TicketAttachments = new HashSet<TicketAttachments>();
            TicketComments = new HashSet<TicketComments>();
            Tickets = new HashSet<Tickets>();

        }
        public virtual ICollection<TicketComments> TicketComments { get; set; }
        public virtual ICollection<TicketAttachments> TicketAttachments { get; set; }
        public virtual ICollection<TicketHistories> TicketHistories { get; set; }
        public virtual ICollection<TicketNotifications> TicketNotifications { get; set; }
        public virtual ICollection<Projects> Projects { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Tickets> Ticket { get; set; }
        public DbSet<TicketComments> TicketComments { get; set; }
        public DbSet<TicketAttachments> TicketAttachments { get; set; }
        public DbSet<TicketHistories> TicketHistories { get; set; }
        public DbSet<TicketNotifications> TicketNotifications { get; set; }
        public DbSet<TicketStatuses> TicketStatus { get; set; }
        public DbSet<TicketPriorities> TicketPriority { get; set; }
        public DbSet<TicketTypes> TicketType { get; set; }
        public DbSet<Projects> Projects { get; set; }
    }
}
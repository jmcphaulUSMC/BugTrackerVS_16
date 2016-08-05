using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class Projects
    {
        public Projects()
        {
            Ticket = new HashSet<Tickets>();
            User = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
    

        public virtual ICollection<ApplicationUser> User { get; set; }
        public virtual ICollection<Tickets> Ticket { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class Projects
    {
        public Projects()
        {
            Tickets = new HashSet<Tickets>();
            Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public System.DateTimeOffset Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTimeOffset? Updated { get; set; }

        public string ManagerId { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
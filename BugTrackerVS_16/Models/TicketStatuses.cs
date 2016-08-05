using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class TicketStatuses
    {
        public TicketStatuses()
        {
            Ticket = new HashSet<Tickets>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
   
        public virtual ICollection<Tickets> Ticket { get; set;}

    }
}
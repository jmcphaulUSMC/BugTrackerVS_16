using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class TicketNotifications
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        
        //Creates the one to one relationship with UserId 
        public virtual ApplicationUser User { get; set; }

        //Creates the one to one relationship with TicketID
        public virtual Tickets Ticket { get; set; }
    }
}
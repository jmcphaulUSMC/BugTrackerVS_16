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
        public string EnteredById { get; set; }
        public string Message { get; set; }
        public string MessageForId { get; set; }
        public bool HasBeenRead { get; set; }

        //Creates the one to one relationship with UserId 
        public virtual ApplicationUser EnteredBy { get; set; }
        public virtual ApplicationUser MessageFor { get; set; }

        //Creates the one to one relationship with TicketID
        public virtual Tickets Ticket { get; set; }
    }
}
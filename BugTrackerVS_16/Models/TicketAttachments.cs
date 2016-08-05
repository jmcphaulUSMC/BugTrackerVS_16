using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class TicketAttachments
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }
        public string FileURL { get; set; }

        //Creates that one to one relatioship userId
        public virtual ApplicationUser User { get; set; }

        //Creates that one to one relatioship with the TicKet
        public virtual Tickets Ticket { get; set; }
    }
}
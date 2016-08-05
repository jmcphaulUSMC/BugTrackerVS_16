using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTrackerVS_16.Models
{
    public class TicketComments
    {
        public int Id { get; set; }
        [AllowHtml]
        [Required]
        public string Comment { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        //C
        public virtual ApplicationUser User { get; set; }

        public virtual Tickets Ticket { get; set; }
    }
}
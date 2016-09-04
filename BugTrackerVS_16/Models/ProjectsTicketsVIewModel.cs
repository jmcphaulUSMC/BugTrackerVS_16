using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class ProjectsTicketsViewModel
    {
        public IList<Projects> project { get; set; }
        public IList<Tickets> Ticket { get; set; }
    }
}
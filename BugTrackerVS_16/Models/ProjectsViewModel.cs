using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class ProjectsViewModel
    {
        public virtual Projects projects { get; set; }
        public IList<ApplicationUser> Users { get; set; }
    }
}
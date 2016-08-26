using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models
{
    public class ProjectRolesViewModel
    {
        public ApplicationUser Users { get; set; }
        public List<Projects> Projects { get; set; }
    }
}
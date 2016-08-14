using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BugTrackerVS_16.Models
{
    public class RoleViewModel
    {
        public ApplicationUser Users { get; set; }
        public List<string> Roles { get; set; }

    } 
}
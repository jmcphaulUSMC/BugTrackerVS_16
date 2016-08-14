using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace BugTrackerVS_16.Models
{
    public class AssignUserToRoleViewModel
    {


        public string Id { get; set; }
        public string Name { get; set; }

        public MultiSelectList Roles { get; set; }

        public string[] SelectedRoles { get; set; }
      


    }
}
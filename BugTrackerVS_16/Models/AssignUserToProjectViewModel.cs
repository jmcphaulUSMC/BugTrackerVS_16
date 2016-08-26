using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace BugTrackerVS_16.Models
{
    public class AssignUserToProjectViewModel
    {


        public int Id { get; set; }
        public string Name { get; set; }

        public MultiSelectList AddUsers { get; set; }

        public string[] SelectedUsers { get; set; }
      


    }
}
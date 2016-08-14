using BugTrackerVS_16.Models;
using BugTrackerVS_16.Models.User_Roles;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.Collections.Generic;

namespace BugTrackerVS_16.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelper helper = new UserRolesHelper();

        //GET: Users/UserRoles
        public ActionResult Roles()
        {

            //Right here I Created an instance of my userViewModel which is a class in my Model called "UserViewModel"
            List<RoleViewModel> userList = new List<RoleViewModel>();
            foreach (var user in db.Users.ToList())//Right here using a for each loop to cycle through user in the database and put them in a list 
            {
                var model = new RoleViewModel();
                model.Users = user;
                model.Roles = helper.ListUserRoles(user.Id).ToList();
                userList.Add(model);
                //Right here I'm calling to the database and telling it to give every role that exists in the database 
            };

            //Right here I want the view to retun model to the Roles view 
            return View(userList);

        }


        //GET: Assign/UserToRoles
        public ActionResult AssignUserToRole(string id) //<==(roleName has to come along with data, to identify the role I just clicked)

        {
            var userdb = db.Users.Find(id); //<==(go to the database find users and then put in a list )


            var model = new AssignUserToRoleViewModel();
            model.Id = id;
            model.Name = userdb.FirstName;
            model.SelectedRoles = helper.ListUserRoles(id).ToArray(); //<==(give me a list of all users not in a role and then push them into an array)
            model.Roles = new MultiSelectList(db.Roles, "Name", "Name", model.SelectedRoles);

            return View(model);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        //POST: Assign/UsersToRoles EDIT
        public ActionResult AssignUserToRole(AssignUserToRoleViewModel model)

        {
            var userdb = db.Users.Find(model.Id); 

            foreach (var roles in db.Roles.Select(r=>r.Name).ToList())
            {
                helper.RemoveUserFromRole(userdb.Id, roles);
            }
               foreach (var assinRoles in model.SelectedRoles)
            {
                helper.AddUserToRole(userdb.Id, assinRoles);
            }

            return RedirectToAction("Roles", "Admin");
        }


    }
}
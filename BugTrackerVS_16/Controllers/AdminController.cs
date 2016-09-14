using BugTrackerVS_16.Models;
using BugTrackerVS_16.Models.User_Roles;
using System.Linq;
using System.Web.Mvc;
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
            if (User.IsInRole("Admin"))
            {
                List<RoleViewModel> userList = new List<RoleViewModel>();//<=== Right here I Created an instance of my userViewModel which is a class in my Model called "UserViewModel" and I'm putting into a list 
                foreach (var user in db.Users.ToList())//Right here using a for each loop to cycle through Users in the database and put them in a list 
                {
                    var model = new RoleViewModel(); //<== Right here I made another instance of my "user view model" to access it's properties 
                    model.Users = user; //<==== Right here I'm setting "Users" from my model equal to the variable in the foreach loop
                    model.Roles = helper.ListUserRoles(user.Id).ToList(); //<=== Right here I'm setting "Roles" equal to my helper class and the I'm using ListUserRoles method and the paramater is grabbing user "Id" and then put them in a list
                    userList.Add(model);//<=== Right here I'm grabbing my UserList and I'm adding my second model to it 
                }

                //Right here I want the view to retun model to the Roles view 
                return View(userList);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
         

        //GET: Assign/UserToRoles
        [Authorize(Roles = "Admin")]
        public ActionResult AssignUserToRole(string id) //<==(The "id" has to come along with data, to identify the role I just clicked on to assign user to Roles)

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
        [ValidateAntiForgeryToken]
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
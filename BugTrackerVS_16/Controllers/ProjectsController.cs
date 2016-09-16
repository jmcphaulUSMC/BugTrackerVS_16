using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BugTrackerVS_16.Models;
using BugTrackerVS_16.Models.User_Roles;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System;

namespace BugTrackerVS_16.Controllers
{
    public class ProjectsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        ProjectsHelper pjHelper = new ProjectsHelper();
        UserRolesHelper help = new UserRolesHelper();

        // GET: Projects
        [Authorize]
        public ActionResult Index()//(pass in projects id
        {

            if (User.IsInRole("Admin"))
            {
                return View(db.Projects.ToList());
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var AssignProject = pjHelper.AssignedUserToProject(userId);

                return View(AssignProject);
            }

        }


        // GET: Projects/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        [Authorize(Roles = "Admin")]
        // GET: Projects/Create
        public ActionResult Create()
        {
            var manager = help.UsersInRole("ProjectManager");

            ViewBag.ManagerId = new SelectList(manager, "Id", "DisplayName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name, ManagerId")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                projects.Created = DateTime.Now;
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        //GET://Project/Assign
        [Authorize]
        public ActionResult AssignUsers(int id)
        {

            var projectdb = db.Projects.Find(id);
            var projectUs = db.Users.Where(u => u.Projects.All(pu => pu.Id != projectdb.Id)).ToList();
            var users = projectdb.Users;

            var model = new AssignUserToProjectViewModel();
            model.Id = id;
            model.AddUsers = new MultiSelectList(db.Users, "Id", "FirstName", model.SelectedUsers);
            //model.SelectedUsers = pjHelper.UsersInProject(projectdb.id).ToArray();


            model.SelectedUsers = users.Select(u => u.Id).ToArray(); //<==(give me a list of all users not in a project and then push them into an array)

            return View(model);
        }


        //POST://Project/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUsers(AssignUserToProjectViewModel model)
        {
            if (model != null)//check that the model isn't null
            {
                var project = db.Projects.Find(model.Id);//get the project that your currently assigning/removing users to/from
                if (project != null)//check that the project isn't null
                {
                    project.Users.Clear();//remove all users from the project
                    ApplicationUser user = new ApplicationUser();//create an instance of the ApplicationUser class
                    foreach (var userId in model.SelectedUsers)//loop through all users that are included in the 'SelectedUsers' collection
                    {
                        user = db.Users.Find(userId);//get the user by their 'Id' property
                        project.Users.Add(user);//add the user to the project, thus assigning the user to the project
                    }
                    db.SaveChanges();//save changes made to the database

                    return RedirectToAction("Index", "Projects");//I am assuming that you'll want to return to a list view of projects but you may want to change this to redirect to the details view for this project or redirect back to the view where you are assigning/removing users to/from this project....whatever you want to do
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//if the model is null or the project is null return a BadRequest error
        }


        [Authorize]
        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                projects.Updated = DateTime.Now;
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        [Authorize]
        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

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

namespace BugTrackerVS_16.Controllers
{
    public class ProjectsController : Controller
    {

       private ApplicationDbContext db = new ApplicationDbContext();
              ProjectsHelper pjHelper = new ProjectsHelper();
                
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
                // var user = db.Users.Find(id);
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
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }


        // GET:  ProjectAssign/View
        [Authorize]
        public ActionResult ProjectView()
        {
            List<ProjectsViewModel> projectList = new List<ProjectsViewModel>();// Put my ProjectsViewModel in a list 
            foreach (var project in db.Projects) //list of all projects in database and put them in a for each loop
            {
                ProjectsViewModel pv = new ProjectsViewModel(); //create another instance of my viewModel to control the data in the for each loop so the model contains

                pv.projects = project;
                pv.Users = pjHelper.UsersNotInProject(project.Id);
                projectList.Add(pv);
            }
            return View(projectList);

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

            foreach (var roles in db.Roles.Select(r => r.Name).ToList())
            {
                    pjHelper.RemoveUserFromProject(roles, model.Id);
            }

            foreach (var assinRoles in model.SelectedUsers)
            {
                pjHelper.AddUserToProject(assinRoles, model.Id);
            }

        return RedirectToAction("Index", "Projects");
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

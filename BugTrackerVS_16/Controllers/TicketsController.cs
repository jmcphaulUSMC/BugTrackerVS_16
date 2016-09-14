using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerVS_16.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using BugTrackerVS_16.Models.User_Roles;
using System.Web.Services.Description;

namespace BugTrackerVS_16.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper phelper = new ProjectsHelper();
        private HistoryHelper helper = new HistoryHelper();
        private UserRolesHelper help = new UserRolesHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();// get the user "Id"
            ViewBag.CurrentUser = User.Identity.GetUserId();// get the user "Id"

            //database look at tickets table and include all Fields in AssignedToUser
            var ticketsTwo = db.Tickets
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType);
            return View(ticketsTwo.ToList());

        }

        // GET: Tickets
        [Authorize]
        public ActionResult _ProjectTotals()
        {
            ViewData["ProjectsTotals"] = db.Projects.Count();
            return PartialView();

        }
        // GET: Tickets
        [Authorize]
        public ActionResult _TicketTotals()
        {
            ViewData["TicketTotals"] = db.Tickets.Count();
            return PartialView();

        }
        // GET: Tickets
        [Authorize]
        public ActionResult _HighTotals()
        {
            ViewData["HighPriorityTotals"] = db.Tickets.Where(t => t.TicketPriority.Name == "Urgent").Count();
            return PartialView();
        }
        // GET: Tickets
        [Authorize]
        public ActionResult _NewTotals()
        {
            ViewData["HighPriorityTotals"] = db.Tickets.Where(t => t.TicketStatus.Name == "New").Count();
            return PartialView();
        }

        // GET: Tickets
        [Authorize]
        public ActionResult _TicketNotifications()
        {
            var userid = User.Identity.GetUserId();

            var notifyMessage = db.TicketNotifications
                        .Where(x => x.MessageForId ==userid)
                        .Where(x => x.HasBeenRead == false).ToList();

            return PartialView(notifyMessage.ToList());
        }

        // GET: Tickets
        [Authorize]
        public ActionResult _DataProfile()
        {
            ViewData["Users"] = User.Identity.GetUserName();
            return PartialView();
        }

        // GET: Project Manager Tickets
        [Authorize]
        public ActionResult IndexAssign()
        {
            //var userId = User.Identity.GetUserId();// get the user "Id"
            ViewBag.CurrentUser = User.Identity.GetUserId();// get the user "Id"

            var userId = User.Identity.GetUserId(); //gets the user "Id" 
            var tickets = new List<Tickets>();


            //var tickets = db.Tickets;//go to the database and grab alll the tickets 
            var tickets2 = db.Tickets.Where(t => t.Project.ManagerId == userId);// 
            var tickets3 = db.Tickets.Where(t => t.AssignedToUserId == userId);
            var tickets4 = db.Tickets.Where(t => t.OwnerUserId == userId);

            if (User.IsInRole("Admin"))
            {
                tickets = db.Tickets.ToList();// return all the tickets and put them in a list
            }

            if (User.IsInRole("ProjectManager"))
            {
                tickets = db.Tickets.Where(t => t.Project.ManagerId == userId).ToList();// return all the tickets2 and put them in a list
            }

            if (User.IsInRole("Developer"))
            {
                tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();// return all the tickets3 and put them in a list
            }

            if (User.IsInRole("Submitter"))
            {
                tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();// return all the tickets4 and put them in a list
            }
            
            return View(tickets);
        }

        // GET: Project Manager Tickets
        [Authorize]
        public ActionResult IndexProjects()
        {
            {
                var manager = help.UsersInRole("ProjectManager");
                //var userId = User.Identity.GetUserId();// get the user "Id"
                ViewBag.CurrentUser = User.Identity.GetUserId();// get the user "Id"
                ViewBag.ManagerId = new SelectList(manager, "Id", "DisplayName");

                var userId = User.Identity.GetUserId();// get the user "Id"
                var currentUser = db.Users.Find(userId);//go to the database and find the user by their "id" 
                var projectTickets = currentUser.Projects.SelectMany(t => t.Tickets);
                //with the currentUser grab the project assoicated with that user and selectmany tickets for all the projects
                ViewBag.Project = currentUser.Projects.SelectMany(t => t.Tickets);
                return View(projectTickets);
            }
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            

            ////Grabs the User Identity the Long ugly looking number/letters "id"
            string userId = User.Identity.GetUserId();

            ViewBag.AllowedToComment = User.IsInRole("Admin") || (User.IsInRole("Project Manager") && db.Users.FirstOrDefault(u => u.Id == userId).Projects.Any(p => p.Tickets.Any(t => t.Id == id)))
  //check if that user is role and if so go to the database and go to the Users Table and return the first record where the "Id" is EQUAL to userId. If there is no record with and "Id"  that is EQUAL to userId return null. Then look at the projects assoicated with that user that was just returned and look at all the tickets for all those projects if any of those tickets "Id" is EQUAL to the id parameter of the method reutrn true IF NOT EQUAL return FALSE.
  || (User.IsInRole("Developer") && db.Tickets.FirstOrDefault(t => t.Id == id).AssignedToUserId == userId) ||
           //check if that user is in role, go to the database and go to the Tickets table and grab the first record where the ticket "Id" is equal to the parameter "id". If there is not record return a DEFAULT VALUE which is null. For ever ticket that is return look at that the "AssignedToUseId" property and if it is EQUAL to the userId return if it's true  
           (User.IsInRole("Submitter") && db.Tickets.FirstOrDefault(t => t.Id == id).OwnerUserId == userId);
            RedirectToAction("Details");
            return  View(tickets);

           
            //check if the user is role, go to the database and the go to the tickets table and grab the first record in the able with an "Id" that EQUAL id that is being passed in and that record doesnt have an "Id" return null and then when those tickets come back look at the owenerUserId property of that ticket and if it's EQUAL to the userID or true return it if its false dont return it.


        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Created = DateTime.Now;
                //Rememeber: this allows you to make the user that creates the ticket the owner of the ticket
                tickets.OwnerUserId = User.Identity.GetUserId();
                if (tickets.TicketPriorityId == 0)
                {
                    tickets.TicketPriorityId = db.TicketPriorities.FirstOrDefault(x => x.Name == "Low").Id;
                }
                if (tickets.TicketStatusId == 0)
                {
                    tickets.TicketStatusId = db.TicketStatuses.FirstOrDefault(x => x.Name == "New").Id;
                }
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var developerRoleId = db.Roles.First(x => x.Name == "Developer").Id;

            ViewBag.AssignedToUserId = new SelectList(db.Users.Where(x => x.Roles.Any(y => y.RoleId == developerRoleId)), "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

               ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
                ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId, ManagerId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets model)
        {
            if (ModelState.IsValid)
            {
                var originalTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == model.Id);

                helper.CheckForHistory(originalTicket, model);
                helper.CheckForNotification(originalTicket, model);

                model.Updated = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { id = model.Id});
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", model.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", model.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", model.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", model.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", model.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", model.TicketTypeId);
            return View(model);
        }


        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
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

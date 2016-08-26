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

namespace BugTrackerVS_16.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper phelper = new ProjectsHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            //ViewBag.CurrentUser = User.Identity.;

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

        // GET: Project Manager Tickets
        [Authorize]
        public ActionResult IndexTwo()
        {
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager") || User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                var userId = User.Identity.GetUserId(); //gets the user "Id" 

                var tickets = db.Tickets.Include(t => t.AssignedToUser)// Include all tickets AssignedToUser
                     .Where(t => t.AssignedToUser.Id == userId)//Where AssignedToUser "id" equals userId 
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType);
                return View(tickets.ToList());//return those tickets and put them in a list 

            }
            else
            {
                RedirectToAction("Index", "Tickets");
            }

            return View();
        
        }
        // GET: Project Manager Tickets
        [Authorize]
        public ActionResult IndexProject()
        {
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager") || User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                var userId = User.Identity.GetUserId(); //gets the user "Id" 

                // all tickets that are in the user's projects
                var tickets = db.Tickets
                    .Where(t => t.Project.Users.Any(u => u.Id == userId))
                    .Include(t => t.AssignedToUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .OrderByDescending(t => t.TicketPriorityId);
                return View(tickets.ToList());
            }
            else
            {
                RedirectToAction("Index", "Tickets");
            }
            return View();
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

            //Grabs the User Identity the Long ugly looking number/letters "id"
            string userId = User.Identity.GetUserId();


            ViewBag.AllowedToComment = User.IsInRole("Admin") || (User.IsInRole("Project Manager") && db.Users.FirstOrDefault(u => u.Id == userId).Projects.Any(p => p.Tickets.Any(t => t.Id == id)))
                //check if that user is role and if so go to the database and go to the Users Table and return the first record where the "Id" is EQUAL to userId. If there is no record with and "Id"  that is EQUAL to userId return null. Then look at the projects assoicated with that user that was just returned and look at all the tickets for all those projects if any of those tickets "Id" is EQUAL to the id parameter of the method reutrn true IF NOT EQUAL return FALSE.
  || (User.IsInRole("Developer") && db.Tickets.FirstOrDefault(t => t.Id == id).AssignedToUserId == userId) || 
          //check if that user is in role, go to the database and go to the Tickets table and grab the first record where the ticket "Id" is equal to the parameter "id". If there is not record return a DEFAULT VALUE which is null. For ever ticket that is return look at that the "AssignedToUseId" property and if it is EQUAL to the userId return if it's true  
           (User.IsInRole("Submitter") && db.Tickets.FirstOrDefault(t => t.Id == id).OwnerUserId == userId);
            return View(tickets);

     
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Created = DateTime.Now;
                //Rememeber: this allows you to make the user that creates the ticket the owner of the ticket
                tickets.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
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
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
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

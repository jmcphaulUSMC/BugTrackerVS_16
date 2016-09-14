using BugTrackerVS_16.Models;
using BugTrackerVS_16.Models.User_Roles;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;



namespace BugTrackerVS_16.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Demo()
        {

            return View();
        }

        public ActionResult Profile()
        {

            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult SelectUser()
        {

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            RedirectToAction("Index", "Projects");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Contact([Bind(Include = "Id,FullName,Email,Message,Phone")] Content contact)
        //{
        //   contact.Created = DateTime.Now;
        //   var newContact = contact.FullName;

        //   var svc = new EmailService();
        //   var msg = new IdentityMessage();
        //   msg.Destination = "jmcphaul@aggies.ncat.edu";
        //   msg.Subject = "Contact From Portfolio Site";
        //   msg.Body = "\r\n You have recieved a request to contact from " + newContact + "(" + contact.Email + ")" + "\r\n"
        //                + "\r\n With the following message: \r\n\t"
        //                + contact.Message;


        //    await svc.SendAsync(msg);

        //    return View(contact);
        //}
    }
}
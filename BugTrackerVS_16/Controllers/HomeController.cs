using BugTrackerVS_16.Models.User_Roles;
using System.Web.Mvc;

namespace BugTrackerVS_16.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RoleAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAdmin(string userId, string rId)
        {
            UserRolesHelper helper = new UserRolesHelper();

            helper.AddUserToRole(userId, rId);
            helper.AddUserToRole(userId, rId);
            helper.RemoveUserFromRole(userId, rId);
            
            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
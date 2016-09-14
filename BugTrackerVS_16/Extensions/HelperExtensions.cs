using BugTrackerVS_16.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Extensions
{
    public static class HelperExtensions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static int GetMyNotifications(this ApplicationUser nTic)
        {
            int nNum = 0;
            string userId = HttpContext.Current.User.Identity.GetUserId();
            //Need to find a way to get the ticketnotification for a that user that it loged in so the see there notification
            nNum = db.TicketNotifications.Where(x => x.MessageForId == userId).Count();
            return nNum;
        }
    }
}
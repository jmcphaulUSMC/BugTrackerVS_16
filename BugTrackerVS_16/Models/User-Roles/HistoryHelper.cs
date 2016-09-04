using BugTrackerVS_16.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerVS_16.Models.User_Roles
{
    public class HistoryHelper
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public Tickets model = new Tickets();
        public ApplicationUser appUser = new ApplicationUser();

        public void CheckForNotification(Tickets originalTicket, Tickets model)
        {

            var subject = originalTicket.Title + "-";

            //Check Description
            if (originalTicket.Description != model.Description)
            {
                var ownerMailer = new EmailService();
                var ownerNotification = new IdentityMessage
                {
                    Destination = originalTicket.OwnerUser.Email,
                    Subject = subject + originalTicket.Description + " has changed.",
                    Body = "The description for this ticket has changed. The new description is " + model.Description + "."
                };
                ownerMailer.Send(ownerNotification);

                if (originalTicket.AssignedToUserId != null)
                {
                    var developerMailer = new EmailService();
                    var developerNotification = new IdentityMessage
                    {
                        Destination = originalTicket.AssignedToUser.Email,
                        Subject = subject + originalTicket.Description + " has changed.",
                        Body = "The description for this ticket has changed. The new description is " + model.Description + "."
                    };

                    developerMailer.Send(developerNotification);
                }
            }


            //Check Status
            if (originalTicket.TicketStatusId != model.TicketStatusId)
            {
                var ownerMailer = new EmailService();
                var ownerNotification = new IdentityMessage
                {
                    Destination = originalTicket.OwnerUser.Email,
                    Subject = subject + originalTicket.TicketStatus.Name + " has changed.",
                    Body = "The status for this ticket has changed. The new status is " + db.TicketStatuses.FirstOrDefault(x => x.Id == model.TicketStatusId).Name + "."
                };
                ownerMailer.Send(ownerNotification);

                if (originalTicket.AssignedToUserId != null)
                {
                    var developerMailer = new EmailService();
                    var developerNotification = new IdentityMessage
                    {
                        Destination = originalTicket.AssignedToUser.Email,
                        Subject = subject + originalTicket.TicketStatus.Name + " has changed.",

                        Body = "The status for this ticket has changed.  The new status is " + db.TicketStatuses.FirstOrDefault(x => x.Id == model.TicketStatusId).Name + "."
                    };

                    developerMailer.Send(developerNotification);
                }
            }

            //Check Assigned User
            if (originalTicket.AssignedToUserId != model.AssignedToUserId)
            {
                if (model.AssignedToUserId != null)
                {
                    var developer = db.Users.Find(model.AssignedToUserId);
                    var ownerMailer = new EmailService();
                    var ownerNotification = new IdentityMessage
                    {
                        Destination = originalTicket.OwnerUser.Email,
                        Subject = "The assigned developer to ticket:" + originalTicket.Title + " has changed.",
                        Body = "The assigned developer for this ticket has changed. The new developer is " + DetermineAssignedUserName(model) + "."
                    };

                    ownerMailer.Send(ownerNotification);
                }
                else
                {
                    var ownerMailer = new EmailService();
                    var ownerNotification = new IdentityMessage
                    {
                        Destination = originalTicket.OwnerUser.Email,
                        Subject = "The assigned developer to ticket:" + originalTicket.Title + " has changed.",
                        Body = "The assigned developer for this ticket has changed.  This ticket is now unassigned."
                    };

                    ownerMailer.Send(ownerNotification);
                }
                if (originalTicket.AssignedToUserId != null)
                {
                    var developerMailer = new EmailService();
                    var developerNotification = new IdentityMessage
                    {
                        Destination = originalTicket.AssignedToUser.Email,
                        Subject = "You have been assigned a new ticket.",
                        Body = "Ticket - " + originalTicket.Title + " has been assigned to you."
                    };

                    developerMailer.Send(developerNotification);
                }
            }
        }

        public void CheckForHistory(Tickets originalTicket, Tickets model )
        {
            //Check Description
            if (originalTicket.Description != model.Description)
            {
                var history = new TicketHistories
                {
                    Property = "Description",
                    Oldvalue = originalTicket.Description,
                    NewValue = model.Description,
                    TicketId = originalTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                Changed = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
            //Check Status
            if (originalTicket.TicketStatusId != model.TicketStatusId)
            {
                var history = new TicketHistories
                {
                    Property = "Staus",
                    Oldvalue = originalTicket.TicketStatus.Name,
                    NewValue = model.TicketStatus.Name,
                    TicketId = originalTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Changed = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
            //Check Assigned User
            if (originalTicket.AssignedToUserId != model.AssignedToUserId)
            {
                var history = new TicketHistories();

                history.Property = "Assigned User";
                history.Oldvalue = DetermineAssignedUserName(originalTicket);
                history.NewValue = DetermineAssignedUserName(model);
                history.TicketId = originalTicket.Id;
                history.UserId = HttpContext.Current.User.Identity.GetUserId();
                history.Changed = DateTime.Now;

                db.TicketHistories.Add(history);
            }
        }

        //Determine assigned user because it could be possible for the user to not be assigned to anytihng
        public string DetermineAssignedUserName(Tickets ticket)
        {
            string name = "";

            if (ticket.AssignedToUserId == null)
                name = "Unassigned";

            if (ticket.AssignedToUserId != null)
            {
                var developer = db.Users.Find(ticket.AssignedToUserId);

                if (developer.DisplayName != null)
                {
                    name = developer.DisplayName;
                }

                else
                {
                    name = developer.Email;
                }
            }

            return name;
        }




    }
}
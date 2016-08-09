using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BugTrackerVS_16.Models.User_Roles
{
    public class UserRolesHelper
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManger;
        private RoleManager<IdentityRole> roleManger;

        public UserRolesHelper()
        {
            db = new ApplicationDbContext();
            userManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        public bool IsUserInRole(string userId, string rolesName)
        {
            return userManger.IsInRole(userId, rolesName);
        }

        public IList<string> ListUserRoles(string userId)
        {
            return userManger.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManger.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManger.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public IList<ApplicationUser> UsersInRole (string roleName)
        {
            var userIDs = roleManger.FindByName(roleName).Users.Select(r => r.UserId);
                return userManger.Users.Where(u => userIDs.Contains(u.Id)).ToList();
        }

        public IList<ApplicationUser>UsersNotInRole (string roleName)
        {
            var userIDs = System.Web.Security.Roles.GetUsersInRole(roleName);
            return userManger.Users.Where(u => !userIDs.Contains(u.Id)).ToList();
        }
    }
}
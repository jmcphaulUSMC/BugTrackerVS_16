using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BugTrackerVS_16.Models.User_Roles
{
    public class ProjectsHelper
    {
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();


        public bool IsUserInRoleInProject(string userId, string roleName)
        {
            return manager.IsInRole(userId, roleName);
        }

        public ICollection<string> ListUserRolesInProject(string userId)
        {
            return manager.GetRoles(userId);
        }

        public bool AddUserToProject(string userId, string roleName)
        {
            var result = manager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool AddUserToProjects(string userId, string[] roleNames)
        {
            var result = manager.AddToRoles(userId, roleNames);
            return result.Succeeded;
        }

        public bool RemoveUserFromProject(string userId, string roleName)
        {
            var result = manager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInProject(string roleName)
        {
            var reslutlList = new List<ApplicationUser>();
            var List = manager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRoleInProject(user.Id, roleName))
                    reslutlList.Add(user);
            }
            //var roleId = db.Roles.FirstOrDefault(r => r.Name == roleName).Id;
            //resultList = manager.Users.Where(u => u.Roels.Any(r => r.RoleId === roleId)).ToList();
            return reslutlList;
        }

        public ICollection<ApplicationUser> UsersNotInProject(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = manager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRoleInProject(user.Id, roleName))
                    resultList.Add(user);
            }
            //var roleId = db.Roles.FirstOrDefault(r => r.Name == roleName).Id;
            //resultList = manager.Users.Where(u => u.Roles.Any(r => r.RolesId != roleId)).ToList();
            return resultList;
        }
    }
}
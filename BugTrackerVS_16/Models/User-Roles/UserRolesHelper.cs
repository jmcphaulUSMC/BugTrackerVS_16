﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Security.Principal;

namespace BugTrackerVS_16.Models.User_Roles
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();


        public bool IsUserInRole(string userId, string roleName)
        {
            return manager.IsInRole(userId, roleName);
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return manager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = manager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool AddUserToRoles(string userId, string[] roleNames)
        {
            var result = manager.AddToRoles(userId, roleNames);
            return result.Succeeded;
        }


        public bool RemoveUserFromRole (string userId, string roleName)
        {
            var result = manager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        internal object IsUserInRole(IPrincipal user, object roleName)
        {
            throw new NotImplementedException();
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var reslutlList = new List<ApplicationUser>();
            var List = manager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    reslutlList.Add(user);
            }
            //var roleId = db.Roles.FirstOrDefault(r => r.Name == roleName).Id;
            //resultList = manager.Users.Where(u => u.Roels.Any(r => r.RoleId === roleId)).ToList();
            return reslutlList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = manager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            //var roleId = db.Roles.FirstOrDefault(r => r.Name == roleName).Id;
            //resultList = manager.Users.Where(u => u.Roles.Any(r => r.RolesId != roleId)).ToList();
            return resultList;
        }
    }
}
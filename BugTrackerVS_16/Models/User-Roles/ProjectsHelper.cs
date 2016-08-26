using BugTrackerVS_16.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BugTrackerVS_16.Models.User_Roles
{
    public class ProjectsHelper
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);//go to the database and find the prjojects table and find the projectId
            return project.Users.Any(u => u.Id == userId);// find any users in a project with an Id equal to userId
        }

        public IList<Projects> ListUserInProjects(string userId)
        {
            var user = db.Users.Find(userId);
            return user.Projects.ToList();
        }

        public IList<Projects> ListUsersInProject(string userId)
        {
            var user = db.Users.Find(userId);
            return user.Projects.ToList();
        }

        public bool AddUserToProject(string userId, int? projectId)//passing in userId and projectId to method
        {
            return AlterUserInProject(userId, projectId, true);//Call 'AlterUserInProject', passing userId, projectId and 'true' - true tells 'AlterUserInProject' that user is being added to project
        }

        public bool AlterUserInProject(string userId, int? projectId, bool addingUser)//passing in userId, projectId and addingUser parameters
        {
            if (userId != null && projectId != null)//Make sure the userId and the projectId are not null
            {
                var project = db.Projects.Find(projectId);//Get the project from the database
                var user = db.Users.Find(userId);//Get the user from the database
                if (addingUser)//check to see if user is being added or removed from project
                    project.Users.Add(user);//If 'addingUser' is true, Add the user to the project
   
            else
                project.Users.Remove(user);//If 'addingUser' is false, Remove user from project
                db.SaveChanges();//Save chages to the database
                return true;//return true means that the user was added to the project successfully
            }
            return false;//if the userId or projectId are null, return false, meaning that the user was not added to the project
        }

        public bool RemoveUserFromProject(string userId, int? projectId)
        {
            return AlterUserInProject(userId, projectId, false);
        }

        public List<ApplicationUser> UsersInProject(int id)
        {
            var project = db.Projects.Find(id);
            var users = project.Users.ToList();
            return users;
        }

        public IList<ApplicationUser> UsersNotInProject(int projectId)//method takes projectId parameter
        {
           return db.Users.Where(u => u.Projects.All(pu => pu.Id != projectId)).ToList();
        }

        //find projects assigned user to project
        public IList<Projects> AssignedUserToProject(string id)
        {
            //go to the database and find projects by id 
             return db.Users.Find(id).Projects.ToList();

            //go to the database and to the projects table find the projects where the project user collection contains that user
            //var AssignProject = db.Projects.Where(p => p.Users.Contains(user));

            //return AssignProject.ToList();

            
        }
    }
}
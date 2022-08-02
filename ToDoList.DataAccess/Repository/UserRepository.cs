
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DataAccess.Data;
using ToDoList.DataAccess.IRepository;
using ToDoList.Models.Models;

namespace ToDoList.DataAccess.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
     

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }
        public async Task<int> CountAsync(string search, string userId, string roleId)
        {
            HttpContextAccessor httpContext = new HttpContextAccessor();
            var userList = await _db.ApplicationUsers.Where(x => x.Id != userId).ToListAsync();
            var roleList = await _db.Roles.ToListAsync();
            var userRole = await _db.UserRoles.ToListAsync();
            foreach (var user in userList)
            {
                var role = userRole.Where(x => x.UserId == user.Id).ToList();
                List<string> roleName = new List<string>();
                List<string> roleIdList = new List<string>();

                foreach (var roles in role)
                {
                    roleName.Add(roleList.FirstOrDefault(x => x.Id == roles.RoleId).Name);
                    roleIdList.Add(roleList.FirstOrDefault(x => x.Id == roles.RoleId).Id);
                }
                user.Role = roleName;
                user.RoleId = roleIdList;

            }
            if (!String.IsNullOrEmpty(search))
            {
                userList = userList.Where(x => x.Name.Contains(search) || x.UserName.Contains(search) ||
                        x.Email.Contains(search)).ToList();
                if (httpContext.HttpContext.User.IsInRole("Program Officer"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Admin") && !x.Role.Contains("Super Admin")).ToList();
                }
                if (httpContext.HttpContext.User.IsInRole("Admin"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Super Admin")).ToList();
                }


                if (!String.IsNullOrEmpty(roleId))
                {
                    userList = userList.Where(x => x.RoleId.Contains(roleId)).ToList();
                }

            }
            else
            {
               
                if (httpContext.HttpContext.User.IsInRole("Program Officer"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Admin") && !x.Role.Contains("Super Admin")).ToList();
                }
                if (httpContext.HttpContext.User.IsInRole("Admin"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Super Admin")).ToList();
                }
                if (!String.IsNullOrEmpty(roleId))
                {
                    userList = userList.Where(x => x.RoleId.Contains(roleId)).ToList();
                }
            }
            return userList.Count();

        }

        public async Task<IEnumerable<ApplicationUser>> SearchAsync(string search, string userId, string roleId, int pageNo, int pageSize)
        {
            HttpContextAccessor httpContext = new HttpContextAccessor();
            var userList = await _db.ApplicationUsers.Where(x => x.Id != userId).ToListAsync();
            var roleList = await _db.Roles.ToListAsync();
            var userRole = await _db.UserRoles.ToListAsync();
            foreach (var user in userList)
            {
                var role = userRole.Where(x => x.UserId == user.Id).ToList();
                List<string> roleName = new List<string>();
                List<string> roleIdList = new List<string>();

                foreach (var roles in role)
                {
                    roleName.Add(roleList.FirstOrDefault(x => x.Id == roles.RoleId).Name);
                    roleIdList.Add(roleList.FirstOrDefault(x => x.Id == roles.RoleId).Id);
                }
                user.Role = roleName;
                user.RoleId = roleIdList;
            }
            if (!String.IsNullOrEmpty(search))
            {
                userList = userList.Where(x => x.Name.Contains(search) || x.UserName.Contains(search) ||
                        x.Email.Contains(search)).ToList();
                if (httpContext.HttpContext.User.IsInRole("Program Officer"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Admin") && !x.Role.Contains("Super Admin")).ToList();
                }
                if (httpContext.HttpContext.User.IsInRole("Admin"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Super Admin")).ToList();
                }
                if (!String.IsNullOrEmpty(roleId))
                {
                    userList = userList.Where(x => x.RoleId.Contains(roleId)).ToList();
                }

            }
            else
            {
                
                if (httpContext.HttpContext.User.IsInRole("Program Officer"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Admin") && !x.Role.Contains("Super Admin")).ToList();
                }
                if (httpContext.HttpContext.User.IsInRole("Admin"))
                {
                    userList = userList.Where(x => !x.Role.Contains("Super Admin")).ToList();
                }
                if (!String.IsNullOrEmpty(roleId))
                {
                    userList = userList.Where(x => x.RoleId.Contains(roleId)).ToList();
                }
            }
            return userList.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        }

        public async Task UpdateAsync(ApplicationUser applicationUser)
        {
            var applicationUserObj = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == applicationUser.Id);
            if (applicationUserObj != null)
            {
                applicationUserObj.Name = applicationUser.Name;
                applicationUserObj.PhoneNumber = applicationUser.PhoneNumber;              
            }
        }
        
    }
}

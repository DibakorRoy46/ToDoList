using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DataAccess.Data;
using ToDoList.Models.ViewModels;
using ToDoList.Utility;

namespace ToDoList.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _db = db;
        }
        #region Index
        [Route("Role")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult>RoleTable(string searchValue,int pageNo,int pageSize)
        {
            pageNo = pageNo != 0 ? pageNo : 1;
            pageSize = 10;
            IEnumerable<IdentityRole> roleList = await _roleManager.Roles.ToListAsync();
            var numberOfRole = roleList.Count();
            if (!String.IsNullOrEmpty(searchValue))
            {
                 roleList = await _roleManager.Roles.
                       Where(x => x.Name.Contains(searchValue)).Skip((pageNo - 1) * pageSize).Take(pageSize)
                       .ToListAsync();
                numberOfRole = roleList.Count();
            }
            else
            {
                roleList= await _roleManager.Roles.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
           
            }
            
            RoleVM roleVM = new RoleVM()
            {
                RoleList= roleList,
                Search=searchValue,
                Pager=new Pager(numberOfRole,pageNo,pageSize)
            };
            return PartialView("_RoleTable", roleVM);
        }
        #endregion
        #region Upsert
        [HttpGet]
        [Route("Role/Upsert")]
        public async Task<IActionResult> Upsert(string id)
        {
            if (String.IsNullOrEmpty(id))
            {             
                return View();
            }
            else
            {
                //update
                var objFromDb = await _db.Roles.FirstOrDefaultAsync(u => u.Id == id);
                return View(objFromDb);
            }
        }

        [HttpPost]

        [Route("Role/Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roleObj)
        {         
                //update
                var objRoleFromDb = _db.Roles.FirstOrDefault(u => u.Id == roleObj.Id);                
                if(objRoleFromDb!=null)
                {
                
                    objRoleFromDb.Name = roleObj.Name;
                    objRoleFromDb.NormalizedName = roleObj.Name.ToUpper();
                    await _roleManager.UpdateAsync(objRoleFromDb);
                    TempData["Message"] = "Role updated successfully";
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole() {Id=roleObj.Id ,Name = roleObj.Name });
                    TempData["Message"] = "Role created successfully";
                }        
                return RedirectToAction(nameof(Index));

        }

        #endregion
        #region Delelte
        [HttpPost]
        public async Task<IActionResult>Delete(string id, string searchValue)
        {
            try
            {
                if(!String.IsNullOrEmpty(id))
                {
                    var roleObj = await _roleManager.Roles.
                        FirstOrDefaultAsync(x => x.Id.ToLower().Equals(id.ToLower()));
                    if (roleObj==null)
                    {
                        TempData["message"] = "Role Not Found";
                        return RedirectToAction(nameof(Index));
                    }
                    var isRoleAssign = await _db.UserRoles.
                        Where(x => x.RoleId.ToLower().Equals(roleObj.Id.ToLower())).ToListAsync();
                    if(isRoleAssign.Count()>0)
                    {
                        return NotFound();
                    }
                    await _roleManager.DeleteAsync(roleObj);
                    int  pageNo =1;
                    int pageSize = 10;
                    IEnumerable<IdentityRole> roleList = await _roleManager.Roles.ToListAsync();
                    var numberOfRole = roleList.Count();
                    if (!String.IsNullOrEmpty(searchValue))
                    {
                        roleList = await _roleManager.Roles.
                              Where(x => x.Name.Contains(searchValue)).Skip((pageNo - 1) * pageSize).Take(pageSize)
                              .ToListAsync();
                        numberOfRole = roleList.Count();
                    }
                    else
                    {
                        roleList = await _roleManager.Roles.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

                    }

                    RoleVM roleVM = new RoleVM()
                    {
                        RoleList = roleList,
                        Search = searchValue,
                        Pager = new Pager(numberOfRole, pageNo, pageSize)
                    };
                    return PartialView("_RoleTable", roleVM);
                }
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Exists
        public async Task<IActionResult> ExistsName(string name)
        {
            try
            {
                if(!String.IsNullOrEmpty(name))
                {
                    var exist = await _db.Roles.Where(x => x.Name.ToLower().Equals(name.ToLower())).ToListAsync();
                    if(exist.Count()>0)
                    {
                        return Json(new { success = false });
                    }
                    return Json(new { success = true });

                }
                return Json(new { success = false });
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}

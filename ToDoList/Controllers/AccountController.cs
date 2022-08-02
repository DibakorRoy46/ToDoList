
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ToDoList.DataAccess.IRepository;
using ToDoList.Models.Models;
using ToDoList.Models.ViewModels;

namespace ToDoList.Controllers
{
    
 
    
    public class AccountController : Controller
    {
        #region Basic
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public AccountController(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            UrlEncoder urlEncoder,
            IUnitOfWork unitOfWork,
            IEmailSender emailSender)
        {
            _urlEncoder = urlEncoder;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        #endregion


        #region Register
        [Route("Register")]
     
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
           
            var roleList = await _roleManager.Roles.ToListAsync();
     
            RegisterVM registerVM = new RegisterVM()
            {
                RoleList = roleList.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
              
            };
            
            return View(registerVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>Register( RegisterVM model)
        {
           

            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    RegisterDate = DateTime.Now.AddHours(6)
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                List<string> userRoleList = new List<string>();
                if (result.Succeeded)
                {
                    if(model.RoleSelected!=null )
                    {
                        
                        foreach (var roleName in model.RoleSelected)
                        {
                            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleName.ToString());
                            await _userManager.AddToRoleAsync(user, role.Name);
                            userRoleList.Add(role.Name);                           
                            await _unitOfWork.SaveAsync();
                            
                            
                        }                    
                    }
                    var count = userRoleList.Count();
                   
                    
                    TempData["success"] = "Account Created Successfully";
                 
                    return RedirectToAction("Index","User");
                }
                foreach(var errorMessage in result.Errors)
                {
                    ModelState.AddModelError("", errorMessage.Description);
                }
            }
            var roleList = await _roleManager.Roles.ToListAsync();
         

            model.RoleList = roleList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
           
            return View(model);
        }
        #endregion

        #region Login
        
        [Route("Login")] 
      
        [AllowAnonymous]
        public async Task<IActionResult>Login()
        {
            return View();
        }    
        
        [Route("Login")]   
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult>Login(LoginVM model,string returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~");
              var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,false, lockoutOnFailure: true);
                if(result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnurl))
                    {
                        return LocalRedirect(returnurl);
                    }
                    TempData["LoginSuccess"] = "Successfully Login";
                    return RedirectToAction("Index", "ToDo");
                }
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty,"Your Account is Block.Please Try After Sometimes");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }              
                return View(model);
          
        }
        #endregion


        #region LogOut
        [Route("Logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            
            TempData["LogoutSuccess"] = "Successfully Logout";
            return RedirectToAction(nameof(Login));
        }

        #endregion


        #region ChangePassword
        [Route("ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword()
        {
            ChangePasswordVM model = new ChangePasswordVM();
            model.IsSuccess = false;
            return View(model);
        }
        [HttpPost]
        [Route("ChangePassword")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    if(user!=null)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                        if(result.Succeeded)
                        {
                            model.IsSuccess = true;
                            ModelState.Clear();
                            return View(model);
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    return View(model);
                }

                return View(model);
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }
        #endregion


        
       
    }
}

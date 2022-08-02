using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToDoList.Models.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The Password length must must be between 6 to 50 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The Password and Confirm Password must be same")]
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }

        public IEnumerable<SelectListItem>? RoleList { get; set; }
        public IEnumerable<string>? RoleSelected { get; set; }
        
    }
}

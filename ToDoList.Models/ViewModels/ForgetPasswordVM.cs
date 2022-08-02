using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models.ViewModels
{
    public class ForgetPasswordVM
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Models;

namespace ToDoList.Models.ViewModels
{
    public class UserRolePartialVM
    {
        public IEnumerable<UserRole> UserRoleList { get; set; }
        public string UserId { get; set; }
    }
}

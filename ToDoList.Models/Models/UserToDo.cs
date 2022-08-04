
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models.Models
{
    public class UserToDo
    {
        public Guid ToDoId { get; set; }
        public string? UserId { get; set; }
        public ToDo? ToDo { get; set; }
        public ApplicationUser? User { get; set; }
    }
}

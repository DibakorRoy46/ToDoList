using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Models;

namespace ToDoList.Models.ViewModels
{
    internal class UserToDoVM
    {
        public ToDo ToDo { get; set; }
        public bool Stauts { get; set; }
    }
}

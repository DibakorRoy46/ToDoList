using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Models;
using ToDoList.Utility;

namespace ToDoList.Models.ViewModels
{
    public class ToDoVM
    {
        public IEnumerable<ToDo> ToDoList { get; set; }
        public string Search { get; set; }
        public Pager Pager { get; set; }
    }
}

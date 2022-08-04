
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models.Models
{
    public class ToDo
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string ToDoDetails { get; set; }      
        public virtual IEnumerable<UserToDo>? UserToDo { get; set; }
        [NotMapped]
        public bool Status { get; set; }
    }
}

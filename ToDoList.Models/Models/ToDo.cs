
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public bool Status { get; set; }

    }
}

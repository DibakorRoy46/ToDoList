using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Models;

namespace ToDoList.DataAccess.IRepository
{
    public interface IToDoRepository : IRepository<ToDo>
    {      
        Task UpdateAsync(ToDo toDo);
        Task<int> CountAsync(string searchValue);
        Task<IEnumerable<ToDo>> SearchAsync(string searchValue, int pageNo, int pageSize);
    }
}

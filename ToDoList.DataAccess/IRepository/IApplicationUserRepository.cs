using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Models;

namespace ToDoList.DataAccess.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        
        Task<int> CountAsync(string search);
        Task<IEnumerable<ApplicationUser>> SearchAsync(string search, int pageNo, int pageSize);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.DataAccess.IRepository
{
    public interface IUnitOfWork:IDisposable
    {   
        public IToDoRepository ToDo { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IUserRepository User { get; }
       
        Task SaveAsync();
    }
}

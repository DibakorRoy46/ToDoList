using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DataAccess.Data;
using ToDoList.DataAccess.IRepository;
using ToDoList.Models.Models;

namespace ToDoList.DataAccess.Repository
{
    public class ToDoRepository : Repository<ToDo>, IToDoRepository
    {
        private readonly ApplicationDbContext _db;
        public ToDoRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public async Task<int> CountAsync(string searchValue)
        {
           
            if(!String.IsNullOrEmpty(searchValue))
            {
                return await _db.ToDo.Where(x => x.ToDoDetails.Contains(searchValue)).CountAsync();
            }
            return await _db.ToDo.CountAsync();
        }

        public async Task<IEnumerable<ToDo>> SearchAsync(string searchValue, int pageNo, int pageSize)
        {
            if (!String.IsNullOrEmpty(searchValue))
            {
                return await _db.ToDo.Where(x => x.ToDoDetails.Contains(searchValue)).ToListAsync();
            }
            return await _db.ToDo.ToListAsync();
        }

        public async Task UpdateAsync(ToDo toDo)
        {
            var toDoObj = await _db.ToDo.FirstOrDefaultAsync(x => x.Id == toDo.Id);
            if(toDoObj!=null)
            {
                toDoObj.ToDoDetails = toDo.ToDoDetails;
                toDoObj.Status = toDo.Status;                      
            }
        }
    }
}

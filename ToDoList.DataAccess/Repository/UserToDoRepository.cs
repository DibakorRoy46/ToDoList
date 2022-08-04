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
    public class UserToDoRepository:Repository<UserToDo>,IUserToDoRepository
    {
        private readonly ApplicationDbContext _db;
        public UserToDoRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
    }
}

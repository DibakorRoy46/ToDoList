﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DataAccess.Data;
using ToDoList.DataAccess.IRepository;
using ToDoList.DataAccess.Repository;

namespace ToDoList.Data.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ToDo = new ToDoRepository(db);        
        }
        public IToDoRepository ToDo { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
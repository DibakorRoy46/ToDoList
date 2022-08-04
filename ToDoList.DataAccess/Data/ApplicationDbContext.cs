using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Models;

namespace ToDoList.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserToDo> UserTodo { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserToDo>().HasKey(x => new { x.UserId, x.ToDoId });

            builder.Entity<UserToDo>().
                HasOne<ApplicationUser>(x => x.User).
                WithMany(x => x.UserToDo).
                HasForeignKey(x => x.UserId);

            builder.Entity<UserToDo>().
                HasOne<ToDo>(x => x.ToDo).
                WithMany(x => x.UserToDo).
                HasForeignKey(x => x.ToDoId);

         
        }
    }
}
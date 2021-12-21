using Microsoft.EntityFrameworkCore;
using minimal_api_demo.Entities;

namespace minimal_api_demo.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options): base(options) { }
        protected override void OnModelCreating(ModelBuilder builder) 
        { 
            builder.Entity<ToDoItem>().HasKey(x => x.Id);
            builder.Entity<ToDoItem>().Property(x => x.Title).IsRequired();
        }
        public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
    }
}

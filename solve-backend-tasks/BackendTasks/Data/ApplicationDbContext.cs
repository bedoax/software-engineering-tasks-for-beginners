using BackendTasks.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendTasks.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Book> Books { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().ToTable("Book").HasKey(b => b.id);
        }
    }
}

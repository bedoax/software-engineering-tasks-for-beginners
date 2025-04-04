using BackendTasks.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendTasks.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().ToTable("Books").HasKey(b => b.ID);
            modelBuilder.Entity<Author>().ToTable("Authors").HasKey(a => a.ID);
            modelBuilder.Entity<Book>().HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorID);
        }
    }
}

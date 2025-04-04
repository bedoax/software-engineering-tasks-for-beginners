using Microsoft.EntityFrameworkCore;
using BackendTask6.Model;

namespace BackendTask6.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Role Entity Configuration
            modelBuilder.Entity<Role>()
                .ToTable("Roles")
                .HasKey(r => r.id);

            // Post Entity Configuration
            modelBuilder.Entity<Post>()
                .ToTable("Post")
                .HasKey(p => p.id);

            // User Entity Configuration
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.role)
                .WithMany(r => r.users)
                .HasForeignKey(u => u.idRole);

            modelBuilder.Entity<User>()
                .HasMany(u => u.posts)
                .WithOne(p => p.user)
                .HasForeignKey(p => p.userId);

            // Seeding Data for Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { id = 1, name = "Admin" },
                new Role { id = 2, name = "Reviewer" }
            );

            // Seeding Data for Users
            modelBuilder.Entity<User>().HasData(
                new User { id = 1, username = "admin_user", Password = "Admin123", idRole = 1 },
                new User { id = 2, username = "reviewer_1", Password = "Review123", idRole = 2 },
                new User { id = 3, username = "reviewer_2", Password = "Review123", idRole = 2 },
                new User { id = 4, username = "reviewer_3", Password = "Review123", idRole = 2 },
                new User { id = 5, username = "reviewer_4", Password = "Review123", idRole = 2 },
                new User { id = 6, username = "reviewer_5", Password = "Review123", idRole = 2 },
                new User { id = 7, username = "reviewer_6", Password = "Review123", idRole = 2 },
                new User { id = 8, username = "reviewer_7", Password = "Review123", idRole = 2 },
                new User { id = 9, username = "reviewer_8", Password = "Review123", idRole = 2 },
                new User { id = 10, username = "reviewer_9", Password = "Review123", idRole = 2 }
            );


        }
    }
}

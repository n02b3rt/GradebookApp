using GradebookApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GradebookApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Teacher)
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(t => t.UserId);
        }
    }
}

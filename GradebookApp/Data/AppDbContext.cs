using Microsoft.EntityFrameworkCore;

namespace GradebookApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Dodasz tu później DbSety np. Students, Grades, itd.
    }
}

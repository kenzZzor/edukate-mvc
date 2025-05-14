using Microsoft.AspNetCore.Identity;                      // для типа IdentityUser
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  // для IdentityDbContext<>
using Microsoft.EntityFrameworkCore;
using EdukateMvc.Models;

namespace EdukateMvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts)
            : base(opts) { }

        public DbSet<Course> Courses { get; set; }
        // DbSet<Category>, DbSet<Instructor> и т.д.
    }
}

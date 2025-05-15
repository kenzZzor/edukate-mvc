using EdukateMvc.Data;
using EdukateMvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdukateMvc.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _ctx;
        public CourseRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Course>> GetAllAsync() =>
            await _ctx.Courses.AsNoTracking().ToListAsync();

        public async Task<Course?> GetByIdAsync(int id) =>
            await _ctx.Courses.FindAsync(id);

        public async Task AddAsync(Course course)
        {
            _ctx.Courses.Add(course);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _ctx.Courses.Update(course);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _ctx.Courses.FindAsync(id);
            if (entity != null)
            {
                _ctx.Courses.Remove(entity);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}

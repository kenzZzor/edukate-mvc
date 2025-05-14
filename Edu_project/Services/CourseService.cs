using EdukateMvc.Models;
using EdukateMvc.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdukateMvc.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        public CourseService(ICourseRepository repo) => _repo = repo;

        public Task<IEnumerable<Course>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Course?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task CreateAsync(Course course)
        {
            course.CreatedAt = DateTime.UtcNow;
            await _repo.AddAsync(course);
        }

        public async Task EditAsync(Course course)
        {
            await _repo.UpdateAsync(course);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}

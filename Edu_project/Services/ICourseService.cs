using EdukateMvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdukateMvc.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task CreateAsync(Course course);
        Task EditAsync(Course course);
        Task DeleteAsync(int id);
    }
}

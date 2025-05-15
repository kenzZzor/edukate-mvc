using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using EdukateMvc.Models;
using EdukateMvc.Repositories;

namespace EdukateMvc.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMemoryCache _cache;

        // ключи, чтобы не писать строки по всему коду
        private const string CacheAll = "courses_all";
        private static string CacheById(int id) => $"course_{id}";

        public CourseService(ICourseRepository repo, IMemoryCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        /* ---------- ЧТЕНИЕ ---------- */

        // 1. Сначала пытаемся взять из кэша.
        // 2. Если нет — читаем из репозитория и кладём в кэш на 5 минут.
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            if (_cache.TryGetValue(CacheAll, out IEnumerable<Course> data))
                return data;

            data = await _repo.GetAllAsync();
            _cache.Set(CacheAll, data, TimeSpan.FromMinutes(5));
            return data;
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            if (_cache.TryGetValue(CacheById(id), out Course? item))
                return item;

            item = await _repo.GetByIdAsync(id);
            if (item != null)
                _cache.Set(CacheById(id), item, TimeSpan.FromMinutes(5));
            return item;
        }

        /* ---------- ЗАПИСЬ / ИНВАЛИДАЦИЯ ---------- */

        public async Task CreateAsync(Course course)
        {
            course.CreatedAt = DateTime.UtcNow;
            await _repo.AddAsync(course);
            InvalidateCache(course.Id);
        }

        public async Task UpdateAsync(Course course)
        {
            await _repo.UpdateAsync(course);
            InvalidateCache(course.Id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            InvalidateCache(id);
        }

        /* ---------- ВСПОМОГАТЕЛЬНОЕ ---------- */

        private void InvalidateCache(int courseId)
        {
            _cache.Remove(CacheAll);            // перечень курсов
            _cache.Remove(CacheById(courseId)); // конкретный курс
        }
    }
}

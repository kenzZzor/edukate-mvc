using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdukateMvc.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        public string InstructorName { get; set; }
        public double Rating { get; set; }
    }
}

using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services
{
    public interface ICourseService
    {
        public List<Course> GetCoursesByUser(int userId);
        public List<Course> GetCourses();
        public List<Course> GetCoursesByFolder(int folderId);
        public Course GetCourse(int id);
        public bool DeleteCourse(int id);
        public bool UpdateCourse(Course course);
        public bool CreateCourse(Course course);
        public bool SaveChanged();
    }
}

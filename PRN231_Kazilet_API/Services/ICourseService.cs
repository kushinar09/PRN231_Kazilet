using PRN231_Kazilet_API.Models.Dto;

namespace PRN231_Kazilet_API.Services
{
    public interface ICourseService
    {
        public bool AddCourse(CourseDto courseDto);
        public CourseDto GetCourse(int courseId);
        public bool UpdateCourse(CourseDto courseDto);
        public bool DeleteCourse(int courseId);
    }
}

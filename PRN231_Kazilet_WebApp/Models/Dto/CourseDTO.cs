using PRN231_Kazilet_API.Models;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Models.Dto
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User CreatedBy { get; set; }
        public int QuestionCount { get; set; }
    }
}

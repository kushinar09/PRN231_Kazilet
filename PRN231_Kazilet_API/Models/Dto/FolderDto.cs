using PRN231_Kazilet_API.Models.Entities;
using System.Text.Json.Serialization;

namespace PRN231_Kazilet_API.Models.Dto
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<FolderCourse> FolderCourses { get; set; }
    }
}

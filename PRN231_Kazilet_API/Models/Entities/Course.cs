using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Course
    {
        public Course()
        {
            FolderCourses = new HashSet<FolderCourse>();
            GameplaySettings = new HashSet<GameplaySetting>();
            LearningHistories = new HashSet<LearningHistory>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public string? CoursePassword { get; set; }
        public bool? IsPublic { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<FolderCourse> FolderCourses { get; set; }
        public virtual ICollection<GameplaySetting> GameplaySettings { get; set; }
        public virtual ICollection<LearningHistory> LearningHistories { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}

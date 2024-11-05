using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models
{
    public partial class LearningHistory
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime? LearningDate { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            GameplayAnswers = new HashSet<GameplayAnswer>();
        }

        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? Content { get; set; }
        public bool? IsMarked { get; set; }

        public virtual Course? Course { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<GameplayAnswer> GameplayAnswers { get; set; }
    }
}

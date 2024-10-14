using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            Gameplays = new HashSet<Gameplay>();
        }

        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? Content { get; set; }
        public bool? IsMarked { get; set; }
        public int? Status { get; set; }

        public virtual Course? Course { get; set; }
        public virtual QuestionStatus? StatusNavigation { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Gameplay> Gameplays { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual Course? Course { get; set; }
        public virtual QuestionStatus? StatusNavigation { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Gameplay> Gameplays { get; set; }
    }
}

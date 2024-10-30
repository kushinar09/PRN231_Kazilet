using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Answer
    {
        public Answer()
        {
            Gameplays = new HashSet<Gameplay>();
        }

        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? Content { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual Question? Question { get; set; }
        public virtual ICollection<Gameplay> Gameplays { get; set; }
    }
}

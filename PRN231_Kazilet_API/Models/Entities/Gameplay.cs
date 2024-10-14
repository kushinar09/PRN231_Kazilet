using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Gameplay
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public int? QuestionId { get; set; }
        public string? PlayerAnswer { get; set; }
        public bool? IsCorrect { get; set; }
        public int? Turn { get; set; }
        public int? Score { get; set; }
        public double? Duration { get; set; }

        public virtual GameplaySetting? CodeNavigation { get; set; }
        public virtual Question? Question { get; set; }
        public virtual User? User { get; set; }

        public Gameplay(string? Code, string? Username, int? Turn)
        {
            this.Code = Code;
            this.Username = Username;
            this.Turn = Turn;
        }
    }
}

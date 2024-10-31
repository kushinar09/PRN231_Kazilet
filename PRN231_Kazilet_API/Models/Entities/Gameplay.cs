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
        public int? PlayerAnswer { get; set; }
        public bool? IsCorrect { get; set; }
        public int? Turn { get; set; }
        public int? Score { get; set; }
        public double? Duration { get; set; }
        public int? Streak { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsGetResult { get; set; }

        public virtual GameplaySetting? CodeNavigation { get; set; }
        public virtual Answer? PlayerAnswerNavigation { get; set; }
        public virtual Question? Question { get; set; }
        public virtual User? User { get; set; }

        public Gameplay()
        {
        }

        public Gameplay(string? code, string? username, int? turn)
        {
            Code = code;
            Username = username;
            Turn = turn;
        }

    }
}
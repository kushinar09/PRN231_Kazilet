using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Answer
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? Content { get; set; }
        public bool? IsCorrect { get; set; }
        [JsonIgnore]
        public virtual Question? Question { get; set; }
    }
}

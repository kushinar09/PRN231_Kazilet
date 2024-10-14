﻿using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Models.Dto
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? Content { get; set; }
        public bool? IsMarked { get; set; }
        public int? Status { get; set; }
        public virtual ICollection<AnswerDto> Answers { get; set; }
    }
}

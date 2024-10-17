namespace PRN231_Kazilet_API.Models.Dto
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CreatedBy { get; set; }
        public string? CoursePassword { get; set; }
        public bool? IsPublic { get; set; }
        public int? NumOfQues { get; set; }
    }
}

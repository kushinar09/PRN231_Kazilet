using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class CourseService : ICourseService
    {
        private readonly PRN231_Kazilet_v2Context _context;
        private readonly IMapper _mapper;

        public CourseService(PRN231_Kazilet_v2Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool AddCourse(CourseDto courseDto)
        {
            if (courseDto == null)
            {
                return false;
            }

            // Tạo và thêm thực thể Course
            var courseEntity = new Course
            {
                Name = courseDto.Name,
                Description = courseDto.Description,
                CreatedAt = courseDto.CreatedAt ?? DateTime.Now,
                CreatedBy = courseDto.CreatedBy,
                CoursePassword = courseDto.CoursePassword,
                IsPublic = courseDto.IsPublic ?? false,
                Status = 1, // Sử dụng giá trị mặc định nếu Status là null
            };

            _context.Courses.Add(courseEntity);
            _context.SaveChanges(); // Lưu để sinh ra ID cho Course
            int courseId = courseEntity.Id;

            // Thêm các câu hỏi và câu trả lời nếu có
            if (courseDto.Questions != null)
            {
                foreach (var questionDto in courseDto.Questions)
                {
                    var questionEntity = new Question
                    {
                        CourseId = courseId,
                        Content = questionDto.Content,
                        IsMarked = questionDto.IsMarked
                    };

                    _context.Questions.Add(questionEntity);
                    _context.SaveChanges(); // Lưu để sinh ra ID cho Question
                    int questionId = questionEntity.Id;

                    // Thêm các câu trả lời nếu có
                    if (questionDto.Answers != null)
                    {
                        foreach (var answerDto in questionDto.Answers)
                        {
                            if (!string.IsNullOrWhiteSpace(answerDto.Content)) // Bỏ qua nếu content null hoặc rỗng
                            {
                                var answerEntity = new Answer
                                {
                                    Content = answerDto.Content,
                                    IsCorrect = answerDto.IsCorrect ?? false,
                                    QuestionId = questionId
                                };

                                _context.Answers.Add(answerEntity);
                            }
                        }
                    }
                }
            }

            // Lưu tất cả thay đổi trong một transaction
            return _context.SaveChanges() > 0;
        }

        public bool UpdateCourse(int courseId, CourseDto courseDto)
        {
            var existingCourse = _context.Courses
                .Include(c => c.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(c => c.Id == courseId);

            if (existingCourse == null || courseDto == null)
            {
                return false;
            }

            // Update basic course information
            existingCourse.Name = courseDto.Name;
            existingCourse.Description = courseDto.Description;
            existingCourse.IsPublic = courseDto.IsPublic ?? existingCourse.IsPublic;
            existingCourse.Status = courseDto.Status ?? existingCourse.Status;
            existingCourse.CoursePassword = courseDto.CoursePassword;

            // Delete existing questions and answers
            _context.Answers.RemoveRange(existingCourse.Questions.SelectMany(q => q.Answers));
            _context.Questions.RemoveRange(existingCourse.Questions);
            _context.SaveChanges(); // Ensure deletion is complete before adding new data

            // Add new questions and answers from courseDto
            foreach (var questionDto in courseDto.Questions)
            {
                var newQuestion = new Question
                {
                    CourseId = courseId,
                    Content = questionDto.Content,
                    IsMarked = questionDto.IsMarked
                };
                _context.Questions.Add(newQuestion);
                _context.SaveChanges(); // Save to generate Question ID

                // Add answers for the new question
                foreach (var answerDto in questionDto.Answers)
                {
                    if (!string.IsNullOrWhiteSpace(answerDto.Content))
                    {
                        var newAnswer = new Answer
                        {
                            Content = answerDto.Content,
                            IsCorrect = answerDto.IsCorrect ?? false,
                            QuestionId = newQuestion.Id
                        };
                        _context.Answers.Add(newAnswer);
                    }
                }
            }

            // Save all new changes
            return _context.SaveChanges() > 0;
        }


        public CourseDto GetCourse(int courseId)
        {
            var course = _context.Courses
    .Include(c => c.Questions) // Bao gồm danh sách Questions
    .ThenInclude(q => q.Answers) // Bao gồm cả danh sách Answers trong mỗi Question
    .FirstOrDefault(c => c.Id == courseId);

            return _mapper.Map<CourseDto>(course);
        }
    }
}

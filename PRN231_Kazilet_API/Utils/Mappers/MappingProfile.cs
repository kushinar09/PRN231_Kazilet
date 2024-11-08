using AutoMapper;
using PRN231_Kazilet_API.Models;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Utils.Mappers
{
    public class MappingProfile : Profile
    {
        PRN231_KaziletContext _context = new PRN231_KaziletContext();
        public MappingProfile() {
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<QuestionDto, Question>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<AnswerDto, Answer>();

            CreateMap<Answer, AnswerDto>();
            CreateMap<Course, CourseDto>()
                 .ForMember(dest => dest.CreateByName, opt => opt.MapFrom(src => src.CreatedByNavigation.Username)); 

            CreateMap<LearningHistory, LearningHistoryDto>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.numOfQues, opt => opt.MapFrom(src => src.Course.Questions.Count));
            CreateMap<LearningHistoryDto, LearningHistory>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => _context.Courses.FirstOrDefault(f=>f.Id==src.CourseId)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => _context.Users.FirstOrDefault(f=>f.Id==src.UserId)));
        }
    }
}

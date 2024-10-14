using AutoMapper;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Utils.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<QuestionDto, Question>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<AnswerDto, Answer>();

            CreateMap<Answer, AnswerDto>();
        }
    }
}

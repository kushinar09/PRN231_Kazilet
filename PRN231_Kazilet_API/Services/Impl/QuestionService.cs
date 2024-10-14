using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class QuestionService : IQuestionService
    {
        private readonly PRN231_KaziletContext _context;

        private readonly IMapper _mapper;

        public QuestionService(PRN231_KaziletContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<QuestionDto> GetAllQuestionsByCourse(int courseId)
        {
            List<Question> questions = _context.Questions
                .Include(q => q.Answers)
                .Where(q => q.CourseId == courseId).ToList();
            List<QuestionDto> questionDtos = _mapper.Map<List<QuestionDto>>(questions);
            List<AnswerDto> answerDtos = questionDtos[0].Answers.ToList();
            return questionDtos;
        }

        public QuestionDto GetById(int questionNumber, int courseId)
        {
            List<Question> question = _context.Questions
                .Include(q => q.Answers)
                .Where(q => q.CourseId == courseId)
                .Take(questionNumber)
                .ToList();
            if(question == null)
            {
                return null;
            }
            else
            {
                QuestionDto questionDto = _mapper.Map<QuestionDto>(question[question.Count - 1]);
                return questionDto;
            }
        }

        public QuestionDto GetById(int questionId)
        {
            Question question = _context.Questions.FirstOrDefault(q => q.Id == questionId);
            return _mapper.Map<QuestionDto>(question);
        }
    }
}

using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services
{
    public interface IQuestionService
    {
        public List<QuestionDto> GetAllQuestionsByCourse(int courseId);

        public QuestionDto GetById(int questionNumber, int courseId);
        public List<QuestionDto> GetRandom(int courseId, int numOfQues);
        public List<QuestionDto> GetQuestionsByIds(List<int> ids);
        public QuestionDto GetById(int questionId);
    }
}

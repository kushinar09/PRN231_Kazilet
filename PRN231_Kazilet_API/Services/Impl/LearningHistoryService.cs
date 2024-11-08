using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class LearningHistoryService : ILearningHistory
    {
        private readonly PRN231_KaziletContext _context;

        private readonly IMapper _mapper;

        public LearningHistoryService(PRN231_KaziletContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool AddLearningHistory(int userId,int courseId)
        {
            var existedOne = _context.LearningHistories.FirstOrDefault(l => l.CourseId == courseId && l.UserId == userId);
            var learningHistory = _mapper.Map<LearningHistory>(existedOne);
            
            if (existedOne != null) {
                learningHistory.LearningDate = DateTime.Now;
                _context.LearningHistories.Update(learningHistory);
            }
            else
            {
                LearningHistory lh = new LearningHistory();
                lh.CourseId = courseId;
                lh.UserId = userId;
                lh.LearningDate = DateTime.Now;
                _context.LearningHistories.Add(lh);
            }
            
            return _context.SaveChanges()>0?true : false;
        }

        public List<LearningHistoryDto> GetAllLearningHistoriesByUserId(int userId)
        {
            return _mapper.Map<List<LearningHistoryDto>>(_context.LearningHistories.Include(l => l.User).Include(l => l.Course).Include(l=>l.Course.Questions).Where(l => l.UserId == userId).ToList().OrderByDescending(l=>l.LearningDate));
        }
    }
}

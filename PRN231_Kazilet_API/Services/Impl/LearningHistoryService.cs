using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class LearningHistoryService : ILearningHistory
    {
        private readonly PRN231_Kazilet_v2Context _context;

        private readonly IMapper _mapper;

        public LearningHistoryService(PRN231_Kazilet_v2Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool AddLearningHistory(LearningHistoryDto learningHistory)
        {
            throw new NotImplementedException();
        }

        public List<LearningHistoryDto> GetAllLearningHistoriesByUserId(int userId)
        {
            return _mapper.Map<List<LearningHistoryDto>>(_context.LearningHistories.Include(l => l.User).Include(l => l.Course).Include(l => l.Course.Questions).Where(l => l.UserId == userId).ToList().OrderByDescending(l => l.LearningDate));
        }
    }
}

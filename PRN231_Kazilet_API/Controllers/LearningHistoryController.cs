using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;
using PRN231_Kazilet_API.Services;
using PRN231_Kazilet_API.Services.Impl;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningHistoryController : ControllerBase
    {
        private readonly ILearningHistory _learningHistoryService;
        public LearningHistoryController(ILearningHistory learningHistoryService)
        {
            _learningHistoryService = learningHistoryService;
        }
        [HttpGet("{userId}")]
        public IActionResult GetLearningHistories(int userId) {
            List<LearningHistoryDto> learningHistoryDtos = _learningHistoryService.GetAllLearningHistoriesByUserId(userId);
            if (learningHistoryDtos == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(learningHistoryDtos);
            }
        }
    }
}

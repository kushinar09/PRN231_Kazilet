using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Services;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        public CourseController(ICourseService courseService,IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetCoures()
        {
            var courseList = _mapper.Map<List<CourseDto>>(_courseService.GetCourses());
            return courseList==null? NotFound() : Ok(courseList); 
        }
        [HttpGet("GetCourseByUser")]
        public IActionResult GetCouresByUser(int userId)
        {
            var courseList = _mapper.Map<List<CourseDto>>(_courseService.GetCoursesByUser(userId));
            return courseList == null ? NotFound() : Ok(courseList);
        }
        [HttpGet("GetCourseByFolder")]
        public IActionResult GetCouresByFolder(int folderId)
        {
            var courseList = _mapper.Map<List<CourseDto>>(_courseService.GetCoursesByFolder(folderId));
            return courseList == null ? NotFound() : Ok(courseList);
        }
    }
}

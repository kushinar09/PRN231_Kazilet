using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Services;
using PRN231_Kazilet_API.Services.Impl;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private IFolderService _folderService;
        private readonly IHttpContextAccessor _contextAccessor;

        public FoldersController(IFolderService folderService, IHttpContextAccessor contextAccessor)
        {
            _folderService = folderService;
            _contextAccessor = contextAccessor;

        }
        //TODO: Sửa created By đổi qua getUser
        [HttpPost("Add")]
        public IActionResult AddFolder([FromQuery] string folderName)
        {
            FolderDto folderDto = new FolderDto();
            folderDto.CreatedBy = 1;
            folderDto.Name = folderName;
            _folderService.AddFolder(folderDto);
            return Ok(folderDto);
        }

        [HttpPost("AddCourse")]
        public IActionResult AddCourseToFolder([FromQuery] int folderId, [FromQuery] int courseId)
        {
           
            if(_folderService.AddCourseToFolder(courseId, folderId))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("RemoveCourse")]
        public IActionResult RemoveCourseToFolder([FromQuery] int folderId, [FromQuery] int courseId)
        {

            if (_folderService.RemoveCourseInFolder(courseId, folderId))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("RemoveFolder")]
        public IActionResult RemoveFolder([FromQuery] int folderId)
        {

            if (_folderService.RemoveFolder( folderId))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

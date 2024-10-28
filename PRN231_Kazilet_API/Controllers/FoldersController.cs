using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly PRN231_KaziletContext _context;

        public FoldersController(PRN231_KaziletContext context)
        {
            _context = context;
        }


        [HttpGet("folders/{userid}")]
        public IActionResult GetFoldersByUser(int userid)
        {
            var folders = _context.Folders.Where(f => f.CreatedByNavigation.Id == userid).ToList();
            if (folders == null || folders.Count == 0)
            {
                return NotFound("No folders found for user");
            }
            return Ok(folders);
        }
    }
}

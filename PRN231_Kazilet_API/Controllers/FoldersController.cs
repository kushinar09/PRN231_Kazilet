using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly PRN231_Kazilet_v2Context _context;

        public FoldersController(PRN231_Kazilet_v2Context context)
        {
            _context = context;
        }


        [HttpGet("folders/{userid}")]
        public IActionResult GetFoldersByUser(int userid)
        {
            var folders = _context.Folders.Include(f => f.CreatedByNavigation)
                                        .Where(c => c.CreatedByNavigation.Id == userid)
                                        .Select(f => new
                                        {
                                            Id = f.Id,
                                            Name = f.Name,
                                            Created_by = f.CreatedByNavigation,
                                            Created_at = f.CreatedAt
                                        })
                                        .ToList();
            if (folders == null || folders.Count == 0)
            {
                return NotFound("No folders found for user");
            }
            return Ok(folders);
        }
    }
}

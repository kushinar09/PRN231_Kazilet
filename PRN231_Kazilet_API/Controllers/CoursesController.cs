using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly PRN231_Kazilet_v2Context _context;

        public CoursesController(PRN231_Kazilet_v2Context context)
        {
            _context = context;
        }


        [HttpGet("search")]
        public IActionResult SearchCoursesByName([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Keyword is required.");
            }

            var courses = _context.Courses
                .Where(c => c.Name.Contains(keyword))
                .ToList();

            if (courses == null || courses.Count == 0)
            {
                return NotFound("No courses found with the provided keyword.");
            }

            return Ok(courses);
        }

        [HttpGet("by-folder/{folderId}/by-user/{userId}")]
        public IActionResult GetCoursesByFolderAndUser(int folderId, int userId)
        {
            var courses = _context.Courses
                .Where(c => c.Folders.Any(f => f.Id == folderId && f.CreatedBy == userId))
                .Include(c => c.Folders)
                .Include(c => c.CreatedByNavigation)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Created_at = c.CreatedAt,
                    Created_by = c.CreatedBy,
                    Password = c.CoursePassword,
                    isPublic = c.IsPublic

                })
                .ToList();

            if (courses == null || courses.Count == 0)
            {
                return NotFound("This folder hasn't any course created by the specified user.");
            }
            return Ok(courses);
        }

        [HttpPost("import")]
        public IActionResult ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Please upload a valid Excel file.");
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Bắt đầu từ hàng 2 để bỏ qua tiêu đề
                    {
                        var course = new Course
                        {
                            Name = worksheet.Cells[row, 2].Value?.ToString(),
                            Description = worksheet.Cells[row, 3].Value?.ToString(),
                            IsPublic = bool.Parse(worksheet.Cells[row, 4].Value?.ToString() ?? "true"),
                        };

                        _context.Courses.Add(course);
                    }

                    _context.SaveChanges();
                }
            }

            return Ok("Courses imported successfully from Excel.");
        }

        [HttpGet("export")]
        public IActionResult ExportToExcel()
        {
            var courses = _context.Courses.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Courses");
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Description";
                worksheet.Cells[1, 4].Value = "IsPublic";

                int row = 2;
                foreach (var course in courses)
                {
                    worksheet.Cells[row, 2].Value = course.Name;
                    worksheet.Cells[row, 3].Value = course.Description;
                    worksheet.Cells[row, 4].Value = course.IsPublic;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "Courses.xlsx";
                return File(stream, contentType, fileName);
            }
        }
    }
}

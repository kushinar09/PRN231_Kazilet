using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml;
using PRN231_Kazilet_API.Models.Entities;
using PRN231_Kazilet_API.Services;
using PRN231_Kazilet_API.Services.Impl;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly PRN231_Kazilet_v2Context _context;

        private readonly ICourseService _courseService;

        public CoursesController(PRN231_Kazilet_v2Context context, ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }

        [HttpGet]
        [Route("Details/{courseId}")]
        public IActionResult GetCourseDetails(int courseId)
        {
            return Ok(_courseService.GetCourse(courseId));
        }


        [HttpGet("by-folder/{folderid}")]
        public IActionResult GetCourseByFolder(int folderid)
        {
            /*
            var courses = _context.
                .Where(fc => fc.FolderId == folderid)    
                .Include(fc => fc.Course)               
                .ThenInclude(c => c.CreatedByNavigation) 
                .Select(fc => fc.Course)                 
                .ToList();



            if (courses == null || courses.Count == 0)
            {
                return NotFound("This folder hasn't have any course");
            }
            return Ok(courses);
            */
            return Ok();
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
                /*
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
                */
            }

            return Ok("Courses imported successfully from Excel.");
        }

        [HttpGet("export")]
        public IActionResult ExportToExcel()
        {
            var courses = _context.Courses.ToList();

            /*
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
            */
            return Ok();
        }

        [HttpGet]
        [Route("Recent/{userId}")]
        public IActionResult GetCourseRecent(int userId)
        {
            var courses = _context.LearningHistories
                .Where(fc => fc.UserId == userId)
                .Include(fc => fc.Course)
                .ThenInclude(c => c.CreatedByNavigation)
                .Select(fc => fc.Course)
                .ToList();

            return Ok(courses);
        }

        [HttpGet]
        [Route("Popular")]
        public IActionResult GetCoursePopular()
        {
            var courses = _context.Courses
                .Include(c => c.CreatedByNavigation)
                .Include(fc => fc.Questions)
                .Where(fc => fc.Questions.Count > 0)
                .OrderByDescending(fc => fc.Questions.Count)
                .Take(5)
                .ToList();

            foreach (var item in courses)
            {
                item.Questions = new List<Question>();
            }

            return Ok(courses);
        }

        [HttpGet]
        [Route("Users/Popular")]
        public IActionResult GetUserCoursePopular()
        {
            /*
            var users = _context.Users
                .Include(u => u.RoleNavigation)
                .ToList();

            foreach (var item in users)
            {
                item.numOfCourse = _context.Courses.Count(c => c.CreatedBy == item.Id);
                item.roleName = item.RoleNavigation.Role;
            }

            var popularUsers = users
                .Where(u => u.numOfCourse > 0)
                .OrderByDescending(u => u.numOfCourse)
                .Take(5)
                .ToList();

            return Ok(popularUsers);
            */
            return Ok();
        }
    }
}

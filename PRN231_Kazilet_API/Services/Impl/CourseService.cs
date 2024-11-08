using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;
using System.Diagnostics;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class CourseService : ICourseService
    {
        private readonly PRN231_KaziletContext _context;
        public CourseService(PRN231_KaziletContext context)
        {
            _context = context;
        }
        public bool CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            return SaveChanged();
        }

        public bool DeleteCourse(int id)
        {
            throw new NotImplementedException();
        }

        public Course GetCourse(int id)
        {
            return _context.Courses.Include(c=>c.Questions).FirstOrDefault(c => c.Id == id);
        }

        public List<Course> GetCourses()
        {
            return _context.Courses.Include(c=>c.CreatedByNavigation).ToList();
        }

        public List<Course> GetCoursesByFolder(int folderId)
        {
            //return _context.FolderCourses
            //        .Where(fc => fc.FolderId == folderId)
            //        .Select(fc => fc.Course)
            //        .ToList();
            return null;
        }

        public List<Course> GetCoursesByUser(int userId)
        {
            return _context.Courses.Where(c=>c.CreatedBy==userId).ToList();
        }

        
        public class CourseCount
        {
            public int CourseId { get; set; }
            public int Count { get; set; }
            public CourseCount(int courseId,int count)
            {
                CourseId = courseId;
                Count=count;
            }

        }
        public bool SaveChanged()
        {
            return _context.SaveChanges()>0?true:false;
        }

        public bool UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            return SaveChanged();
        }
    }
}

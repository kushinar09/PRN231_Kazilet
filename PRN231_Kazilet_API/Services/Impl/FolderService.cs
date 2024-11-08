using AutoMapper;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class FolderService : IFolderService
    {
        private readonly PRN231_Kazilet_v2Context _context;

        private readonly IMapper _mapper;

        public FolderService(PRN231_Kazilet_v2Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool AddCourseToFolder(int courseId, int folderId)
        {
            /*
            Folder f = _context.Folders.FirstOrDefault(f => f.Id == folderId);
            _context.FolderCourses.Add(new FolderCourse()
            {
                FolderId = folderId,
                CourseId = courseId
            });

            return _context.SaveChanges() > 0;
            */
            return true;
        }

        public bool AddFolder(FolderDto folderDto)
        {
            Folder f = new Folder()
            {
                Id = 0,
                Name = folderDto.Name,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = folderDto.CreatedBy
            };
            _context.Folders.Add(f);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveCourseInFolder(int courseId, int folderId)
        {
            /*
            FolderCourse folderCourse = _context.FolderCourses.FirstOrDefault(c => c.CourseId == courseId && c.FolderId == folderId);
            if (folderCourse != null)
            {
                _context.FolderCourses.Remove(folderCourse);
                return _context.SaveChanges() > 0;
            }
            */
            return false;
        }

        public bool RemoveFolder(int folderId)
        {
            /*
            Folder folder = _context.Folders.FirstOrDefault(f => f.Id == folderId);
            if (folder == null)
            {
                return false;
            }

            List<FolderCourse> folderCourses = _context.FolderCourses.Where(c => c.FolderId == folderId).ToList();
            if (folderCourses.Count > 0)
            {
                foreach (FolderCourse c in folderCourses)
                {
                    _context.FolderCourses.Remove(c);
                }
            }

            _context.Folders.Remove(folder);
            return _context.SaveChanges() > 0;
            */
            return true;
        }

    }
}

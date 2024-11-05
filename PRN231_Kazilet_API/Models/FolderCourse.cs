using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models
{
    public partial class FolderCourse
    {
        public int CourseId { get; set; }
        public int FolderId { get; set; }
        public int? PlayerAnswer { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Folder Folder { get; set; } = null!;
    }
}

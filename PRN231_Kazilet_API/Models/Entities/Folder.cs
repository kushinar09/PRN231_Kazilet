﻿using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Folder
    {
        public Folder()
        {
            FolderCourses = new HashSet<FolderCourse>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<FolderCourse> FolderCourses { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }
        public string? Link { get; set; }
        public DateTime? Date { get; set; }

        public virtual User? User { get; set; }
    }
}

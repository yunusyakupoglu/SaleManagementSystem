using System;

namespace Data.Models.Project
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string TagColor { get; set; }
        public Guid Product { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}

using System;

namespace Data.Models.Project
{
    public class Brand
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string ImgName { get; set; }
        public string BrandName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}

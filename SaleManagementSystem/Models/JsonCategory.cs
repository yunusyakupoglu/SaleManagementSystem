using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagementSystem.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; } // Nullable int to accommodate null values
        public List<Category> SubCategories { get; set; }
    }

    public class RootObject
    {
        public List<Category> Categories { get; set; }
    }
}
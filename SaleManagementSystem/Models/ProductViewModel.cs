using Data.Models.Dto;
using Data.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleManagementSystem.Models
{
    public class ProductViewModel
    {
        public IEnumerable<ProductDto> products { get; set; }
        public IEnumerable<Brand> brands { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
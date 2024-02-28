using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices
{
    public interface IIntegratorService
    {
        string GetTrendyolBrands(int page);
        string GetTrendyolCategories();
        string GetTrendyolProducts();
    }
}

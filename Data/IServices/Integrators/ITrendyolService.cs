using Data.Models.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.IServices.Integrators
{
    public interface ITrendyolService
    {
        Task<RootObject> getProducts();
        Task<string> deleteProduct(List<string> barcodes);
    }
}

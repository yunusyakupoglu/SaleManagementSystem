using Data.Models.api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.IServices.Integrators
{
    public interface ITrendyolService
    {
        Task<RootObject> getProducts(string parameter, string size, string SvcCredentials, string UserAgent, string merchantId);
        Task<ApiResponseModel> getBatchRequestResult(string batchRequestId, string SvcCredentials, string UserAgent, string merchantId);
        Task<ApiResponseModel> deleteProduct(List<string> barcodes, string SvcCredentials, string UserAgent, string merchantId);
        Task<ApiResponseModel> createProduct(PostRequest postRequest, string SvcCredentials, string UserAgent, string merchantId);
        Task<ApiResponseModel> getSuppliersAddresses(string SvcCredentials, string UserAgent, string merchantId);
        Task<ApiResponseModel> getBrands(string page, string size, string SvcCredentials, string UserAgent);
        Task<ApiResponseModel> getCategoryTree(string SvcCredentials, string UserAgent);
        Task<ApiResponseModel> getCategoryAttributes(string categoryId, string SvcCredentials, string UserAgent);
        Task<ApiResponseModel> updateProduct(PutRequest putRequest, string SvcCredentials, string UserAgent, string merchantId);
        Task<ApiResponseModel> updatePriceAndInventory(StockInventoryRequest stockRequest, string SvcCredentials, string UserAgent, string merchantId);
        Task<ApiResponseModel> getBrandByName(string brandName, string SvcCredentials, string UserAgent);
    }
}

using Data.IServices.Integrators;
using Data.Models.api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Integrators
{
    public class TrendyolService : ITrendyolService
    {
        public async Task<RootObject> getProducts(string parameter, string size, string SvcCredentials, string UserAgent, string merchantId)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/suppliers/{merchantId}/products?{parameter}&size={size}", Method.Get);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                // JSON string'ini RootObject nesnesine deserialize ediyoruz
                var rootObject = JsonConvert.DeserializeObject<RootObject>(response.Content);
                return rootObject;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task<ApiResponseModel> deleteProduct(List<string> barcodes, string SvcCredentials, string UserAgent, string merchantId)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/suppliers/{merchantId}/v2/products", Method.Delete);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);

            // Barcode'ları JSON formatında items listesine dönüştürme
            var items = barcodes.Select(barcode => new { barcode = barcode }).ToList();
            var body = JsonConvert.SerializeObject(new { items = items });

            // İsteğe JSON gövdesini ekleyin
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, Content: {response.Content}");
            }
        }

        public async Task<ApiResponseModel> createProduct(PostRequest postRequest, string SvcCredentials, string UserAgent, string merchantId)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/suppliers/{merchantId}/v2/products", Method.Post);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            var body = JsonConvert.SerializeObject(postRequest);

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // JSON string'ini RootObject nesnesine deserialize ediyoruz
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                return new ApiResponseModel { statusCode = System.Net.HttpStatusCode.OK, message = apiResponse.BatchRequestId };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                // response.Content'i ApiResponse nesnesine dönüştür
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                if (apiResponse != null && apiResponse.Errors != null && apiResponse.Errors.Length > 0)
                {
                    return new ApiResponseModel { statusCode = System.Net.HttpStatusCode.BadRequest, message = apiResponse.Errors[0].Message };
                }
            }
            throw new Exception($"Error: {response.StatusCode}, Content: {response.Content}");
        }

        public async Task<ApiResponseModel> getBatchRequestResult(string batchRequestId, string SvcCredentials, string UserAgent, string merchantId)
        {
            var options = new RestClientOptions("https://api.trendyol.com/")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"sapigw/suppliers/{merchantId}/products/batch-requests/{batchRequestId}", Method.Get);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
            else
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
        }

        #region (İade ve Sevkiyat Adres Bilgileri) createProduct V2 servisine yapılacak isteklerde gönderilecek sipariş ve sevkiyat kargo firma bilgileri bu servisten çekilecek.
        public async Task<ApiResponseModel> getSuppliersAddresses(string SvcCredentials, string UserAgent, string merchantId)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/suppliers/{merchantId}/addresses", Method.Get);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
            else
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
        }
        #endregion

        #region (Trendyol Marka Listesi) createProduct V2 servisine yapılacak isteklerde gönderilecek brandId bilgisi bu servis kullanılarak alınacaktır.
        public async Task<ApiResponseModel> getBrands(string page, string size, string SvcCredentials, string UserAgent)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/brands?page={page}&size={size}", Method.Get);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
            else
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
        }
        #endregion

        #region (Trendyol Kategori Listesi) createProduct V2 servisine yapılacak isteklerde gönderilecek categoryId bilgisi bu servis kullanılarak alınacaktır.
        public async Task<ApiResponseModel> getCategoryTree(string SvcCredentials, string UserAgent)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/product-categories", Method.Get);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
            else
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
        }
        #endregion

        #region (Trendyol Kategori - Özellik Listesi) createProduct V2 servisine yapılacak isteklerde gönderilecek attributes bilgileri ve bu bilgilere ait detaylar bu servis kullanılarak alınacaktır.
        public async Task<ApiResponseModel> getCategoryAttributes(string categoryId, string SvcCredentials, string UserAgent)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/product-categories/{categoryId}/attributes", Method.Get);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
            else
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
        }
        #endregion

        public async Task<ApiResponseModel> updateProduct(PutRequest putRequest, string SvcCredentials, string UserAgent, string merchantId)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/suppliers/{merchantId}/v2/products", Method.Put);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            var body = JsonConvert.SerializeObject(putRequest);

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // JSON string'ini RootObject nesnesine deserialize ediyoruz
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                return new ApiResponseModel { statusCode = System.Net.HttpStatusCode.OK, message = apiResponse.BatchRequestId };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                // response.Content'i ApiResponse nesnesine dönüştür
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                if (apiResponse != null && apiResponse.Errors != null && apiResponse.Errors.Length > 0)
                {
                    return new ApiResponseModel { statusCode = System.Net.HttpStatusCode.BadRequest, message = apiResponse.Errors[0].Message };
                }
            }
            throw new Exception($"Error: {response.StatusCode}, Content: {response.Content}");
        }

        public async Task<ApiResponseModel> updatePriceAndInventory(StockInventoryRequest stockRequest, string SvcCredentials, string UserAgent, string merchantId)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/suppliers/{merchantId}/products/price-and-inventory", Method.Post);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            var body = JsonConvert.SerializeObject(stockRequest);

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // JSON string'ini RootObject nesnesine deserialize ediyoruz
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                return new ApiResponseModel { statusCode = System.Net.HttpStatusCode.OK, message = apiResponse.BatchRequestId };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                // response.Content'i ApiResponse nesnesine dönüştür
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                if (apiResponse != null && apiResponse.Errors != null && apiResponse.Errors.Length > 0)
                {
                    return new ApiResponseModel { statusCode = System.Net.HttpStatusCode.BadRequest, message = apiResponse.Errors[0].Message };
                }
            }
            throw new Exception($"Error: {response.StatusCode}, Content: {response.Content}");
        }

        public async Task<ApiResponseModel> getBrandByName(string brandName, string SvcCredentials, string UserAgent)
        {
            var options = new RestClientOptions("https://api.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/sapigw/brands/by-name?name={brandName}", Method.Get);
            request.AddHeader("Authorization", "Basic " + SvcCredentials);
            request.AddHeader("User-Agent", UserAgent);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
            else
            {
                return new ApiResponseModel { statusCode = response.StatusCode, message = response.Content };
            }
        }
    }
}

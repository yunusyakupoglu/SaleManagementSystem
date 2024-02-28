using Data.IServices.Integrators;
using Data.Models.api;
using Newtonsoft.Json;
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


        public async Task<RootObject> getProducts()
        {
            var options = new RestClientOptions("https://stageapi.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/stagesapigw/suppliers/2738/products?page=0&size=5", Method.Get);
            request.AddHeader("Authorization", "Basic " + TrendyolSettings.svcCredentials);
            request.AddHeader("User-Agent", "2738 - SelfIntegration");
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

        public async Task<string> deleteProduct(List<string> barcodes)
        {
            var options = new RestClientOptions("https://stageapi.trendyol.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/stagesapigw/suppliers/2738/v2/products", Method.Delete);
            request.AddHeader("Authorization", "Basic " + TrendyolSettings.svcCredentials);
            request.AddHeader("User-Agent", "2738 - SelfIntegration");

            // Barcode'ları JSON formatında items listesine dönüştürme
            var items = barcodes.Select(barcode => new { barcode = barcode }).ToList();
            var body = JsonConvert.SerializeObject(new { items = items });

            // İsteğe JSON gövdesini ekleyin
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                // JSON string'ini RootObject nesnesine deserialize ediyoruz
                var rootObject = JsonConvert.DeserializeObject<RootObject>(response.Content);
                return rootObject.Content[0].BatchRequestId; // Dikkat: Dönen JSON objesindeki gerçek anahtar ismine göre ayarlayın
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, Content: {response.Content}");
            }
        }
    }
}

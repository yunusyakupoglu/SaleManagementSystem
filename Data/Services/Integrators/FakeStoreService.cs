using Data.IServices.Integrators;
using Data.Models.api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Integrators
{
    public class FakeStoreService : IFakeStoreService
    {
        public string AddProduct(ApiProduct apiProduct)
        {
            var client = new RestClient("https://fakestoreapi.com/");
            var request = new RestRequest("products",Method.Post);
            request.AddBody(apiProduct);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.StatusDescription;
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }

        public string GetProducts()
        {
            var client = new RestClient("https://fakestoreapi.com/");
            var request = new RestRequest("products",Method.Get);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
    }
}

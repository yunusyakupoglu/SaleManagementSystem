using Data.IServices;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class IntegratorService : IIntegratorService
    {
        public string GetTrendyolBrands(int page)
        {
            var client = new RestClient("https://api.trendyol.com/sapigw/");
            var request = new RestRequest("brands", Method.Get);

            // API anahtarınızı buraya ekleyin
            request.AddHeader("Authorization", "Bearer QQN6re5UcQVjLQnNpK8R");
            request.AddParameter("page", page);

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


        public string GetTrendyolCategories()
        {
            var client = new RestClient("https://api.trendyol.com/sapigw/");

            var request = new RestRequest("product-categories", Method.Get);
            request.AddHeader("Authorization", "Bearer QQN6re5UcQVjLQnNpK8R");
            request.AddHeader("Accept", "application/json");

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

        public string GetTrendyolProducts()
        {
            var client = new RestClient("https://api.trendyol.com/sapigw/");
            var request = new RestRequest("products", Method.Get);

            // API anahtarınızı buraya ekleyin
            request.AddHeader("Authorization", "Basic QQN6re5UcQVjLQnNpK8R");
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

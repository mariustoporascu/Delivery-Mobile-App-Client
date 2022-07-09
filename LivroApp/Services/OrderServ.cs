using LivroApp.Constants;
using LivroApp.Models.ShopModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LivroApp.Services
{
    public class OrderServ : IOrderServ
    {
        private HttpClient _httpClient;

        public OrderServ()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> AgreeEstTime(int orderId, bool accept)
        {
            ServerConstants.TryAddHeaders(_httpClient);
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/agreeesttime/{orderId}&{accept}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine(respInfo);
                if (respInfo.Contains("agreed :"))
                    return true;

            }
            return false;
        }

        public async Task<string> CreateOrder(ServerOrder order)
        {
            ServerConstants.TryAddHeaders(_httpClient);
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorder");
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                return respInfo;
            }
            return string.Empty;
        }

        public async Task CreateOrderInfo(OrderInfo orderInfo)
        {
            ServerConstants.TryAddHeaders(_httpClient);
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorderinfo");
            var json = JsonConvert.SerializeObject(orderInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            Debug.WriteLine(httpResponseMessage.IsSuccessStatusCode);
        }

        public async Task CreateProductsInOrder(List<ProductInOrder> productsInOrder)
        {
            ServerConstants.TryAddHeaders(_httpClient);
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorderproducts");
            var json = JsonConvert.SerializeObject(productsInOrder);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            Debug.WriteLine(httpResponseMessage.IsSuccessStatusCode);
        }

        public async Task<bool> GiveRatingDriver(string email, int orderId, int rating)
        {
            ServerConstants.TryAddHeaders(_httpClient);
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/ratingdriver/{email}&{orderId}&{rating}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {

                return true;
            }
            return false;
        }

        public async Task<bool> GiveRatingRestaurant(string email, int orderId, int rating)
        {
            ServerConstants.TryAddHeaders(_httpClient);
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/ratingrestaurant/{email}&{orderId}&{rating}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {

                return true;
            }
            return false;
        }

        public async Task<DriverLocation> LoadDrivers(string driverId, int orderId)
        {
            ServerConstants.TryAddHeaders(_httpClient);
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getmydriverlocation/{driverId}&{orderId}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = await httpResponseMessage.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                return JsonConvert.DeserializeObject<DriverLocation>(content, settings);
            }
            return null;
        }
    }
}

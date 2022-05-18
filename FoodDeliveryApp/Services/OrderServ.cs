using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public class OrderServ : IOrderServ
    {
        private HttpClient _httpClient = new HttpClient();

        public async Task<bool> AgreeEstTime(int orderId, bool accept)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/agreeesttime/{orderId}/{accept}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine(respInfo);
                if (respInfo.Contains("agreed :"))
                    return true;

            }
            return false;
        }

        public async Task<int> CreateOrder(Order order)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorder");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                return int.Parse(respInfo);
            }
            return 0;
        }

        public async Task CreateOrderInfo(OrderInfo orderInfo)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorderinfo");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonConvert.SerializeObject(orderInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            Debug.WriteLine(httpResponseMessage.IsSuccessStatusCode);
        }

        public async Task CreateProductsInOrder(List<ProductInOrder> productsInOrder)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorderproducts");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonConvert.SerializeObject(productsInOrder);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            Debug.WriteLine(httpResponseMessage.IsSuccessStatusCode);
        }

        public async Task<bool> GiveRatingDriver(string email, int orderId, int rating)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/ratingdriver/{email}/{orderId}/{rating}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {

                return true;
            }
            return false;
        }

        public async Task<bool> GiveRatingRestaurant(string email, int orderId, int rating)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/ratingrestaurant/{email}/{orderId}/{rating}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {

                return true;
            }
            return false;
        }

        public async Task<DriverLocation> LoadDrivers(string driverId, int orderId)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/getmydriverlocation/{driverId}/{orderId}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
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

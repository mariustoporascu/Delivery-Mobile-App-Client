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
        private HttpClient _httpClient;

        public OrderServ()
        {
            _httpClient = new HttpClient();
        }
        private void TryAddHeaders()
        {
            try
            {
                bool authkey = _httpClient.DefaultRequestHeaders.TryGetValues("authkey", out var val);
                bool authid = _httpClient.DefaultRequestHeaders.TryGetValues("authid", out var val2);
                if (!authid && !authkey)
                {
                    _httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    _httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Remove("authkey");
                    _httpClient.DefaultRequestHeaders.Remove("authid");
                    _httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    _httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public async Task<bool> AgreeEstTime(int orderId, bool accept)
        {
            TryAddHeaders();
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
            TryAddHeaders();
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
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorderinfo");
            var json = JsonConvert.SerializeObject(orderInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            Debug.WriteLine(httpResponseMessage.IsSuccessStatusCode);
        }

        public async Task CreateProductsInOrder(List<ProductInOrder> productsInOrder)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorderproducts");
            var json = JsonConvert.SerializeObject(productsInOrder);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            Debug.WriteLine(httpResponseMessage.IsSuccessStatusCode);
        }

        public async Task<bool> GiveRatingDriver(string email, int orderId, int rating)
        {
            TryAddHeaders();

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
            TryAddHeaders();

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
            TryAddHeaders();

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

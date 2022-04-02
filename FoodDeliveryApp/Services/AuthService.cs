using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.AuthModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public class AuthService : IAuthController
    {


        public AuthService()
        {

        }
        public async Task<string> CreateUser(UserModel userModel)
        {
            Uri uri = new Uri("https://fooddelivapp.somee.com/auth/create");
            return await sendRequest(userModel, uri);

        }

        public async Task<string> LoginUser(UserModel userModel)
        {
            Uri uri = new Uri("https://fooddelivapp.somee.com/auth/login");
            return await sendRequest(userModel, uri);

        }

        public async Task<string> UserProfile(UserModel userModel)
        {
            Uri uri = new Uri("https://fooddelivapp.somee.com/auth/profile");
            return await sendRequest(userModel, uri);

        }

        private async Task<string> sendRequest(UserModel userModel, Uri uri)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                return respInfo;
            }
            return string.Empty;
        }
    }
}

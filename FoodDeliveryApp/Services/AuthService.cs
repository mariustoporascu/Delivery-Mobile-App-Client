using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.AuthModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public class AuthService : IAuthController
    {
        private HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<string> CreateUser(UserModel userModel)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/auth/create");
            return await sendRequest(userModel, uri);

        }

        public async Task<string> LoginUser(UserModel userModel)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/auth/login");
            return await sendRequest(userModel, uri);

        }

        public async Task<string> UserProfile(UserModel userModel)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/auth/profile");
            return await sendRequest(userModel, uri);

        }
        public async Task<string> DeleteProfile(UserModel userModel)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/auth/delete");
            return await sendRequest(userModel, uri);

        }
        private void TryAddHeaders()
        {
            try
            {
                bool authkey = _httpClient.DefaultRequestHeaders.TryGetValues("authkey", out var val);
                bool authid = _httpClient.DefaultRequestHeaders.TryGetValues("authid", out var val2);
                if (!authid && !authkey)
                {
                    _httpClient.DefaultRequestHeaders.Add("authkey", App.userInfo.LoginToken);
                    _httpClient.DefaultRequestHeaders.Add("authid", App.userInfo.Email);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        private async Task<string> sendRequest(UserModel userModel, Uri uri)
        {
            if (uri.AbsoluteUri.Contains("profile") || uri.AbsoluteUri.Contains("delete"))
                TryAddHeaders();
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

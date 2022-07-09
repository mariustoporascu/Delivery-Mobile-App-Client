using LivroApp.Constants;
using LivroApp.Models.AuthModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LivroApp.Services
{
    public class AuthService : IAuthController
    {
        private HttpClient _httpClient;
        private static string[] endPoint = { "delete","login","create","profile","userlocation","setpassword","changepassword",
        "resetpassword","sendtokenpassword","confirmemail","deletelocation"};

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> Execute(UserModel userModel, AuthOperations operation)
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/auth/{endPoint[(int)operation]}");

            ServerConstants.TryAddHeaders(_httpClient);
            return await SendRequest(userModel, uri);
        }
        private async Task<string> SendRequest(UserModel userModel, Uri uri)
        {

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

        public async Task<bool> FbLoginEnabled()
        {
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/fbbtnios");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                return respInfo.Contains("true");
            }
            return false;
        }
    }
}

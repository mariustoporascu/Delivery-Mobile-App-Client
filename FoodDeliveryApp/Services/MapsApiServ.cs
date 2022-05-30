using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.MapsModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Services
{
    public class MapsApiServ
    {

        private static MapsApiServ _ServiceClientInstance;

        public static MapsApiServ ServiceClientInstance
        {
            get
            {
                if (_ServiceClientInstance == null)
                    _ServiceClientInstance = new MapsApiServ();
                return _ServiceClientInstance;
            }
        }
        private HttpClient client;
        public MapsApiServ()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ServerConstants.BaseUrl);
        }
        private void TryAddHeaders()
        {
            try
            {
                bool authkey = client.DefaultRequestHeaders.TryGetValues("authkey", out var val);
                bool authid = client.DefaultRequestHeaders.TryGetValues("authid", out var val2);
                if (!authid && !authkey)
                {
                    client.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    client.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }
                else
                {
                    client.DefaultRequestHeaders.Remove("authkey");
                    client.DefaultRequestHeaders.Remove("authid");
                    client.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    client.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public async Task<GoogleDirection> GetDirections(Position position1, Position position2)
        {
            TryAddHeaders();
            GoogleDirection googleDirection = new GoogleDirection();
            var response = await client.GetAsync("api/getdirections/getroute/" +
                 position2.Latitude.ToString("N7", CultureInfo.InvariantCulture) + "&" +
                 position2.Longitude.ToString("N7", CultureInfo.InvariantCulture) + "&" +
                 position1.Latitude.ToString("N7", CultureInfo.InvariantCulture) + "&" +
                 position1.Longitude.ToString("N7", CultureInfo.InvariantCulture));
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    googleDirection = await Task.Run(() =>
                       JsonConvert.DeserializeObject<GoogleDirection>(json)
                    );

                }

            }

            return googleDirection;
        }
    }
}

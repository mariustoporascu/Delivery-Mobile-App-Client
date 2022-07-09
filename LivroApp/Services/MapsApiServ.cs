using LivroApp.Constants;
using LivroApp.Models.MapsModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace LivroApp.Services
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
        public async Task<GoogleDirection> GetDirections(Position position1, Position position2)
        {
            ServerConstants.TryAddHeaders(client);
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

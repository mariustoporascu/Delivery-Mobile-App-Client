using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.MapsModels;
using Newtonsoft.Json;
using System;
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
            client.BaseAddress = new Uri("https://maps.googleapis.com/maps/");
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
        }

        public async Task<GoogleDirection> GetDirections(Position position1, Position position2)
        {
            GoogleDirection googleDirection = new GoogleDirection();
            var response = await client.GetAsync("api/directions/json?mode=driving&transit_routing_preference=less_driving&origin=" +
                 position2.Latitude.ToString("N7", CultureInfo.InvariantCulture) + "," +
                 position2.Longitude.ToString("N7", CultureInfo.InvariantCulture) + "&destination=" +
                 position1.Latitude.ToString("N7", CultureInfo.InvariantCulture) + "," +
                 position1.Longitude.ToString("N7", CultureInfo.InvariantCulture) +
                "&language=ro&region=RO&key=" + GoogleConstants.GeoApiKey).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    googleDirection = await Task.Run(() =>
                       JsonConvert.DeserializeObject<GoogleDirection>(json)
                    ).ConfigureAwait(false);

                }

            }

            return googleDirection;
        }
    }
}

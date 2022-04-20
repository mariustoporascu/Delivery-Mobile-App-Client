using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.MapsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Services
{
    public class MapsApiServ
    {
        private JsonSerializer _serializer = new JsonSerializer();

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
        }

        public async Task<GoogleDirection> GetDirections(Position position1, Position position2)
        {
            GoogleDirection googleDirection = new GoogleDirection();

            var response = await client.GetAsync($"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={position1.Latitude},{position1.Longitude}&destination={position2.Latitude},{position2.Longitude}&key={GoogleConstants.GeoApiKey}").ConfigureAwait(false);
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

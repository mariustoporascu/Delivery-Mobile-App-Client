using FoodDeliveryApp.Models.MapsModels;
using FoodDeliveryApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.ViewModels
{
    public class MapsViewModel
    {
        Geocoder geoCoder;
        public Pin pinRoute1 = new Pin
        {
            Label = "My location"
        };
        public MapsViewModel()
        {
            geoCoder = new Geocoder();
            //AppMap.Pins.Add(pinRoute1);
            //AppMap.Pins.Add(pinRoute2);

        }

        public async Task LoadMyLocation()
        {
            if (App.userInfo != null)
            {
                IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync(App.userInfo?.Street + ", " + App.userInfo.City + ", Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    pinRoute1.Position = position1;
                }
            }
        }
        internal async Task<List<Position>> LoadRoute(Pin pin)
        {
            if (App.userInfo != null)
            {
                var googleDirection = await MapsApiServ.ServiceClientInstance.GetDirections(pinRoute1.Position, pin.Position);
                if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
                {
                    var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                    return positions;
                }
                return null;

            }
            return null;
        }
    }
}

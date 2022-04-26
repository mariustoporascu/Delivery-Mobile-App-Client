using FoodDeliveryApp.Models.MapsModels;
using FoodDeliveryApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.ViewModels
{
    public class MapsViewModel
    {
        public Geocoder geoCoder;
        public Pin pinRoute1 = new Pin
        {
            Label = "Adresa mea"
        };
        public MapsViewModel()
        {
            geoCoder = new Geocoder();
        }

        public async Task LoadMyLocation()
        {
            if (App.isLoggedIn)
            {
                IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync(App.userInfo.Street + ", " + App.userInfo.City + ", Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    pinRoute1.Position = position1;
                }
            }
            else
            {
                IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync("Cernavoda, Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    pinRoute1.Position = position1;
                }
            }
        }
        internal async Task<GoogleDirection> LoadRoute(Pin pin)
        {
            if (App.isLoggedIn)
            {
                var googleDirection = await MapsApiServ.ServiceClientInstance.GetDirections(pinRoute1.Position, pin.Position);
                if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
                {
                    return googleDirection;
                }
                return null;

            }
            return null;
        }
    }
}

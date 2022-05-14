using FoodDeliveryApp.Models.MapsModels;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        public Geocoder geoCoder;
        public Pin pinRoute1 = new Pin
        {
            Label = "Adresa mea"
        };
        private List<DriverLocation> _driverLocations;

        public MapsViewModel()
        {
            geoCoder = new Geocoder();
            _driverLocations = new List<DriverLocation>();
        }

        public async Task LoadMyLocation()
        {
            if (App.isLoggedIn && App.userInfo.CompleteProfile)
            {
                Position myPosition = new Position(App.userInfo.CoordX, App.userInfo.CoordY);
                pinRoute1.Position = myPosition;

            }
            else
            {
                IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync("Centru, Cernavoda, Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    pinRoute1.Position = position1;
                }
            }
        }
        public async Task<Dictionary<int, GoogleDirection>> DrawDriverRoute()
        {
            if (App.isLoggedIn && App.userInfo.CompleteProfile)
            {
                _driverLocations.Clear();
                var myOrders = await DataStore.GetServerOrders(App.userInfo.Email).ConfigureAwait(false);
                foreach (var o in myOrders)
                {
                    if (o.Status == "In curs de livrare")
                    {
                        var driverloc = await OrderService.LoadDrivers(o.DriverRefId, o.OrderId);
                        if (driverloc != null)
                        {
                            _driverLocations.Add(driverloc);
                        }
                    }
                }
            }
            Dictionary<int, GoogleDirection> directions = new Dictionary<int, GoogleDirection>();
            foreach (var o in _driverLocations)
            {
                var route = await LoadRoute(new Position(o.CoordX, o.CoordY));
                if (route != null)
                    directions.Add(o.OrderId, route);
            }
            return directions;
        }
        private async Task<GoogleDirection> LoadRoute(Position pin)
        {
            if (App.isLoggedIn)
            {
                var googleDirection = await MapsApiServ.ServiceClientInstance.GetDirections(pinRoute1.Position, pin);
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

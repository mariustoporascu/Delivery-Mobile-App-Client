﻿using LivroApp.Controls;
using LivroApp.Models.MapsModels;
using LivroApp.Models.ShopModels;
using LivroApp.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace LivroApp.ViewModels
{
    public class MapsViewModel : BaseViewModel<StatsModel>
    {
        public Geocoder GeoCoder;
        public List<CustomPin> MyLocations { get; set; }
        private List<DriverLocation> _driverLocations;
        private bool _hasRoute = false;
        public bool HasRoute { get { return _hasRoute; } set => SetProperty(ref _hasRoute, value); }
        public MapsViewModel()
        {
            GeoCoder = new Geocoder();
            _driverLocations = new List<DriverLocation>();
            Items = new ObservableRangeCollection<StatsModel>();
            MyLocations = new List<CustomPin>();
        }

        public async Task LoadMyLocation()
        {
            if (App.IsLoggedIn && App.UserInfo.Locations != null)
            {
                MyLocations.Clear();
                foreach (var loc in App.UserInfo.Locations)
                {
                    Position myPosition = new Position(loc.CoordX, loc.CoordY);
                    var pin = new CustomPin
                    {
                        Label = loc.LocationName,
                        Type = PinType.Place,
                        LocationId = loc.LocationId,
                        Position = myPosition
                    };
                    MyLocations.Add(pin);
                }

            }
            else
            {
                IEnumerable<Position> aproxLocation = await GeoCoder.GetPositionsForAddressAsync("Dacia, Cernavoda, Constanta, Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    var pin = new CustomPin
                    {
                        Label = "Cernavoda",
                        Type = PinType.Place,
                        Position = position1
                    };
                    MyLocations.Add(pin);
                }
            }
        }
        public async Task<Dictionary<int, GoogleDirection>> DrawDriverRoute()
        {
            try
            {
                if (App.IsLoggedIn && App.UserInfo.Locations != null)
                {
                    _driverLocations.Clear();
                    var myOrders = await DataStore.GetServerOrders(App.UserInfo.Email);
                    foreach (var o in myOrders)
                    {
                        if (o.Status == "In curs de livrare")
                        {
                            var driverloc = await OrderService.LoadDrivers(o.DriverRefId, o.OrderId);
                            if (driverloc != null)
                            {
                                driverloc.OrderId = o.OrderId;
                                _driverLocations.Add(driverloc);
                            }
                        }
                    }
                }
                Dictionary<int, GoogleDirection> directions = new Dictionary<int, GoogleDirection>();
                foreach (var o in _driverLocations)
                {
                    var order = DataStore.GetOrder(o.OrderId);
                    var locationInOrder = App.UserInfo.Locations.Find(loc => loc.LocationId == order.UserLocationId);
                    var route = await LoadRoute(new Position(locationInOrder.CoordX, locationInOrder.CoordY),
                        new Position(o.CoordX, o.CoordY));
                    if (route != null)
                        directions.Add(o.OrderId, route);
                }
                return directions;

            }
            catch (Exception) { return null; }

        }
        private async Task<GoogleDirection> LoadRoute(Position customer, Position driver)
        {
            if (App.IsLoggedIn)
            {
                try
                {
                    var googleDirection = await MapsApiServ.ServiceClientInstance.GetDirections(customer, driver);
                    if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
                    {
                        HasRoute = true;
                        return googleDirection;
                    }
                }
                catch (Exception) { }
            }
            HasRoute = false;
            return null;
        }
    }
}

using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroApp.ViewModels.UserVModels
{
    public class UserLocationViewModel : BaseViewModel<BaseModel>
    {
        private string _city = string.Empty;
        private string _buildinginfo = string.Empty;
        private string _street = string.Empty;
        private double _coordX;
        private double _coordY;
        private string _name = string.Empty;
        public List<string> AvailableCities { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get => _name; set => SetProperty(ref _name, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public double CoordX { get => _coordX; set => SetProperty(ref _coordX, value); }
        public double CoordY { get => _coordY; set => SetProperty(ref _coordY, value); }
        public UserLocation LocationReference { get; set; }
        public Command SaveLocation { get; }
        public UserLocationViewModel(int locationId)
        {
            AvailableCities = new List<string>();
            AvailableCities.AddRange(DataStore.GetAvailableCities().ToList().Select(city => city.Name));
            if (locationId > 0)
            {
                LocationId = locationId;
                var location = App.UserInfo.Locations.Find(loc => loc.LocationId == locationId);
                LocationName = location.LocationName;
                City = location.City;
                BuildingInfo = location.BuildingInfo;
                Street = location.Street;
                CoordX = location.CoordX;
                CoordY = location.CoordY;
                LocationReference = location;
            }
            SaveLocation = new Command(async () => await OnSaveLocation());

        }
        async Task OnSaveLocation()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Location = new UserLocation
                    {
                        LocationId = LocationId,
                        LocationName = LocationName,
                        City = City,
                        BuildingInfo = BuildingInfo,
                        Street = Street,
                        CoordX = CoordX,
                        CoordY = CoordY,
                    },
                    Email = App.UserInfo.Email,
                    UserIdentification = App.UserInfo.UserIdentification,
                    Password = App.UserInfo.Password,
                }, AuthOperations.Location);
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Location updated."))
                {
                    if (App.UserInfo.Locations == null)
                        App.UserInfo.Locations = new List<UserLocation>();
                    var locationId = int.Parse(result.Split(':')[0]);
                    var location = App.UserInfo.Locations.Find(loc => loc.LocationId == locationId);
                    if (location == null)
                        App.UserInfo.Locations.Add(new UserLocation
                        {
                            LocationId = locationId,
                            LocationName = LocationName,
                            City = City,
                            BuildingInfo = BuildingInfo,
                            Street = Street,
                            CoordX = CoordX,
                            CoordY = CoordY,
                        });
                    else
                    {
                        location.LocationId = locationId;
                        location.LocationName = LocationName;
                        location.City = City;
                        location.BuildingInfo = BuildingInfo;
                        location.Street = Street;
                        location.CoordX = CoordX;
                        location.CoordY = CoordY;
                    }
                    CallSuccessEvent();
                }
                else
                    CallFailedEvent();
            }
            catch (Exception) { CallFailedEvent(); }
            finally { IsBusy = false; }
        }
    }
}

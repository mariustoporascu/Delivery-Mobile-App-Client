using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class UserLocationViewModel : BaseViewModel
    {
        private string _city = string.Empty;
        private string _buildinginfo = string.Empty;
        private string _street = string.Empty;
        private double _coordX;
        private double _coordY;

        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public double CoordX { get => _coordX; set => SetProperty(ref _coordX, value); }
        public double CoordY { get => _coordY; set => SetProperty(ref _coordY, value); }

        public Command SaveLocation { get; }

        public event EventHandler OnUpdateLocation = delegate { };
        public event EventHandler UpdateLocationFailed = delegate { };


        public UserLocationViewModel()
        {
            SaveLocation = new Command(async () => await OnSaveLocation());
        }
        async Task OnSaveLocation()
        {
            var result = await AuthController.Execute(new UserModel
            {
                Location = new UserLocation
                {
                    City = City,
                    BuildingInfo = BuildingInfo,
                    Street = Street,
                    CoordX = CoordX,
                    CoordY = CoordY,
                },

                Email = App.UserInfo.Email,
                UserIdentification = App.UserInfo.UserIdentification,
                Password = App.UserInfo.Password,
            }, Constants.AuthOperations.Location);
            if (!string.IsNullOrWhiteSpace(result) && result.Contains("Location updated."))
            {
                App.UserInfo.Location = new UserLocation();
                App.UserInfo.Location.BuildingInfo = BuildingInfo;
                App.UserInfo.Location.City = City;
                App.UserInfo.Location.Street = Street;
                App.UserInfo.Location.CoordX = CoordX;
                App.UserInfo.Location.CoordY = CoordY;

                OnUpdateLocation?.Invoke(this, new EventArgs());
            }
            else
            {
                UpdateLocationFailed?.Invoke(this, new EventArgs());
            }
        }
    }
}

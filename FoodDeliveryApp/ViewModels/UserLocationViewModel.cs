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
                City = City,
                BuildingInfo = BuildingInfo,
                Street = Street,
                CoordX = CoordX,
                CoordY = CoordY,
                Email = App.userInfo.Email,
                UserIdentification = App.userInfo.UserIdentification,
                Password = App.userInfo.Password,
                CompleteLocation = true
            }, Constants.AuthOperations.Location);
            if (!string.IsNullOrWhiteSpace(result) && result.Contains("Location updated."))
            {
                App.userInfo.BuildingInfo = BuildingInfo;
                App.userInfo.City = City;
                App.userInfo.Street = Street;
                App.userInfo.CoordX = CoordX;
                App.userInfo.CoordY = CoordY;
                App.userInfo.CompleteLocation = true;
                OnUpdateLocation?.Invoke(this, new EventArgs());
            }
            else
            {
                UpdateLocationFailed?.Invoke(this, new EventArgs());
            }
        }
    }
}

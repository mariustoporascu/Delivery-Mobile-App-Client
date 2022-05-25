using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        private string _fullName = string.Empty;
        private string _city = string.Empty;
        private string _buildinginfo = string.Empty;
        private string _street = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _email = string.Empty;
        private string _header = string.Empty;
        private double _coordX;
        private double _coordY;
        private bool isLoggedIn = false;

        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Header { get => _header; set => SetProperty(ref _header, value); }
        public double CoordX { get => _coordX; set => SetProperty(ref _coordX, value); }
        public double CoordY { get => _coordY; set => SetProperty(ref _coordY, value); }

        public Command SaveProfile { get; }
        public Command DeleteProfile { get; }
        public event EventHandler OnUpdateProfile = delegate { };
        public event EventHandler OnDeleteAcc = delegate { };

        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }

        public Command Logout { get; }

        public UserProfileViewModel()
        {
            RefreshProfile();

            SaveProfile = new Command(async () => await OnSaveProfile());
            DeleteProfile = new Command(async () => await OnDeleteProfile());
            Logout = new Command(LogOutFunct);

            MessagingCenter.Subscribe<LoginViewModel>(this, "UpdateProfile", (sender) =>
               {
                   RefreshProfile();
               });
            MessagingCenter.Subscribe<RegisterViewModel>(this, "UpdateProfile", (sender) =>
            {
                RefreshProfile();
            });
        }
        void LogOutFunct()
        {
            App.userInfo = new UserModel();
            App.isLoggedIn = false;
            SecureStorage.RemoveAll();
            IsLoggedIn = false;
            Header = string.Empty;
            FullName = string.Empty;
            BuildingInfo = string.Empty;
            Street = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            City = string.Empty;
            CoordX = 0.0;
            CoordY = 0.0;
        }

        public void RefreshProfile()
        {
            IsLoggedIn = App.isLoggedIn;
            Header = "Bun venit " + App.userInfo.FullName;
            FullName = App.userInfo.FullName;
            BuildingInfo = App.userInfo.BuildingInfo;
            Street = App.userInfo.Street;
            Email = App.userInfo.Email;
            PhoneNumber = App.userInfo.PhoneNumber;
            City = App.userInfo.City;
            CoordX = App.userInfo.CoordX;
            CoordY = App.userInfo.CoordY;
        }
        async Task OnSaveProfile()
        {
            var result = await AuthController.UserProfile(new UserModel
            {
                FullName = FullName,
                City = City,
                BuildingInfo = BuildingInfo,
                Street = Street,
                PhoneNumber = PhoneNumber,
                Email = Email,
                CoordX = CoordX,
                CoordY = CoordY,
                UserIdentification = App.userInfo.UserIdentification,
                Password = App.userInfo.Password,
                CompleteProfile = true
            });
            if (!result.Contains("Data invalid") || !result.Contains("Email is wrong or user not existing."))
            {
                App.userInfo.FullName = FullName;
                App.userInfo.BuildingInfo = BuildingInfo;
                App.userInfo.City = City;
                App.userInfo.PhoneNumber = PhoneNumber;
                App.userInfo.Street = Street;
                App.userInfo.CoordX = CoordX;
                App.userInfo.CoordY = CoordY;
                App.userInfo.CompleteProfile = true;
                RefreshProfile();
                OnUpdateProfile(this, new EventArgs());
            }
        }
        async Task OnDeleteProfile()
        {
            var result = await AuthController.DeleteProfile(new UserModel
            {
                Email = Email,
                UserIdentification = App.userInfo.UserIdentification,
                Password = App.userInfo.Password,
                CompleteProfile = true
            });
            if (!result.Contains("Email is wrong or user not existing."))
            {
                App.userInfo = new UserModel();
                App.isLoggedIn = false;
                SecureStorage.RemoveAll();
                IsLoggedIn = false;
                Header = string.Empty;
                FullName = string.Empty;
                BuildingInfo = string.Empty;
                Street = string.Empty;
                Email = string.Empty;
                PhoneNumber = string.Empty;
                City = string.Empty;
                CoordX = 0.0;
                CoordY = 0.0;
                OnDeleteAcc(this, new EventArgs());
            }
        }
    }
}

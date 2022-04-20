using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private string _fullName = string.Empty;
        private string _city = string.Empty;
        private string _buildinginfo = string.Empty;
        private string _street = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _email = string.Empty;
        private bool isLoggedIn = false;

        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public Command SaveProfile { get; }
        public event EventHandler OnUpdateProfile = delegate { };

        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public Command Logout { get; }

        public ProfileViewModel()
        {
            RefreshProfile();

            SaveProfile = new Command(OnSaveProfile);
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
            App.userInfo = null;
            App.isLoggedIn = false;
            SecureStorage.RemoveAll();
            IsLoggedIn = false;

        }

        public void RefreshProfile()
        {
            IsLoggedIn = App.isLoggedIn;
            Title = "Bun venit " + App.userInfo?.FullName;
            FullName = App.userInfo?.FullName;
            BuildingInfo = App.userInfo?.BuildingInfo;
            Street = App.userInfo?.Street;
            Email = App.userInfo?.Email;
            PhoneNumber = App.userInfo?.PhoneNumber;
            City = App.userInfo?.City;
        }
        async void OnSaveProfile()
        {
            var result = await AuthController.UserProfile(new UserModel
            {
                FullName = FullName,
                City = City,
                BuildingInfo = BuildingInfo,
                Street = Street,
                PhoneNumber = PhoneNumber,
                Email = Email,
                UserIdentification = App.userInfo.UserIdentification,
                Password = App.userInfo.Password
            });
            if (!result.Contains("Data invalid") || !result.Contains("Email is wrong or user not existing."))
            {
                App.userInfo.FullName = FullName;
                App.userInfo.BuildingInfo = BuildingInfo;
                App.userInfo.City = City;
                App.userInfo.PhoneNumber = PhoneNumber;
                App.userInfo.Street = Street;
                RefreshProfile();
                OnUpdateProfile(this, default(EventArgs));
            }
        }
    }
}

using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class UserDetailsViewModel : BaseViewModel
    {
        private string _fullName = string.Empty;
        private string _phoneNumber = string.Empty;

        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public Command SaveProfile { get; }
        public event EventHandler OnUpdateProfile = delegate { };
        public event EventHandler UpdateProfileFailed = delegate { };

        public UserDetailsViewModel()
        {
            //FullName = App.userInfo.FullName;
            //PhoneNumber = App.userInfo.PhoneNumber;
            SaveProfile = new Command(async () => await OnSaveProfile());
        }
        async Task OnSaveProfile()
        {
            var result = await AuthController.Execute(new UserModel
            {
                Email = App.userInfo.Email,
                Password = App.userInfo.Password,
                UserIdentification = App.userInfo.UserIdentification,
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                CompleteProfile = true,
            }, Constants.AuthOperations.Profile);
            if (!string.IsNullOrWhiteSpace(result) && result.Contains("Profile updated."))
            {
                App.userInfo.FullName = FullName;
                App.userInfo.PhoneNumber = PhoneNumber;
                App.userInfo.CompleteProfile = true;
                OnUpdateProfile?.Invoke(this, new EventArgs());
            }
            else
            {
                UpdateProfileFailed?.Invoke(this, new EventArgs());
            }
        }
    }
}

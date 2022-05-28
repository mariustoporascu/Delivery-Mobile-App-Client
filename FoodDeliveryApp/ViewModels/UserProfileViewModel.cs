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
        private string _phoneNumber = string.Empty;
        private string _email = string.Empty;
        private string _header = string.Empty;
        private bool isLoggedIn = false;
        private bool hasPasswordSet = false;

        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Header { get => _header; set => SetProperty(ref _header, value); }
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public bool HasPasswordSet { get => hasPasswordSet; set => SetProperty(ref hasPasswordSet, value); }
        public Command Logout { get; }

        public Command DeleteProfile { get; }

        public event EventHandler OnDeleteAcc = delegate { };
        public event EventHandler DeleteAccFailed = delegate { };

        public UserProfileViewModel()
        {
            RefreshProfile();

            DeleteProfile = new Command(async () => await OnDeleteProfile());
            Logout = new Command(LogOutFunct);

        }
        void LogOutFunct()
        {
            App.userInfo = new UserModel();
            App.isLoggedIn = false;
            SecureStorage.RemoveAll();
            IsLoggedIn = false;
            Header = string.Empty;
            FullName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
        }

        public void RefreshProfile()
        {
            IsLoggedIn = App.isLoggedIn;
            Header = "Bun venit " + App.userInfo.FullName;
            FullName = App.userInfo.FullName;
            Email = App.userInfo.Email;
            PhoneNumber = App.userInfo.PhoneNumber;
            HasPasswordSet = App.userInfo.HasSetPassword;
        }

        async Task OnDeleteProfile()
        {
            var result = await AuthController.Execute(new UserModel
            {
                Email = Email,
                UserIdentification = App.userInfo.UserIdentification,
                Password = App.userInfo.Password,
            }, Constants.AuthOperations.Delete);
            if (!result.Contains("Email is wrong or user not existing.") && !result.Contains("result : False"))
            {
                App.userInfo = new UserModel();
                App.isLoggedIn = false;
                SecureStorage.RemoveAll();
                IsLoggedIn = false;
                Header = string.Empty;
                FullName = string.Empty;
                Email = string.Empty;
                PhoneNumber = string.Empty;
                OnDeleteAcc?.Invoke(this, new EventArgs());
            }
            else
            {
                DeleteAccFailed?.Invoke(this, new EventArgs());
            }
        }
    }
}

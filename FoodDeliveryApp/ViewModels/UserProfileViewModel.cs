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
        private bool canChangePass = false;
        private bool _canEditLocation = false;


        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Header { get => _header; set => SetProperty(ref _header, value); }
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public bool HasPasswordSet { get => hasPasswordSet; set => SetProperty(ref hasPasswordSet, value); }
        public bool CanChangePass { get => canChangePass; set => SetProperty(ref canChangePass, value); }
        public bool CanEditLocation { get => _canEditLocation; set => SetProperty(ref _canEditLocation, value); }

        public Command Logout { get; }

        public Command DeleteProfile { get; }

        public event EventHandler OnDeleteAcc = delegate { };
        public event EventHandler DeleteAccFailed = delegate { };

        public UserProfileViewModel()
        {
            DeleteProfile = new Command(async () => await OnDeleteProfile());
            Logout = new Command(LogOutFunct);
        }
        void LogOutFunct()
        {
            App.UserInfo = new UserModel();
            App.IsLoggedIn = false;
            SecureStorage.RemoveAll();
            IsLoggedIn = false;
            Header = string.Empty;
            FullName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            CanChangePass = false;
        }

        public async void RefreshProfile()
        {
            if (App.IsLoggedIn)
            {
                var serverOrders = await DataStore.GetServerOrders(App.UserInfo.Email);
                if (serverOrders == null || serverOrders.Count == 0 || serverOrders.FindAll(or => or.Status != "Livrata" && or.Status != "Refuzata"
                    && or.Status != "Anulata").Count == 0)
                    CanEditLocation = true;
                else
                    CanEditLocation = false;
                IsLoggedIn = App.IsLoggedIn;
                Header = "Bun venit " + App.UserInfo.FullName;
                FullName = App.UserInfo.FullName;
                Email = App.UserInfo.Email;
                PhoneNumber = App.UserInfo.PhoneNumber;
                HasPasswordSet = App.UserInfo.HasSetPassword;
                CanChangePass = string.IsNullOrWhiteSpace(App.UserInfo.UserIdentification);
            }
        }

        async Task OnDeleteProfile()
        {
            var result = await AuthController.Execute(new UserModel
            {
                Email = Email,
                UserIdentification = App.UserInfo.UserIdentification,
                Password = App.UserInfo.Password,
            }, Constants.AuthOperations.Delete);
            if (!result.Contains("Email is wrong or user not existing.") && !result.Contains("result : False"))
            {
                App.UserInfo = new UserModel();
                App.IsLoggedIn = false;
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

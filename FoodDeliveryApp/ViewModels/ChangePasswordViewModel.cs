using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string confirmPassword = String.Empty;
        private string password = String.Empty;
        private string newpassword = String.Empty;
        public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string NewPassword { get => newpassword; set => SetProperty(ref newpassword, value); }

        public bool _loggedIn = !App.IsLoggedIn;
        public bool LoggedIn { get => _loggedIn; }

        public event EventHandler ChangePasswordSuc = delegate { };

        public event EventHandler ChangePasswordFailed = delegate { };

        public Command ChangePassword { get; }
        public ChangePasswordViewModel()
        {
            ChangePassword = new Command(async () => await ChangingPass());
            IsBusy = false;

        }
        private async Task ChangingPass()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = App.UserInfo.Email,
                    Password = Password,
                    NewPassword = NewPassword,
                }, Constants.AuthOperations.ChangePassword);
                IsBusy = false;
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Password changed."))
                {
                    App.UserInfo.Password = NewPassword;
                    SecureStorage.SetAsync(App.WEBPASS, NewPassword).Wait();
                    ChangePasswordSuc?.Invoke(this, new EventArgs());
                }
                else
                {
                    ChangePasswordFailed?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                ChangePasswordFailed?.Invoke(this, new EventArgs());
            }

        }
    }
}

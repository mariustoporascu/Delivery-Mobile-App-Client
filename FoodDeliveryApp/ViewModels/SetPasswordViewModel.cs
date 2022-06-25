using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class SetPasswordViewModel : BaseViewModel
    {
        private string confirmPassword = String.Empty;
        private string password = String.Empty;
        public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }

        public bool _loggedIn = !App.IsLoggedIn;
        public bool LoggedIn { get => _loggedIn; }

        public event EventHandler OnSetPasswordSuc = delegate { };

        public event EventHandler OnSetPasswordFailed = delegate { };

        public Command SetPassword { get; }
        public SetPasswordViewModel()
        {
            SetPassword = new Command(async () => await SettingPass());
            IsBusy = false;

        }
        private async Task SettingPass()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = App.UserInfo.Email,
                    Password = Password,
                    UserIdentification = App.UserInfo.UserIdentification,
                }, Constants.AuthOperations.SetPassword);
                IsBusy = false;

                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Password set."))
                {
                    App.UserInfo.HasSetPassword = true;
                    OnSetPasswordSuc?.Invoke(this, new EventArgs());
                }
                else
                {
                    OnSetPasswordFailed?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                OnSetPasswordFailed?.Invoke(this, new EventArgs());
            }

        }
    }
}

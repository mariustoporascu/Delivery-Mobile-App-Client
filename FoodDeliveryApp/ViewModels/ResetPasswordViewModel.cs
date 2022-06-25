using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private string confirmPassword = String.Empty;
        private string token = String.Empty;
        private string newpassword = String.Empty;
        private string userName = String.Empty;
        public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }
        public string Token { get => token; set => SetProperty(ref token, value); }
        public string NewPassword { get => newpassword; set => SetProperty(ref newpassword, value); }
        public string UserName { get => userName; set => SetProperty(ref userName, value); }

        public bool _loggedIn = !App.IsLoggedIn;
        public bool LoggedIn { get => _loggedIn; }

        public event EventHandler ResetPasswordSuc = delegate { };

        public event EventHandler ResetPasswordFailed = delegate { };
        public event EventHandler CoolDown = delegate { };

        public Command ResetPassword { get; }
        public ResetPasswordViewModel()
        {
            ResetPassword = new Command(async () => await ChangingPass());
            IsBusy = false;

        }
        private async Task ChangingPass()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    ResetTokenPass = Token,
                    Email = UserName,
                    NewPassword = NewPassword,
                }, Constants.AuthOperations.ResetPassword);
                IsBusy = false;

                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Password changed"))
                {

                    ResetPasswordSuc?.Invoke(this, new EventArgs());
                }
                else if (!string.IsNullOrWhiteSpace(result) && result.Contains("Tried too many times"))
                {
                    CoolDown?.Invoke(this, new EventArgs());

                }
                else
                {
                    ResetPasswordFailed?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                ResetPasswordFailed?.Invoke(this, new EventArgs());
            }

        }
    }
}

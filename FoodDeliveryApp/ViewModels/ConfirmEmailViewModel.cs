using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class ConfirmEmailViewModel : BaseViewModel
    {
        private string userName;
        private string token;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Token { get => token; set => SetProperty(ref token, value); }
        public Command ConfirmEmail { get; }
        public event EventHandler OnSignIn = delegate { };
        public event EventHandler OnSignInFailed = delegate { };

        public ConfirmEmailViewModel()
        {
            ConfirmEmail = new Command(async () => await Confirm());
            IsBusy = false;

        }
        public async Task Confirm()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = UserName,
                    ResetTokenPass = Token,
                }, Constants.AuthOperations.ConfirmEmail);
                IsBusy = false;
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Email Confirmed."))
                {
                    OnSignIn?.Invoke(this, new EventArgs());
                }
                else
                {
                    OnSignInFailed?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                OnSignInFailed?.Invoke(this, new EventArgs());
            }

        }
    }
}

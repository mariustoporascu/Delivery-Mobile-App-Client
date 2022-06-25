using FoodDeliveryApp.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class GenerateTokenViewModel : BaseViewModel
    {
        private string userName;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public Command GenerateToken { get; }
        public event EventHandler OnSignIn = delegate { };
        public event EventHandler HasCode = delegate { };
        public event EventHandler OnSignInFailed = delegate { };

        public GenerateTokenViewModel()
        {
            GenerateToken = new Command(async () => await Generate());
            IsBusy = false;

        }
        public async Task Generate()
        {
            IsBusy = true;
            try
            {
                var result = await AuthController.Execute(new UserModel
                {
                    Email = UserName,
                }, Constants.AuthOperations.GenerateToken);
                IsBusy = false;
                if (!string.IsNullOrWhiteSpace(result) && result.Contains("Token sent."))
                {
                    OnSignIn?.Invoke(this, new EventArgs());
                }
                else if (!string.IsNullOrWhiteSpace(result) && result.Contains("Already generated"))
                {
                    HasCode?.Invoke(this, new EventArgs());
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

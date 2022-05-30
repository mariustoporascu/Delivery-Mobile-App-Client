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
        public event EventHandler OnSignInFailed = delegate { };

        public GenerateTokenViewModel()
        {
            GenerateToken = new Command(async () => await Generate());
        }
        public async Task Generate()
        {
            var result = await AuthController.Execute(new UserModel
            {
                Email = UserName,
            }, Constants.AuthOperations.GenerateToken);
            if (!string.IsNullOrWhiteSpace(result) && result.Contains("Token sent."))
            {
                OnSignIn?.Invoke(this, new EventArgs());
            }
            else
            {
                OnSignInFailed?.Invoke(this, new EventArgs());
            }
        }
    }
}

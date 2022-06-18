using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace FoodDeliveryApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string userName;
        private string fullName;
        private string confirmPassword;
        private string password;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string FullName { get => fullName; set => SetProperty(ref fullName, value); }
        public string ConfirmPassword { get => confirmPassword; set => SetProperty(ref confirmPassword, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }

        public bool _loggedIn = !App.IsLoggedIn;
        public bool LoggedIn { get => _loggedIn; }
        public Command SignUpWebCommand { get; set; }

        public event EventHandler OnSignIn = delegate { };

        public event EventHandler OnSignInFailed = delegate { };

        public event EventHandler OnSignUpWeb = delegate { };


        public RegisterViewModel()
        {
            SignUpWebCommand = new Command(async () => await SignUpWithWeb());
        }

        private async Task SignUpWithWeb()
        {
            var serverResp = await AuthController.Execute(new UserModel
            {
                FullName = FullName,
                Email = UserName,
                Password = Password,
                FireBaseToken = App.FirebaseUserToken

            }, Constants.AuthOperations.Create);
            if (!string.IsNullOrEmpty(serverResp) && serverResp.Contains("Account created, you can now login."))
                OnSignUpWeb?.Invoke(this, new EventArgs());
            else
                OnSignInFailed?.Invoke(this, new EventArgs());
        }



    }
}

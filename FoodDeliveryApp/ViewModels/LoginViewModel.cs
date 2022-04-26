using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string userName;
        private string password;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public Command Login { get; }
        public event EventHandler OnSignIn = delegate { };
        public event EventHandler OnSignInFailed = delegate { };
        public LoginViewModel()
        {
            Login = new Command(async () => await AfterSignIn());
        }

        async Task AfterSignIn()
        {
            var authService = DependencyService.Get<IAuthController>();
            string loginResult = await authService.LoginUser(new UserModel { Email = UserName, Password = Password });

            if (loginResult != string.Empty && !loginResult.Contains("Password is wrong.")
                && !loginResult.Contains("Email is wrong or user not existing.") && !loginResult.Contains("Login data invalid."))
            {
                App.isLoggedIn = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                App.userInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                App.userInfo.Email = UserName;
                App.userInfo.Password = Password;
                SecureStorage.SetAsync(App.WEBEMAIL, UserName).Wait();
                SecureStorage.SetAsync(App.WEBPASS, Password).Wait();
                SecureStorage.SetAsync(App.LOGIN_WITH, "WebLogin").Wait();
                MessagingCenter.Send<LoginViewModel>(this, "UpdateProfile");
                OnSignIn?.Invoke(this, new EventArgs());

            }
            else
                OnSignInFailed?.Invoke(this, new EventArgs());

        }
    }
}

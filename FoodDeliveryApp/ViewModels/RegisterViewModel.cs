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

        public bool _loggedIn = !App.isLoggedIn;
        public bool LoggedIn { get => _loggedIn; }
        public bool IsAppleSignInAvailable { get { return appleSignInService?.IsAvailable ?? false; } }
        public Command SignInWithAppleCommand { get; set; }
        public Command SignUpWebCommand { get; set; }

        public event EventHandler OnSignIn = delegate { };

        public event EventHandler OnSignInFailed = delegate { };

        public event EventHandler OnSignUpWeb = delegate { };

        IAppleSignInService appleSignInService;

        public RegisterViewModel()
        {

            appleSignInService = DependencyService.Get<IAppleSignInService>();
            SignInWithAppleCommand = new Command(async () => await OnAppleSignInRequest());
            SignUpWebCommand = new Command(async () => await SignUpWithWeb());
        }

        private async Task SignUpWithWeb()
        {
            var serverResp = await AuthController.Execute(new UserModel
            {
                FullName = FullName,
                Email = UserName,
                Password = Password,
            }, Constants.AuthOperations.Create);
            if (!string.IsNullOrEmpty(serverResp) && serverResp.Contains("Account created, you can now login."))
                OnSignUpWeb?.Invoke(this, new EventArgs());
            else
                OnSignInFailed?.Invoke(this, new EventArgs());
        }

        async Task OnAppleSignInRequest()
        {
            try
            {
                var account = await appleSignInService.SignInAsync();
                if (account != null && !string.IsNullOrWhiteSpace(account.Email))
                {
                    SecureStorage.SetAsync(App.APPLE_ID_EMAIL, account.Email).Wait();
                    SecureStorage.SetAsync(App.APPLE_ID_NAME, account.Name).Wait();
                    SecureStorage.SetAsync(App.APPLE_ID, account.UserId).Wait();
                    SecureStorage.SetAsync(App.LOGIN_WITH, "Apple").Wait();

                    var serverResp = await AuthController.Execute(new UserModel
                    {
                        Email = account.Email,
                        FullName = account.Name,
                        UserIdentification = account.UserId,
                    }, Constants.AuthOperations.Create);
                    //if (!string.IsNullOrEmpty(serverResp) && await AfterSignIn())
                    //    OnSignIn?.Invoke(this, new EventArgs());
                    //else
                    //    OnSignInFailed?.Invoke(this, new EventArgs());
                }
                else
                    OnSignInFailed?.Invoke(this, new EventArgs());
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                OnSignInFailed?.Invoke(this, new EventArgs());
            }

        }

    }
}

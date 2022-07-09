using LivroApp.Constants;
using LivroApp.Models;
using LivroApp.Models.AuthModels;
using LivroApp.Services;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace LivroApp.ViewModels.AuthVModels
{
    public class LoginViewModel : BaseViewModel<BaseModel>
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private readonly OidcIdentity _oidcIdentity;
        private readonly IAppleSignInService _appleSignInService;
        public bool IsGoogleSignInAvailable { get { return Device.RuntimePlatform == Device.Android; } }
        public bool IsAppleSignInAvailable { get { return _appleSignInService?.IsAvailable ?? false; } }

        private bool _facebookLoginEnabled = false;
        public bool FacebookLoginEnabled { get => _facebookLoginEnabled; set => SetProperty(ref _facebookLoginEnabled, value); }
        public event EventHandler SuccessDelegate = delegate { };
        public event EventHandler FailedDelegate = delegate { };
        public event EventHandler RequireConfirmEmail = delegate { };
        public Command ExecuteLoginLivro { get; set; }
        public Command ExecuteLoginGoogle { get; set; }
        public Command ExecuteLoginFacebook { get; set; }
        public Command ExecuteLoginApple { get; set; }
        public LoginViewModel()
        {
            _oidcIdentity = new OidcIdentity();
            _appleSignInService = DependencyService.Get<IAppleSignInService>();
            ExecuteLoginLivro = new Command(async () => await LoginLivroCred());
            ExecuteLoginApple = new Command(async () => await LoginWithApple());
            ExecuteLoginGoogle = new Command(async () => await LoginWithGoogle());
            ExecuteLoginFacebook = new Command(async () => await LoginWithFacebook());
        }
        async Task LoginLivroCred()
        {
            IsBusy = true;
            try
            {
                SecureStorage.SetAsync(App.LOGIN_EMAIL, UserName).Wait();
                SecureStorage.SetAsync(App.LOGIN_PASSWORD, Password).Wait();
                SecureStorage.SetAsync(App.LOGIN_TYPE, LoginTypeEnum.LivroWeb.ToString()).Wait();
                if (await SignInAsync())
                    SuccessDelegate?.Invoke(this, new EventArgs());
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }

        }
        async Task<bool> SignInAsync()
        {
            await App.TryLogin();

            return App.IsLoggedIn;
        }
        private async Task LoginWithFacebook()
        {
            IsBusy = true;
            try
            {
                FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
                if (response != null && !response.Message.Contains("User cancelled facebook operation"))
                {
                    FacebookResponse<string> responseData = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name" }, new string[] { "email" });
                    if (responseData != null && !response.Message.Contains("User cancelled facebook login operation"))
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        var userData = JsonConvert.DeserializeObject<FaceBookAcc>(responseData.Data, settings);
                        SecureStorage.SetAsync(App.LOGIN_EMAIL, userData.Email).Wait();
                        SecureStorage.SetAsync(App.LOGIN_USERID, userData.Id).Wait();
                        SecureStorage.SetAsync(App.LOGIN_TYPE, LoginTypeEnum.Facebook.ToString()).Wait();
                        var serverResp = await AuthController.Execute(new UserModel
                        {
                            Email = userData.Email,
                            FullName = string.Concat(userData.First_Name, " ", userData.Last_Name),
                            UserIdentification = userData.Id,
                            FireBaseToken = App.FirebaseUserToken
                        }, AuthOperations.Create);
                        if (!string.IsNullOrEmpty(serverResp) &&
                            (serverResp.Contains("Account created, you can now login.") || serverResp.Contains("Reused previous loginwithothers."))
                            && await SignInAsync())
                            SuccessDelegate?.Invoke(this, new EventArgs());
                        else
                            FailedDelegate?.Invoke(this, new EventArgs());
                    }
                    else
                        FailedDelegate?.Invoke(this, new EventArgs());
                }
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }
        }
        private async Task LoginWithGoogle()
        {
            IsBusy = true;
            try
            {
                GoogleCredentials credentials = await _oidcIdentity.Authenticate();
                if (credentials != null && string.IsNullOrEmpty(credentials.Error) && !string.IsNullOrWhiteSpace(credentials.AccessToken))
                {
                    GoogleUserInfo userInfo = await _oidcIdentity.GetUserInfo(credentials.AccessToken);
                    if (userInfo != null)
                    {
                        var userData = userInfo.UserClaim.ToList();
                        SecureStorage.SetAsync(App.LOGIN_EMAIL, userData.FirstOrDefault(claim => claim.Type == "email").Value).Wait();
                        SecureStorage.SetAsync(App.LOGIN_USERID, userData.FirstOrDefault(claim => claim.Type == "sub").Value).Wait();
                        SecureStorage.SetAsync(App.LOGIN_TYPE, LoginTypeEnum.Google.ToString()).Wait();

                        var serverResp = await AuthController.Execute(new UserModel
                        {
                            Email = userData.FirstOrDefault(claim => claim.Type == "email").Value,
                            FullName = userData.FirstOrDefault(claim => claim.Type == "name").Value,
                            UserIdentification = userData.FirstOrDefault(claim => claim.Type == "sub").Value,
                            FireBaseToken = App.FirebaseUserToken
                        }, AuthOperations.Create);
                        if (!string.IsNullOrEmpty(serverResp) &&
                            (serverResp.Contains("Account created, you can now login.") || serverResp.Contains("Reused previous loginwithothers."))
                            && await SignInAsync())
                            SuccessDelegate?.Invoke(this, new EventArgs());
                        else
                            FailedDelegate?.Invoke(this, new EventArgs());
                    }
                    else
                        FailedDelegate?.Invoke(this, new EventArgs());
                }
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }
        }
        private async Task LoginWithApple()
        {
            IsBusy = true;
            var appleDefault = "apple@apple.com";
            try
            {
                var account = await _appleSignInService.SignInAsync();
                if (account != null && !string.IsNullOrWhiteSpace(account.Email))
                {
                    SecureStorage.SetAsync(App.LOGIN_EMAIL, appleDefault).Wait();
                    SecureStorage.SetAsync(App.LOGIN_USERID, account.UserId).Wait();
                    SecureStorage.SetAsync(App.LOGIN_TYPE, LoginTypeEnum.Apple.ToString()).Wait();

                    var serverResp = await AuthController.Execute(new UserModel
                    {
                        Email = account.Email,
                        FullName = account.Name,
                        UserIdentification = account.UserId,
                        FireBaseToken = App.FirebaseUserToken
                    }, AuthOperations.Create);
                    if (!string.IsNullOrEmpty(serverResp) &&
                            (serverResp.Contains("Account created, you can now login.") || serverResp.Contains("Reused previous loginwithothers."))
                            && await SignInAsync())
                        SuccessDelegate?.Invoke(this, new EventArgs());
                    else
                        FailedDelegate?.Invoke(this, new EventArgs());
                }
                else if (account != null && !string.IsNullOrWhiteSpace(account.UserId))
                {
                    SecureStorage.SetAsync(App.LOGIN_EMAIL, appleDefault).Wait();
                    SecureStorage.SetAsync(App.LOGIN_USERID, account.UserId).Wait();
                    SecureStorage.SetAsync(App.LOGIN_TYPE, LoginTypeEnum.Apple.ToString()).Wait();
                    if (await SignInAsync())
                        SuccessDelegate?.Invoke(this, new EventArgs());
                    else
                        FailedDelegate?.Invoke(this, new EventArgs());
                }
                else
                    FailedDelegate?.Invoke(this, new EventArgs());
            }
            catch (Exception) { FailedDelegate?.Invoke(this, new EventArgs()); }
            finally { IsBusy = false; }

        }

    }
}

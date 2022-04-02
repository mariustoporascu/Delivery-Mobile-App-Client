using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodDeliveryApp.Annotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace FoodDeliveryApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string GoogleUrl = "https://accounts.google.com";
        private const string ClientId = "121275221168-i230p391b86n5rfu4quoq7b6tijitu72.apps.googleusercontent.com";
        private const string scope = "openid profile email";
        private UserInfo? _userInfoGoogle;
        private readonly OidcIdentity _oidcIdentity;

        public bool _loggedIn = !App.isLoggedIn;
        public bool LoggedIn { get => _loggedIn; }
        public bool IsAppleSignInAvailable { get { return appleSignInService?.IsAvailable ?? false; } }
        public bool IsGoogleSignInAvailable { get { return !appleSignInService?.IsAvailable ?? true; } }
        public ICommand SignInWithAppleCommand { get; set; }

        public event EventHandler OnSignIn = delegate { };
        public event EventHandler OnSignInFailed = delegate { };

        IAppleSignInService appleSignInService;
        IAuthController authController;

        public LoginViewModel()
        {
            _oidcIdentity = new OidcIdentity(ClientId, App.CallbackScheme, App.SignoutCallbackScheme, scope, GoogleUrl);
            ExecuteLoginGoogle = new Command(LoginWithGoogle);
            ExecuteLoginFacebook = new Command(LoginWithFacebook);
            appleSignInService = DependencyService.Get<IAppleSignInService>();
            authController = DependencyService.Get<IAuthController>();
            SignInWithAppleCommand = new Command(OnAppleSignInRequest);
        }

        public ICommand ExecuteLoginGoogle { get; }
        public ICommand ExecuteLoginFacebook { get; }

        private async void LoginWithFacebook()
        {
            try
            {
                FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
                FacebookResponse<string> responseData = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
                if (responseData != null)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var userData = JsonConvert.DeserializeObject<FaceBookAcc>(responseData.Data, settings);
                    SecureStorage.SetAsync(App.FACEBOOK_ID_EMAIL, userData.Email).Wait();
                    SecureStorage.SetAsync(App.FACEBOOK_ID_NAME, string.Concat(userData.First_Name, " ", userData.Last_Name)).Wait();
                    SecureStorage.SetAsync(App.FACEBOOK_ID, userData.Id).Wait();
                    SecureStorage.SetAsync(App.LOGIN_WITH, "Facebook").Wait();
                    var serverResp = await authController.CreateUser(new UserModel
                    {
                        Email = userData.Email,
                        FullName = string.Concat(userData.First_Name, " ", userData.Last_Name),
                        UserIdentification = userData.Id,
                    });
                    if (!string.IsNullOrEmpty(serverResp) && await AfterSignIn())
                        OnSignIn?.Invoke(this, default(EventArgs));
                    else
                        OnSignInFailed?.Invoke(this, new EventArgs());
                }
                else
                    OnSignInFailed?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private async void LoginWithGoogle()
        {
            try
            {
                Credentials credentials = await _oidcIdentity.Authenticate();
                if (credentials != null && string.IsNullOrEmpty(credentials.Error))
                {
                    UserInfo userInfo = await _oidcIdentity.GetUserInfo(credentials.AccessToken);
                    if (userInfo != null)
                    {
                        var userData = userInfo.UserClaim.ToList();
                        SecureStorage.SetAsync(App.GOOGLE_ID_EMAIL, userData.FirstOrDefault(claim => claim.Type == "email").Value).Wait();
                        SecureStorage.SetAsync(App.GOOGLE_ID_NAME, userData.FirstOrDefault(claim => claim.Type == "name").Value).Wait();
                        SecureStorage.SetAsync(App.GOOGLE_ID, userData.FirstOrDefault(claim => claim.Type == "sub").Value).Wait();
                        SecureStorage.SetAsync(App.LOGIN_WITH, "Google").Wait();

                        var serverResp = await authController.CreateUser(new UserModel
                        {
                            Email = userData.FirstOrDefault(claim => claim.Type == "email").Value,
                            FullName = userData.FirstOrDefault(claim => claim.Type == "name").Value,
                            UserIdentification = userData.FirstOrDefault(claim => claim.Type == "sub").Value,
                        });
                        if (!string.IsNullOrEmpty(serverResp) && await AfterSignIn())
                            OnSignIn?.Invoke(this, default(EventArgs));
                        else
                            OnSignInFailed?.Invoke(this, new EventArgs());
                    }
                    else
                        OnSignInFailed?.Invoke(this, new EventArgs());
                }
                else
                    OnSignInFailed?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        async void OnAppleSignInRequest()
        {
            try
            {
                var account = await appleSignInService.SignInAsync();
                if (account != null)
                {
                    SecureStorage.SetAsync(App.APPLE_ID_EMAIL, account.Email).Wait();
                    SecureStorage.SetAsync(App.APPLE_ID_NAME, account.Name).Wait();
                    SecureStorage.SetAsync(App.APPLE_ID, account.UserId).Wait();
                    SecureStorage.SetAsync(App.LOGIN_WITH, "Apple").Wait();

                    var serverResp = await authController.CreateUser(new UserModel
                    {
                        Email = account.Email,
                        FullName = account.Name,
                        UserIdentification = account.UserId,
                    });
                    if (!string.IsNullOrEmpty(serverResp) && await AfterSignIn())
                        OnSignIn?.Invoke(this, default(EventArgs));
                    else
                        OnSignInFailed?.Invoke(this, new EventArgs());
                }
                else
                    OnSignInFailed?.Invoke(this, new EventArgs());
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
        async Task<bool> AfterSignIn()
        {
            string loginResult = string.Empty;
            string finalEmail = string.Empty;
            string finalId = string.Empty;
            var authService = DependencyService.Get<IAuthController>();
            var gMail = await SecureStorage.GetAsync(App.GOOGLE_ID_EMAIL);
            var gMailId = await SecureStorage.GetAsync(App.GOOGLE_ID);
            var fMail = await SecureStorage.GetAsync(App.FACEBOOK_ID_EMAIL);
            var fMailId = await SecureStorage.GetAsync(App.FACEBOOK_ID);
            var aMail = await SecureStorage.GetAsync(App.APPLE_ID_EMAIL);
            var aMailId = await SecureStorage.GetAsync(App.APPLE_ID);
            var lWith = await SecureStorage.GetAsync(App.LOGIN_WITH);
            if (!string.IsNullOrEmpty(lWith))
            {
                if (lWith.Equals("Google"))
                {
                    loginResult = await authService.LoginUser(new UserModel { Email = gMail, UserIdentification = gMailId });
                    finalEmail = gMail;
                    finalId = gMailId;
                }
                else if (lWith.Equals("Facebook"))
                {
                    loginResult = await authService.LoginUser(new UserModel { Email = fMail, UserIdentification = fMailId });
                    finalEmail = fMail;
                    finalId = fMailId;
                }
                else if (lWith.Equals("Apple"))
                {
                    loginResult = await authService.LoginUser(new UserModel { Email = aMail, UserIdentification = aMailId });
                    finalEmail = aMail;
                    finalId = aMailId;
                }
            }

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
                App.userInfo.Email = finalEmail;
                App.userInfo.UserIdentification = finalId;
            }
            MessagingCenter.Send<LoginViewModel>(this, "UpdateProfile");
            return App.isLoggedIn;
        }
    }
}

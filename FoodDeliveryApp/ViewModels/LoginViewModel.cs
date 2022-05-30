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
    public class LoginViewModel : BaseViewModel
    {
        private string userName;
        private string password;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public Command Login { get; }
        public event EventHandler OnSignIn = delegate { };
        public event EventHandler OnSignInFailed = delegate { };
        public event EventHandler RequireConfirmEmail = delegate { };
        private readonly OidcIdentity _oidcIdentity;
        public bool IsGoogleSignInAvailable { get { return Device.RuntimePlatform == Device.Android ? true : false; } }

        public Command ExecuteLoginGoogle { get; }

        public Command ExecuteLoginFacebook { get; }
        public LoginViewModel()
        {
            _oidcIdentity = new OidcIdentity();
            Login = new Command(async () => await AfterSignIn());
            ExecuteLoginGoogle = new Command(async () => await LoginWithGoogle());
            ExecuteLoginFacebook = new Command(async () => await LoginWithFacebook());
        }
        async Task<bool> AfterSignInOthers()
        {
            string loginResult = string.Empty;
            string finalEmail = string.Empty;
            string finalId = string.Empty;
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
                    loginResult = await AuthController.Execute(new UserModel { Email = gMail, UserIdentification = gMailId }, Constants.AuthOperations.Login);
                    finalEmail = gMail;
                    finalId = gMailId;
                }
                else if (lWith.Equals("Facebook"))
                {
                    loginResult = await AuthController.Execute(new UserModel { Email = fMail, UserIdentification = fMailId }, Constants.AuthOperations.Login);
                    finalEmail = fMail;
                    finalId = fMailId;
                }
                else if (lWith.Equals("Apple"))
                {
                    loginResult = await AuthController.Execute(new UserModel { Email = aMail, UserIdentification = aMailId }, Constants.AuthOperations.Login);
                    finalEmail = aMail;
                    finalId = aMailId;
                }
            }

            if (loginResult != string.Empty && !loginResult.Contains("Password is wrong.")
                && !loginResult.Contains("Email is wrong or user not existing.") && !loginResult.Contains("Login data invalid."))
            {
                App.IsLoggedIn = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                App.UserInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                App.UserInfo.Email = finalEmail;
                App.UserInfo.UserIdentification = finalId;
            }

            return App.IsLoggedIn;
        }
        async Task AfterSignIn()
        {
            var authService = DependencyService.Get<IAuthController>();
            string loginResult = await authService.Execute(new UserModel { Email = UserName, Password = Password }, Constants.AuthOperations.Login);
            if (loginResult != string.Empty && loginResult.Contains("Email not confirmed"))
            {
                RequireConfirmEmail?.Invoke(this, new EventArgs());
            }
            else if (loginResult != string.Empty && !loginResult.Contains("Password is wrong.")
                && !loginResult.Contains("Email is wrong or user not existing.") && !loginResult.Contains("Login data invalid."))
            {
                App.IsLoggedIn = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                App.UserInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                App.UserInfo.Email = UserName;
                App.UserInfo.Password = Password;
                SecureStorage.SetAsync(App.WEBEMAIL, UserName).Wait();
                SecureStorage.SetAsync(App.WEBPASS, Password).Wait();
                SecureStorage.SetAsync(App.LOGIN_WITH, "WebLogin").Wait();
                OnSignIn?.Invoke(this, new EventArgs());

            }
            else
                OnSignInFailed?.Invoke(this, new EventArgs());

        }
        private async Task LoginWithFacebook()
        {
            try
            {
                FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
                if (response != null && !response.Message.Contains("User cancelled facebook operation"))
                {
                    FacebookResponse<string> responseData = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
                    if (responseData != null && !response.Message.Contains("User cancelled facebook login operation"))
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
                        var serverResp = await AuthController.Execute(new UserModel
                        {
                            Email = userData.Email,
                            FullName = string.Concat(userData.First_Name, " ", userData.Last_Name),
                            UserIdentification = userData.Id,
                        }, Constants.AuthOperations.Create);
                        if (!string.IsNullOrEmpty(serverResp) &&
                            (serverResp.Contains("Account created, you can now login.") || serverResp.Contains("Reused previous loginwithothers."))
                            && await AfterSignInOthers())
                            OnSignIn?.Invoke(this, new EventArgs());
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
                OnSignInFailed?.Invoke(this, new EventArgs());
            }
        }
        private async Task LoginWithGoogle()
        {
            try
            {
                Credentials credentials = await _oidcIdentity.Authenticate();
                if (credentials != null && string.IsNullOrEmpty(credentials.Error) && !string.IsNullOrWhiteSpace(credentials.AccessToken))
                {
                    UserInfo userInfo = await _oidcIdentity.GetUserInfo(credentials.AccessToken);
                    if (userInfo != null)
                    {
                        var userData = userInfo.UserClaim.ToList();
                        SecureStorage.SetAsync(App.GOOGLE_ID_EMAIL, userData.FirstOrDefault(claim => claim.Type == "email").Value).Wait();
                        SecureStorage.SetAsync(App.GOOGLE_ID_NAME, userData.FirstOrDefault(claim => claim.Type == "name").Value).Wait();
                        SecureStorage.SetAsync(App.GOOGLE_ID, userData.FirstOrDefault(claim => claim.Type == "sub").Value).Wait();
                        SecureStorage.SetAsync(App.LOGIN_WITH, "Google").Wait();

                        var serverResp = await AuthController.Execute(new UserModel
                        {
                            Email = userData.FirstOrDefault(claim => claim.Type == "email").Value,
                            FullName = userData.FirstOrDefault(claim => claim.Type == "name").Value,
                            UserIdentification = userData.FirstOrDefault(claim => claim.Type == "sub").Value,
                        }, Constants.AuthOperations.Create);
                        if (!string.IsNullOrEmpty(serverResp) &&
                            (serverResp.Contains("Account created, you can now login.") || serverResp.Contains("Reused previous loginwithothers."))
                            && await AfterSignInOthers())
                            OnSignIn?.Invoke(this, new EventArgs());
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
                OnSignInFailed?.Invoke(this, new EventArgs());
            }
        }

    }
}

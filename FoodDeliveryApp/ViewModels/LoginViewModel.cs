using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FoodDeliveryApp.Annotations;
using FoodDeliveryApp.Services;
using Plugin.FacebookClient;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string AuthorityUrl = "https://accounts.google.com";
        private Credentials? _credentials;
        private UserInfo? _userInfo;
        private readonly OidcIdentity _oidcIdentity;

        public LoginViewModel()
        {
            // const string scope = "openid profile offline_access";
            const string scope = "openid profile email";
            _oidcIdentity = new OidcIdentity("121275221168-m5ng9m5r5mhdlcj31r9pl0c21m63gr6b.apps.googleusercontent.com", App.CallbackScheme, App.SignoutCallbackScheme, scope, AuthorityUrl);
            ExecuteLogin = new Command(Login);
            ExecuteRefresh = new Command(RefreshTokens);
            ExecuteLogout = new Command(Logout);
            ExecuteGetInfo = new Command(GetInfo);
            ExecuteCopyAccessToken = new Command(async () => await Clipboard.SetTextAsync(_credentials?.AccessToken));
            ExecuteCopyIdentityToken = new Command(async () => await Clipboard.SetTextAsync(_credentials?.IdentityToken));
        }

        public ICommand ExecuteLogin { get; }
        public ICommand ExecuteRefresh { get; }
        public ICommand ExecuteLogout { get; }
        public ICommand ExecuteGetInfo { get; }
        public ICommand ExecuteCopyAccessToken { get; }
        public ICommand ExecuteCopyIdentityToken { get; }

        public string TokenExpirationText => "Access Token expires at: " + _credentials?.AccessTokenExpiration;
        public string AccessTokenText => "Access Token: " + _credentials?.AccessToken;
        public string IdTokenText => "Id Token: " + _credentials?.IdentityToken;
        public string User => "User: " + _userInfo?.UserClaim.ToList().FirstOrDefault(claim => claim.Type == "name").Value;
        public string UserEmail => "UserEmail: " + _userInfo?.UserClaim.ToList().FirstOrDefault(claim => claim.Type == "email").Value;
        public bool IsLoggedIn => _credentials != null;
        public bool IsNotLoggedIn => _credentials == null;

        private async void GetInfo()
        {
            var url = Path.Combine(AuthorityUrl, "manage", "index");
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (string.IsNullOrEmpty(_credentials?.RefreshToken))
                {
                    // no valid refresh token exists => authenticate
                    await _oidcIdentity.Authenticate();
                }
                else
                {
                    // we have a valid refresh token => refresh tokens
                    await _oidcIdentity.RefreshToken(_credentials!.RefreshToken);
                }
            }

            Debug.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private async void Login()
        {
            try
            {
                FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
                Debug.WriteLine(response);
                FacebookResponse<string> responseData = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
                Debug.WriteLine(responseData);
                Credentials credentials = await _oidcIdentity.Authenticate();
                UserInfo userInfo = await _oidcIdentity.GetUserInfo(credentials.AccessToken);
                UpdateCredentials(credentials, userInfo);

                _httpClient.DefaultRequestHeaders.Authorization = credentials.IsError
                    ? null
                    : new AuthenticationHeaderValue("bearer", credentials.AccessToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void RefreshTokens()
        {
            if (_credentials?.RefreshToken == null) return;
            Credentials credentials = await _oidcIdentity.RefreshToken(_credentials.RefreshToken);
            UpdateCredentials(credentials);
        }

        private async void Logout()
        {
            await _oidcIdentity.Logout(_credentials?.IdentityToken);
            _credentials = null;
            _userInfo = null;
            OnPropertyChanged(nameof(UserEmail));
            OnPropertyChanged(nameof(TokenExpirationText));
            OnPropertyChanged(nameof(AccessTokenText));
            OnPropertyChanged(nameof(IdTokenText));
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsNotLoggedIn));
        }

        private void UpdateCredentials(Credentials credentials, UserInfo userInfo = null)
        {
            if (credentials.RefreshToken == null)
            {
                credentials.RefreshToken = _credentials.RefreshToken;
            }
            _credentials = credentials;
            if (userInfo != null)
                _userInfo = userInfo;
            OnPropertyChanged(nameof(UserEmail));
            OnPropertyChanged(nameof(TokenExpirationText));
            OnPropertyChanged(nameof(AccessTokenText));
            OnPropertyChanged(nameof(IdTokenText));
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(User));
            OnPropertyChanged(nameof(IsNotLoggedIn));
        }
        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync("//EntryFoodAppPage");
        }
    }
}

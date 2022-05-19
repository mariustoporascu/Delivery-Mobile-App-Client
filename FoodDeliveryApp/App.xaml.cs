using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoodDeliveryApp
{
    public partial class App : Application
    {
        public const string CallbackUri = "com.tmiit.fooddeliveryapp";
        public static readonly string CallbackScheme = $"{CallbackUri}:/authenticated";
        public static readonly string SignoutCallbackScheme = $"{CallbackUri}:/signout-callback-oidc";
        public static UserModel userInfo = new UserModel();
        public const string LOGIN_WITH = "LoginWith";
        public const string APPLE_ID = "AppleId";
        public const string APPLE_ID_EMAIL = "AppleIdEmail";
        public const string APPLE_ID_NAME = "AppleIdName";
        public const string GOOGLE_ID = "GoogleId";
        public const string GOOGLE_ID_EMAIL = "GoogleIdEmail";
        public const string GOOGLE_ID_NAME = "GoogleIdName";
        public const string FACEBOOK_ID = "FacebookId";
        public const string FACEBOOK_ID_EMAIL = "FacebookIdEmail";
        public const string FACEBOOK_ID_NAME = "FacebookIdName";
        public const string WEBEMAIL = "WebEmail";
        public const string WEBPASS = "WebPass";
        public static bool isLoggedIn = false;

        public bool PromptToConfirmExit
        {
            get
            {
                bool promptToConfirmExit = false;
                if (Shell.Current.Navigation.NavigationStack.Count == 1)
                {
                    promptToConfirmExit = true;
                }
                return promptToConfirmExit;
            }
        }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDataStore, MockDataStore>();
            DependencyService.Register<IAuthController, AuthService>();
            DependencyService.Register<IOrderServ, OrderServ>();
                MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();
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
            var webMail = await SecureStorage.GetAsync(App.WEBEMAIL);
            var webPass = await SecureStorage.GetAsync(App.WEBPASS);
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
                else if (lWith.Equals("WebLogin"))
                {
                    loginResult = await authService.LoginUser(new UserModel { Email = webMail, Password = webPass });
                    finalEmail = webMail;
                }
            }

            if (loginResult != string.Empty && !loginResult.Contains("Password is wrong.")
                && !loginResult.Contains("Email is wrong or user not existing.") && !loginResult.Contains("Login data invalid."))
            {
                isLoggedIn = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                userInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                userInfo.Email = finalEmail;
                if (string.IsNullOrEmpty(finalId))
                {
                    userInfo.Password = webPass;
                }
                else
                    userInfo.UserIdentification = finalId;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public bool IsToastExitConfirmation
        {
            get => Preferences.Get(nameof(IsToastExitConfirmation), false);
            set => Preferences.Set(nameof(IsToastExitConfirmation), value);
        }
    }
}

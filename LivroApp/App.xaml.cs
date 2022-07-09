using LivroApp.Constants;
using LivroApp.Models.AuthModels;
using LivroApp.Services;
using LivroApp.Views;
using Newtonsoft.Json;
using OneSignalSDK.Xamarin;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LivroApp
{
    public partial class App : Application
    {
        public const string CallbackUri = "com.tmiit.livroapp";
        public static readonly string CallbackScheme = $"{CallbackUri}:/authenticated";
        public static readonly string SignoutCallbackScheme = $"{CallbackUri}:/signout-callback-oidc";
        public const string LOGIN_TYPE = "LoginType";
        public const string LOGIN_EMAIL = "Email";
        public const string LOGIN_USERID = "UserId";
        public const string LOGIN_PASSWORD = "AppleIdName";
        public const string FIREBASE_TOKEN = "FireBaseToken";
        public static bool IsLoggedIn = false;
        public static string FirebaseUserToken = string.Empty;

        private static UserModel userInfo = new UserModel();
        public static UserModel UserInfo { get => userInfo; set => userInfo = value; }

        public bool PromptToConfirmExit
        {
            get
            {
                bool promptToConfirmExit = false;
                if (Shell.Current.Navigation.NavigationStack.Count == 1)
                    promptToConfirmExit = true;
                if (Shell.Current.Navigation.ModalStack.Count > 0)
                    promptToConfirmExit = false;
                return promptToConfirmExit;
            }
        }

        public App()
        {
            InitializeComponent();
            DependencyService.Register<IDataStore, MockDataStore>();
            DependencyService.Register<IAuthController, AuthService>();
            DependencyService.Register<IOrderServ, OrderServ>();

            MainPage = new LoadingPage();

            OneSignal.Default.Initialize("67b1b944-bcf4-467a-a6ae-4f0f0512b038");
            OneSignal.Default.PromptForPushNotificationsWithUserResponse();

            FirebaseUserToken = OneSignal.Default.DeviceState.userId;
            try
            {
                SecureStorage.SetAsync(FIREBASE_TOKEN, FirebaseUserToken).Wait();
            }
            catch (Exception) { }

        }
        protected override async void OnStart()
        {
            base.OnStart();
            await TryLogin();
        }
        public static async Task TryLogin()
        {
            string loginResult = string.Empty;
            var authService = DependencyService.Get<IAuthController>();
            var email = await SecureStorage.GetAsync(LOGIN_EMAIL);
            var userID = await SecureStorage.GetAsync(LOGIN_USERID);
            var password = await SecureStorage.GetAsync(LOGIN_PASSWORD);
            var loginType = await SecureStorage.GetAsync(LOGIN_TYPE);

            FirebaseUserToken = await SecureStorage.GetAsync(FIREBASE_TOKEN);
            if (!string.IsNullOrWhiteSpace(loginType))
            {
                switch (loginType)
                {
                    case "Google":
                    case "Facebook":
                    case "Apple":
                        loginResult = await authService.Execute(new UserModel
                        {
                            Email = email,
                            UserIdentification = userID,
                            FireBaseToken = FirebaseUserToken
                        }, AuthOperations.Login);
                        break;
                    case "LivroWeb":
                        loginResult = await authService.Execute(new UserModel
                        {
                            Email = email,
                            Password = password,
                            FireBaseToken = FirebaseUserToken
                        }, AuthOperations.Login);
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrWhiteSpace(loginResult) && !(loginResult.Contains("Password is wrong.")
                    || loginResult.Contains("Email is wrong or user not existing.") || loginResult.Contains("Login data invalid.")))
                {
                    IsLoggedIn = true;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    UserInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                    if (string.IsNullOrWhiteSpace(userID))
                    {
                        UserInfo.Password = password;
                    }
                    else
                        UserInfo.UserIdentification = userID;
                }

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

using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Views;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
            Routing.RegisterRoute(nameof(ListaRestaurantePage), typeof(ListaRestaurantePage));
            Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(PlaceOrderPage), typeof(PlaceOrderPage));
            Routing.RegisterRoute(nameof(OrderInfoPage), typeof(OrderInfoPage));
            Routing.RegisterRoute(nameof(ProductInOrderPage), typeof(ProductInOrderPage));
            Routing.RegisterRoute(nameof(UserDetailsPage), typeof(UserDetailsPage));
            Routing.RegisterRoute(nameof(UserLocationPage), typeof(UserLocationPage));
            Routing.RegisterRoute(nameof(ConfirmEmailPage), typeof(ConfirmEmailPage));
            Routing.RegisterRoute(nameof(SetPasswordPage), typeof(SetPasswordPage));
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(GenerateTokenPage), typeof(GenerateTokenPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Task.Run(async () => await DependencyService.Get<IDataStore>().Init());
        }

    }
}

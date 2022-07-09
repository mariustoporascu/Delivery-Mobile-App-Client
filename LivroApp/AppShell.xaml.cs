using LivroApp.Views;
using Xamarin.Forms;

namespace LivroApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
            Routing.RegisterRoute(nameof(ListaCompaniiPage), typeof(ListaCompaniiPage));
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
            Routing.RegisterRoute(nameof(GoogleDriveViewerPage), typeof(GoogleDriveViewerPage));
            Routing.RegisterRoute(nameof(UserLocationsListPage), typeof(UserLocationsListPage));
            Routing.RegisterRoute(nameof(SelectLocationAndPaymentPage), typeof(SelectLocationAndPaymentPage));
        }

    }
}

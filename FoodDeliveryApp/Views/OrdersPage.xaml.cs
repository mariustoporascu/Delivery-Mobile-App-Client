using FoodDeliveryApp.ViewModels;

using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class OrdersPage : ContentPage
    {
        OrdersViewModel viewModel;
        public OrdersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrdersViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.isLoggedIn)
            {
                viewModel.IsLoggedIn = true;
                viewModel.LoadOrdersCommand.Execute(null);
                ItemsListView.ScrollTo(0, position: ScrollToPosition.Start);
            }
            else
                viewModel.IsLoggedIn = false;
        }
    }
}
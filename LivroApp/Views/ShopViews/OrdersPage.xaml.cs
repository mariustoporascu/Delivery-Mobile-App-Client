using LivroApp.ViewModels.ShopVModels;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class OrdersPage : ContentPage
    {
        OrdersViewModel viewModel;
        public OrdersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrdersViewModel();
            MessagingCenter.Subscribe<OrderInfoPage>(this, "RefreshOrders", (sender) => viewModel.LoadOrdersCommand.Execute(null));

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.IsLoggedIn)
            {
                viewModel.IsLoggedIn = true;
                //viewModel.LoadOrdersCommand.Execute(null);
            }
            else
            {
                viewModel.IsLoggedIn = false;
                viewModel.IsBusy = false;
            }

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.SelectedTime = e.NewDate;
            viewModel.FilterBy(e.NewDate);
        }

    }
}
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
            MessagingCenter.Subscribe<OrderInfoPage>(this, "RefreshOrders", (sender) => viewModel.LoadAllItems.Execute(null));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoggedIn = App.IsLoggedIn;
        }
        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.SelectedTime = e.NewDate;
            viewModel.FilterBy(e.NewDate);
        }

    }
}
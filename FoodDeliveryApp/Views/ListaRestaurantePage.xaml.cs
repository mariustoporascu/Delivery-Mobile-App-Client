using FoodDeliveryApp.ViewModels;

using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class ListaRestaurantePage : ContentPage
    {
        ListaRestauranteViewModel viewModel;
        public ListaRestaurantePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListaRestauranteViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
            ItemsListView.ScrollTo(0, position: ScrollToPosition.Start);

        }
    }
}
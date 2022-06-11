using FoodDeliveryApp.ViewModels;

using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class CategoryPage : ContentPage
    {
        CategViewModel viewModel;

        public CategoryPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CategViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);

        }
    }
}
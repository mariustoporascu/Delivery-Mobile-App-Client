using FoodDeliveryApp.ViewModels;
using System;

using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class CosContentPage : ContentPage
    {
        CosContentViewModel viewModel;

        public CosContentPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CosContentViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.IsLoggedIn = App.isLoggedIn;
            viewModel.LoadItemsCommand.Execute(null);
            ItemsListView.ScrollTo(0, position: ScrollToPosition.Start);

        }
        private async void GoToFinalizeOrder(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PlaceOrderPage());
        }
    }
}
using FoodDeliveryApp.Constants;
using FoodDeliveryApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
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

            viewModel.IsLoggedIn = App.IsLoggedIn;
            viewModel.LoadItemsCommand.Execute(null);
            ItemsListView.ScrollTo(0, position: ScrollToPosition.Start);
            viewModel.GetTime();

        }
        private async void GoToFinalizeOrder(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SelectLocationAndPaymentPage());
        }


    }
}
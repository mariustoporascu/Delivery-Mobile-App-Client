using LivroApp.ViewModels.ShopVModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class PlaceOrderPage : ContentPage
    {
        PlaceOrderViewModel vm;
        public PlaceOrderPage(int locationId, string paymentMethod)
        {
            InitializeComponent();
            BindingContext = vm = new PlaceOrderViewModel(locationId, paymentMethod);
            vm.SuccessDelegate += OnPlaceOrder;
            vm.FailedDelegate += OnPlaceOrderFailed;
        }
        private void OnPlaceOrderApple(object sender, EventArgs e)
        {
            try
            {
                this.DisplayToastAsync("Comanda a fost plasata", 2300);
            }
            catch (Exception) { }
            Navigation.PopModalAsync(true);
        }
        private async void OnPlaceOrder(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Comanda a fost plasata", 1500);
            }
            catch (Exception) { }
            await Shell.Current.Navigation.PopToRootAsync();
        }
        private async void OnPlaceOrderFailed(object sender, EventArgs e)
        {
            try
            {
                await DisplayAlert("Eroare", "Comanda nu s-a putut plasa, va rugam sa reincercati", "OK");
            }
            catch (Exception) { }
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }
    }
}
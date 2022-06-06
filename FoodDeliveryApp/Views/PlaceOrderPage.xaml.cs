using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class PlaceOrderPage : ContentPage
    {
        PlaceOrderViewModel vm;
        public PlaceOrderPage(int locationId, string paymentMethod)
        {
            InitializeComponent();
            BindingContext = vm = new PlaceOrderViewModel(locationId, paymentMethod);
            if (Device.RuntimePlatform == Device.iOS)
                vm.OnPlaceOrder += OnPlaceOrderApple;
            else
            {
                vm.OnPlaceOrder += OnPlaceOrder;
                vm.OnPlaceOrderFailed += OnPlaceOrderFailed;

            }

        }
        private void OnPlaceOrderApple(object sender, EventArgs e)
        {
            try
            {
                this.DisplayToastAsync("Comanda a fost plasata", 2300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Navigation.PopModalAsync(true);
        }
        private async void OnPlaceOrder(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Comanda a fost plasata", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Shell.Current.Navigation.PopToRootAsync();
        }
        private async void OnPlaceOrderFailed(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayAlert("Eroare", "Comanda nu s-a putut plasa, va rugam sa reincercati", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
    }
}
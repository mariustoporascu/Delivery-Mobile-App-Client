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
        public PlaceOrderPage()
        {
            InitializeComponent();
            BindingContext = vm = new PlaceOrderViewModel();
            if (Device.RuntimePlatform == Device.iOS)
                vm.OnPlaceOrder += OnPlaceOrderApple;
            else
                vm.OnPlaceOrder += OnPlaceOrder;
            if (App.isLoggedIn)
                vm.HasValidProfile = App.userInfo.CompleteProfile && App.userInfo.CompleteLocation;
            else
                vm.HasValidProfile = false;
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
            Navigation.PopModalAsync(false).ConfigureAwait(false);
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
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
    }
}
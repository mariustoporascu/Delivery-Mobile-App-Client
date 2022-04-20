using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceOrderPage : ContentPage
    {
        public PlaceOrderPage()
        {
            InitializeComponent();
            var vm = new PlaceOrderViewModel();
            vm.OnPlaceOrder += OnPlaceOrder;
            BindingContext = vm;
        }

        private async void OnPlaceOrder(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(false);
        }
    }
}
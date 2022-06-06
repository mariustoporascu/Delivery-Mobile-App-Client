using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class SelectLocationAndPaymentPage : ContentPage
    {
        SelectLocationAndPaymentViewModel viewModel;
        public SelectLocationAndPaymentPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SelectLocationAndPaymentViewModel();
        }

        async void ClickedGoToFinalize(object sender, EventArgs args)
        {
            if (viewModel.SelLocation > 0 && !string.IsNullOrWhiteSpace(viewModel.SelMethod))
                await Navigation.PushModalAsync(new PlaceOrderPage(viewModel.SelLocation, viewModel.SelMethod));
            else
                await DisplayAlert("Eroare", "Nu ai selectat o modalitate de plata si/sau o locatie de livrare.", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
        private async void PaymentMethods_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            try
            {
                if (SelectorPaymentMethods.ItemsSource[SelectorPaymentMethods.SelectedIndex].ToString() == viewModel.SelMethod)
                    return;
                viewModel.SelMethod = SelectorPaymentMethods.ItemsSource[SelectorPaymentMethods.SelectedIndex].ToString();
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
        private async void Locations_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //$"{loc.LocationName},{loc.BuildingInfo},{loc.Street},{loc.City}"
            try
            {
                var selLocationString = SelectorLocations.ItemsSource[SelectorLocations.SelectedIndex].ToString();
                var location = App.UserInfo.Locations.Find(loc => $"{loc.LocationName},{loc.BuildingInfo},{loc.Street},{loc.City}" == selLocationString);
                if (location != null)
                    viewModel.SelLocation = location.LocationId;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
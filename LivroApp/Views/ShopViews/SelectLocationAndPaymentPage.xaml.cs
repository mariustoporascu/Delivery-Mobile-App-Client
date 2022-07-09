using LivroApp.ViewModels.ShopVModels;
using System;
using System.Diagnostics;

using Xamarin.Forms;

namespace LivroApp.Views
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
            await Navigation.PopModalAsync(true);
        }
        private void PaymentMethods_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (SelectorPaymentMethods.ItemsSource[SelectorPaymentMethods.SelectedIndex].ToString() == viewModel.SelMethod)
                    return;
                viewModel.SelMethod = SelectorPaymentMethods.ItemsSource[SelectorPaymentMethods.SelectedIndex].ToString();
            }

            catch (Exception) { }


        }
        private void Locations_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                var selLocationString = SelectorLocations.ItemsSource[SelectorLocations.SelectedIndex].ToString();
                var location = App.UserInfo.Locations.Find(loc => $"{App.UserInfo.Locations.IndexOf(loc) + 1} - {loc.LocationName}, {loc.BuildingInfo}, {loc.Street}, {loc.City}" == selLocationString);
                if (location != null)
                    viewModel.SelLocation = location.LocationId;
            }

            catch (Exception) { }
        }
    }
}
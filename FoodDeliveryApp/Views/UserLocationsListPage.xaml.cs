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
    public partial class UserLocationsListPage : ContentPage
    {
        UserLocationsListViewModel viewModel;
        public UserLocationsListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new UserLocationsListViewModel();
            viewModel.EditLocation += EditLocationClicked;
            viewModel.DeleteLocationFailed += DeleteLocationFailed;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.RefreshLocations();
        }
        private async void EditLocationClicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.LocationId > 0)
                    await Navigation.PushModalAsync(new UserLocationPage(viewModel.LocationId));
                else
                    await this.DisplayAlert("Eroare", "Nu se poate edita locatia aceasta", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void DeleteLocationFailed(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayAlert("Eroare", "Stergerea locatiei a intampinat o eroare.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void AddNewClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.UserInfo.Locations == null || App.UserInfo.Locations.Count < 3)
                    await Navigation.PushModalAsync(new UserLocationPage(0));
                else
                    await this.DisplayAlert("Eroare", "Nu mai poti adauga locatii, este posibil doar 3.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void DismissClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
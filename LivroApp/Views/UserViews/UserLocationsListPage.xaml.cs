﻿using LivroApp.ViewModels.UserVModels;
using System;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class UserLocationsListPage : ContentPage
    {
        UserLocationsListViewModel viewModel;
        public UserLocationsListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new UserLocationsListViewModel();
            viewModel.EditLocation += EditLocationClicked;
            viewModel.FailedDelegate += DeleteLocationFailed;
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
                    await DisplayAlert("Eroare", "Nu se poate edita locatia aceasta", "OK");
            }
            catch (Exception) { }
        }
        private async void DeleteLocationFailed(object sender, EventArgs e)
        {
            try
            {
                await DisplayAlert("Eroare", "Stergerea locatiei a intampinat o eroare.", "OK");
            }
            catch (Exception) { }
        }
        private async void AddNewClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.UserInfo.Locations == null || App.UserInfo.Locations.Count < 3)
                    await Navigation.PushModalAsync(new UserLocationPage(0));
                else
                    await DisplayAlert("Eroare", "Nu mai poti adauga locatii, este posibil doar 3.", "OK");
            }
            catch (Exception) { }
        }
        private async void DismissClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync(true);
            }
            catch (Exception) { }
        }
    }
}
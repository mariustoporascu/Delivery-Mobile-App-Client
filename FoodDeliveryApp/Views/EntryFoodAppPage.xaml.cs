using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class EntryFoodAppPage : ContentPage
    {

        EntryFoodAppViewModel viewModel;
        public EntryFoodAppPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EntryFoodAppViewModel();
            viewModel.Supermarketpush += OnSupermarket;
        }
        public async void OnSupermarket(object sender, EventArgs e)
        {
            try
            {
                await DisplayAlert("Eroare", "Inca nu este disponibila sectiunea de supermarket-uri. V-a fi adaugata in curand.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
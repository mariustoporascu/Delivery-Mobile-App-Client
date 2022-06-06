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
            viewModel.NotOpen += NotOpen;
        }
        private async void NotOpen(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayAlert("Info", "Pastreaza-ti pofta, se va deschide in curand.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class ListaRestaurantePage : ContentPage
    {
        ListaRestauranteViewModel viewModel;
        public ListaRestaurantePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListaRestauranteViewModel();
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
        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
            //ItemsListView.ScrollTo(0, position: ScrollToPosition.Start);

        }
    }
}
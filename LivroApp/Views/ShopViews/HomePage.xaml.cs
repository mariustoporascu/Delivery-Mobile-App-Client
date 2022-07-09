using LivroApp.ViewModels.ShopVModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class HomePage : ContentPage
    {

        readonly HomeViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomeViewModel();
            viewModel.FailedDelegate += NotOpen;
        }
        private async void NotOpen(object sender, EventArgs e)
        {
            try
            {
                await DisplayAlert("Info", "Indisponibil momentan, se va deschide in curand.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
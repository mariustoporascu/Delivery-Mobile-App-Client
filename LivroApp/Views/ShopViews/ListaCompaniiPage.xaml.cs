using LivroApp.ViewModels.ShopVModels;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class ListaCompaniiPage : ContentPage
    {
        ListaCompaniiViewModel viewModel;
        public ListaCompaniiPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListaCompaniiViewModel();
            viewModel.FailedDelegate += NotOpen;
        }
        private async void NotOpen(object sender, EventArgs e)
        {
            try
            {
                await DisplayAlert("Info", "Indisponibil momentan, se va deschide in curand.", "OK");
            }
            catch (Exception) { }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadAllItems.Execute(null);
            viewModel.Title = viewModel.DataStore.GetTipCompanii().First(tip => tip.TipCompanieId == viewModel.RefId).Name;
        }
    }
}
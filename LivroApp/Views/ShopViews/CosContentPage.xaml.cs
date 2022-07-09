using LivroApp.ViewModels.ShopVModels;
using System;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class CosContentPage : ContentPage
    {
        CosContentViewModel viewModel;
        public CosContentPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CosContentViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadAllItems.Execute(null);
            viewModel.GetTime();

        }
        private async void GoToFinalizeOrder(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SelectLocationAndPaymentPage());
        }


    }
}
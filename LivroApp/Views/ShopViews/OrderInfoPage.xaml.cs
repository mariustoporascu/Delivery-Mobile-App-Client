using LivroApp.ViewModels.ShopVModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class OrderInfoPage : ContentPage
    {
        OrderInfoViewModel viewModel;
        public OrderInfoPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrderInfoViewModel();
            viewModel.GetRatDriver += GetDriverRat;
            viewModel.GetRatRest += GetRestRat;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoggedIn = App.IsLoggedIn;
            if (!App.IsLoggedIn)
                await Shell.Current.Navigation.PopToRootAsync();
            else
                viewModel.Title = $"Detalii Comanda {viewModel.OrderId}";
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var prompt = await DisplayAlert("Confirmati", "Apasand acest buton confirmati ca ati acceptat timpul estimat de pregatire.", "OK", "Cancel");
            if (prompt)
            {
                try
                {
                    if (await viewModel.ConfirmOrder(true))

                        await DisplayAlert("Succes", "Alegerea ta a fost transmisa.", "OK");
                    else
                        await DisplayAlert("Eroare", "Alegerea ta nu a fost transmisa.", "OK");
                    MessagingCenter.Send<OrderInfoPage>(this, "RefreshOrders");
                }
                catch (Exception) { }
            }

        }

        private async void Button_Clicked_1(object sender, System.EventArgs e)
        {
            var prompt = await DisplayAlert("Confirmati", "Apasand acest buton confirmati si faptul ca renuntati la comanda.", "OK", "Cancel");
            if (prompt)
            {
                try
                {
                    if (await viewModel.ConfirmOrder(false))

                        await DisplayAlert("Succes", "Alegerea ta a fost transmisa.", "OK");
                    else
                        await DisplayAlert("Eroare", "Alegerea ta nu a fost transmisa.", "OK");
                    MessagingCenter.Send<OrderInfoPage>(this, "RefreshOrders");
                }
                catch (Exception) { }
            }

        }
        private async void GetDriverRat(object sender, System.EventArgs e)
        {
            try
            {
                var prompt = await DisplayAlert("Confirmati", $"Ati selectat {Rating.SelectedStarValue.ToString()} stelut{(Rating.SelectedStarValue == 1 ? 'a' : 'e')}. Confirmati ca selectia este in regula.", "OK", "Cancel");
                if (prompt)
                {
                    if (await viewModel.GiveDriverRating(Rating.SelectedStarValue))
                        await DisplayAlert("Succes", "Alegerea ta a fost transmisa.", "OK");
                    else
                        await DisplayAlert("Eroare", "Alegerea ta nu a fost transmisa.", "OK");
                    MessagingCenter.Send<OrderInfoPage>(this, "RefreshOrders");

                }
            }
            catch (Exception) { }
        }
        private async void GetRestRat(object sender, EventArgs e)
        {
            try
            {
                var prompt = await DisplayAlert("Confirmati", $"Ati selectat {Rating2.SelectedStarValue.ToString()} stelut{(Rating2.SelectedStarValue == 1 ? 'a' : 'e')}. Confirmati ca selectia este in regula.", "OK", "Cancel");
                if (prompt)
                {
                    if (await viewModel.GiveRestaurantRating(Rating2.SelectedStarValue))
                        await DisplayAlert("Succes", "Alegerea ta a fost transmisa.", "OK");
                    else
                        await DisplayAlert("Eroare", "Alegerea ta nu a fost transmisa.", "OK");
                    MessagingCenter.Send<OrderInfoPage>(this, "RefreshOrders");

                }
            }
            catch (Exception) { }
        }
    }
}
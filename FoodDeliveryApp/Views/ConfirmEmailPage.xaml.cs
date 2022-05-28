using FoodDeliveryApp.ViewModels;

using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class ConfirmEmailPage : ContentPage
    {
        ConfirmEmailViewModel viewModel;
        public ConfirmEmailPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ConfirmEmailViewModel();
            viewModel.OnSignIn += OnSignIn;
            viewModel.OnSignInFailed += OnSignInFailed;

        }


        private async void RedirSignUp(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Emailul a fost confirmat cu succes.", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }

        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Emailul nu s-a putut confirma.", "OK");
        }
    }
}
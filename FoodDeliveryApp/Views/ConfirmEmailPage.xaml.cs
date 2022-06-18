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

        async void ConfirmClicked(object sender, EventArgs args)
        {
            if (!TokenEntry.IsValid)
            {
                Token.TextColor = Color.Red;
                await DisplayAlert("Eroare", "Tokenul un are numarul de caractere potrivit.", "OK");
                return;
            }
            Token.TextColor = Color.Black;
            viewModel.ConfirmEmail.Execute(null);
        }
        private async void RedirSignUp(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Emailul a fost confirmat cu succes.", 1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(true);
        }

        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Emailul nu s-a putut confirma.", "OK");
        }
        private void CheckFieldToken(object sender, TextChangedEventArgs e)
        {
            if (!TokenEntry.IsValid)
            {
                Token.TextColor = Color.Red;
                return;
            }
            Token.TextColor = Color.Black;
        }
    }
}
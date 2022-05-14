using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        RegisterViewModel viewModel;
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new RegisterViewModel();
            if (Device.RuntimePlatform == Device.iOS)
            {
                viewModel.OnSignIn += OnSignInApple;
                viewModel.OnSignUpWeb += OnSignUpWebApple;
            }
            else
            {
                viewModel.OnSignIn += OnSignIn;
                viewModel.OnSignUpWeb += OnSignUpWeb;
            }

            viewModel.OnSignInFailed += OnSignInFailed;
            BindingContext = viewModel;
            if (!viewModel._loggedIn)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else
                    OnSignIn(this, new EventArgs());
            }

        }
        private void CheckFieldMail(object sender, TextChangedEventArgs e)
        {
            if (!UsernameEntry.IsValid)
            {
                Email.TextColor = Color.Red;
                return;
            }
            Email.TextColor = Color.Black;
        }
        private void CheckFieldPass(object sender, TextChangedEventArgs e)
        {
            if (!PasswordEntry.IsValid)
            {
                Password.TextColor = Color.Red;
                return;
            }
            Password.TextColor = Color.Black;
        }
        private async void CheckFields(object sender, EventArgs e)
        {
            if (!UsernameEntry.IsValid && !PasswordEntry.IsValid)
            {
                Email.TextColor = Color.Red;
                Password.TextColor = Color.Red;
                await DisplayAlert("Eroare", "Email si parola invalide", "OK");
                return;
            }
            if (!UsernameEntry.IsValid)
            {
                Email.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Email invalid", "OK");
                return;
            }
            if (!PasswordEntry.IsValid)
            {
                Password.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Parola trebuie sa contina minimum 6 caractere", "OK");
                return;
            }
            Email.TextColor = Color.Black;
            Password.TextColor = Color.Black;

            viewModel.SignUpWebCommand.Execute(null);
        }
        private void OnSignInApple(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
        private void OnSignUpWebApple(object sender, EventArgs e)
        {
            try
            {
                this.DisplayToastAsync("Contul tau a fost creat cu succes", 2300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
        private async void OnSignUpWeb(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Contul tau a fost creat cu succes", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }


        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Inregistrare esuata", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }

        private void FBBtn_Pressed(object sender, EventArgs e)
        {
            FBBtn.Scale = 1.1;
        }

        private void FBBtn_Released(object sender, EventArgs e)
        {
            FBBtn.Scale = 1.0;

        }
        private void GoogleBtn_Pressed(object sender, EventArgs e)
        {
            GoogleBtn.Scale = 1.1;
        }

        private void GoogleBtn_Released(object sender, EventArgs e)
        {
            GoogleBtn.Scale = 1.0;

        }
        private void AppleBtn_Pressed(object sender, EventArgs e)
        {
            AppleBtn.Scale = 1.1;
        }

        private void AppleBtn_Released(object sender, EventArgs e)
        {
            AppleBtn.Scale = 1.0;

        }
    }
}
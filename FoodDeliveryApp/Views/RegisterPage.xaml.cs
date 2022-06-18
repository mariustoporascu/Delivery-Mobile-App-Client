using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
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
            /*if (Device.RuntimePlatform == Device.iOS)
            {
                viewModel.OnSignIn += OnSignInApple;
                viewModel.OnSignUpWeb += OnSignUpWebApple;
            }
            else
            {*/
            viewModel.OnSignIn += OnSignIn;
            viewModel.OnSignUpWeb += OnSignUpWeb;
            //}

            viewModel.OnSignInFailed += OnSignInFailed;
            BindingContext = viewModel;
            if (!viewModel._loggedIn)
            {
                /*if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else*/
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
        private void CheckFieldNumeComplet(object sender, TextChangedEventArgs e)
        {

            try
            {
                if (FullName.Text.Split(null).Count() < 2)
                {
                    FullNameEntry.IsNotValid = true;
                    FullNameEntry.IsValid = false;
                }
                if (!FullNameEntry.IsValid)
                {
                    FullName.TextColor = Color.Red;
                    return;
                }
                FullName.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
        private void CheckFieldConfirmPass(object sender, TextChangedEventArgs e)
        {
            if (!ConfirmPasswordEntry.IsValid)
            {
                ConfirmPassword.TextColor = Color.Red;
                return;
            }
            if (ConfirmPassword.Text != Password.Text)
            {
                ConfirmPassword.TextColor = Color.Red;
                return;
            }
            ConfirmPassword.TextColor = Color.Black;
        }
        private async void CheckFields(object sender, EventArgs e)
        {

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
            if (ConfirmPassword.Text != Password.Text)
            {
                ConfirmPassword.TextColor = Color.Red;
                await DisplayAlert("Eroare", "Parolele nu coincid", "OK");
                return;
            }
            if (!FullNameEntry.IsValid)
            {
                FullName.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Introduceti numele complet(ex: nume, prenume)", "OK");
                return;
            }
            Email.TextColor = Color.Black;
            Password.TextColor = Color.Black;
            ConfirmPassword.TextColor = Color.Black;
            FullName.TextColor = Color.Black;

            viewModel.SignUpWebCommand.Execute(null);
        }
        private void OnSignInApple(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
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
            Navigation.PopModalAsync(true);
        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
        private async void OnSignUpWeb(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Contul tau a fost creat cu succes", 1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(true);
        }


        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Inregistrare esuata", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }

    }
}
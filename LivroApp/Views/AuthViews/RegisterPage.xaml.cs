using LivroApp.ViewModels.AuthVModels;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        RegisterViewModel viewModel;
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new RegisterViewModel();
            viewModel.SuccessDelegate += OnSignUpWeb;
            viewModel.FailedDelegate += OnSignInFailed;
            BindingContext = viewModel;
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
            catch (Exception) { }
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

            viewModel.SignUpLivro.Execute(null);
        }
        private async void OnSignUpWeb(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Contul tau a fost creat cu succes", 1500);
            }
            catch (Exception) { }
            await Navigation.PopModalAsync(true);
        }
        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Inregistrare esuata", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }

    }
}
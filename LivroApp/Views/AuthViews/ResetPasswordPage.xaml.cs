using LivroApp.ViewModels.AuthVModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class ResetPasswordPage : ContentPage
    {
        ResetPasswordViewModel viewModel;
        public ResetPasswordPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ResetPasswordViewModel();
            viewModel.SuccessDelegate += ResetPasswordSuc;
            viewModel.FailedDelegate += ResetPasswordFailed;
            viewModel.CoolDown += CoolDown;
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
        private void CheckFieldConfirmPass(object sender, TextChangedEventArgs e)
        {
            if (!ConfirmPasswordEntry.IsValid)
            {
                ConfirmPassword.TextColor = Color.Red;
                return;
            }
            if (ConfirmPassword.Text != NewPassword.Text)
            {
                ConfirmPassword.TextColor = Color.Red;
                return;
            }
            ConfirmPassword.TextColor = Color.Black;
        }
        private void CheckFieldNewPass(object sender, TextChangedEventArgs e)
        {
            if (!NewPasswordEntry.IsValid)
            {
                NewPassword.TextColor = Color.Red;
                return;
            }
            NewPassword.TextColor = Color.Black;
        }
        private async void CheckFields(object sender, EventArgs e)
        {

            if (!TokenEntry.IsValid)
            {
                Token.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Tokenul nu are numarul de caractere potrivit.", "OK");
                return;
            }
            if (!NewPasswordEntry.IsValid)
            {
                NewPassword.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Parola noua trebuie sa contina minimum 6 caractere", "OK");
                return;
            }
            if (ConfirmPassword.Text != NewPassword.Text)
            {
                ConfirmPassword.TextColor = Color.Red;
                await DisplayAlert("Eroare", "Parolele nu coincid", "OK");
                return;
            }

            Token.TextColor = Color.Black;
            NewPassword.TextColor = Color.Black;
            ConfirmPassword.TextColor = Color.Black;

            viewModel.ResetPassword.Execute(null);
        }
        private async void ResetPasswordSuc(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Parola a fost schimbata.", 1500);
            }
            catch (Exception) { }
            await Navigation.PopModalAsync(true);
        }


        private async void ResetPasswordFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Incercare esuata", "OK");
        }
        private async void CoolDown(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Ai atins numarul maxim de incercari, genereaza alt cod.", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }
    }
}
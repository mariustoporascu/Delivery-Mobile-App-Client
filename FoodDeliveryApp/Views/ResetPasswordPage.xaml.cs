using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class ResetPasswordPage : ContentPage
    {
        ResetPasswordViewModel viewModel;
        public ResetPasswordPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ResetPasswordViewModel();
            viewModel.ResetPasswordSuc += ResetPasswordSuc;
            viewModel.ResetPasswordFailed += ResetPasswordFailed;
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(true);
        }


        private async void ResetPasswordFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Incercare esuata", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
    }
}
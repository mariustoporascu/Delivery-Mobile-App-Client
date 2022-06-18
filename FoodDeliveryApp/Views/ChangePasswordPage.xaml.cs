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
    public partial class ChangePasswordPage : ContentPage
    {
        ChangePasswordViewModel viewModel;
        public ChangePasswordPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ChangePasswordViewModel();
            viewModel.ChangePasswordSuc += ChangePasswordSuc;
            viewModel.ChangePasswordFailed += ChangePasswordFailed;
        }
        private void CheckFieldPass(object sender, TextChangedEventArgs e)
        {
            if (!PasswordEntry.IsValid)
            {
                Password.TextColor = Color.Red;
                return;
            }
            if (Password.Text != App.UserInfo.Password)
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

            if (!PasswordEntry.IsValid)
            {
                Password.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Parola curenta este gresita.", "OK");
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

            Password.TextColor = Color.Black;
            NewPassword.TextColor = Color.Black;
            ConfirmPassword.TextColor = Color.Black;

            viewModel.ChangePassword.Execute(null);
        }
        private async void ChangePasswordSuc(object sender, EventArgs e)
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


        private async void ChangePasswordFailed(object sender, EventArgs e)
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
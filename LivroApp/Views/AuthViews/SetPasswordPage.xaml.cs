using LivroApp.ViewModels.AuthVModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class SetPasswordPage : ContentPage
    {
        SetPasswordViewModel viewModel;
        public SetPasswordPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SetPasswordViewModel();
            viewModel.OnSetPasswordSuc += SetPasswordSuc;
            viewModel.OnSetPasswordFailed += OnSetPasswordFailed;
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

            Password.TextColor = Color.Black;
            ConfirmPassword.TextColor = Color.Black;

            viewModel.SetPassword.Execute(null);
        }
        private async void SetPasswordSuc(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Parola a fost setata.", 1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(true);
        }


        private async void OnSetPasswordFailed(object sender, EventArgs e)
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
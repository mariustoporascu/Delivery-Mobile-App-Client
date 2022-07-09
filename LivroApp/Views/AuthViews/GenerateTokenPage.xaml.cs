using LivroApp.ViewModels.AuthVModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class GenerateTokenPage : ContentPage
    {
        GenerateTokenViewModel viewModel;
        public GenerateTokenPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new GenerateTokenViewModel();
            viewModel.SuccessDelegate += OnGenerateSucces;
            viewModel.FailedDelegate += OnGenerateFailed;
            viewModel.HasCode += HasCode;
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
        private async void CheckFields(object sender, EventArgs e)
        {

            if (!UsernameEntry.IsValid)
            {
                Email.TextColor = Color.Red;

                await DisplayAlert("Eroare", "Email invalid", "OK");
                return;
            }

            Email.TextColor = Color.Black;

            viewModel.GenerateToken.Execute(null);
        }

        async void HasCode(object sender, EventArgs args)
        {
            try
            {
                await Shell.Current.DisplayToastAsync($"Ai generat deja un cod valid, se poate genera altul dupa 15 minute de la generarea anterioara.", 1500);
            }
            catch (Exception) { }
            await Navigation.PushModalAsync(new ResetPasswordPage());
        }
        async void OnGenerateSucces(object sender, EventArgs args)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Codul a fost trimis catre tine.", 1500);
            }
            catch (Exception) { }
            await Navigation.PushModalAsync(new ResetPasswordPage());
        }
        async void OnGenerateFailed(object sender, EventArgs args)
        {
            try
            {
                await DisplayAlert("Eroare", "Codul nu a putut fi generat. Reincearca.", "OK");
            }
            catch (Exception) { }
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
        async void AmCodClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PushModalAsync(new ResetPasswordPage());
        }
    }
}
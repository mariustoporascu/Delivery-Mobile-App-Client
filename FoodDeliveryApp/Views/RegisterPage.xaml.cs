using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        RegisterViewModel viewModel;
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new RegisterViewModel();
            viewModel.OnSignIn += OnSignIn;
            viewModel.OnSignInFailed += OnSignInFailed;
            viewModel.OnSignUpWeb += OnSignUpWeb;
            BindingContext = viewModel;
            if (!viewModel._loggedIn)
            {
                OnSignIn(this, default(EventArgs));
            }

        }
        private async void CheckFields(object sender, EventArgs e)
        {
            if (!UsernameEntry.IsValid && !PasswordEntry.IsValid)
            {
                await DisplayAlert("Eroare", "Email si parola invalide", "OK");
                return;
            }
            if (!UsernameEntry.IsValid)
            {
                await DisplayAlert("Eroare", "Email invalid", "OK");
                return;
            }
            if (!PasswordEntry.IsValid)
            {
                await DisplayAlert("Eroare", "Parola trebuie sa contina minimum 6 caractere", "OK");
                return;
            }
            viewModel.SignUpWebCommand.Execute(null);
        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void OnSignUpWeb(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Inregistrare esuata", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(false);
        }
    }
}
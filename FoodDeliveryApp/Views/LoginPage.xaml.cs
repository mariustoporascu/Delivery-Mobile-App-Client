using FoodDeliveryApp.Constants;
using FoodDeliveryApp.ViewModels;
using OneSignalSDK.Xamarin;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new LoginViewModel();
            if (Device.RuntimePlatform == Device.iOS)
                viewModel.OnSignIn += OnSignInApple;
            else
                viewModel.OnSignIn += OnSignIn;
            viewModel.OnSignInFailed += OnSignInFailed;
            viewModel.RequireConfirmEmail += RequireConfirmEmail;

            if (App.IsLoggedIn)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else
                    OnSignIn(this, new EventArgs());
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.RuntimePlatform == Device.Android)
            {
                if (string.IsNullOrWhiteSpace(App.FirebaseUserToken))
                {
                    App.FirebaseUserToken = OneSignal.Default.DeviceState.userId;
                    try
                    {
                        SecureStorage.SetAsync(App.FBToken, App.FirebaseUserToken).Wait();

                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (App.IsLoggedIn)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else
                    OnSignIn(this, new EventArgs());
            }

        }
        private async void TermeniClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new GoogleDriveViewerPage(ServerConstants.Termeni));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void GDPRclicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new GoogleDriveViewerPage(ServerConstants.Gdpr));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
        private void OnSignInApple(object sender, EventArgs e)
        {
            try
            {
                this.DisplayToastAsync("Ai fost autentificat.", 2300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Navigation.PopModalAsync(true);
        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Ai fost autentificat.", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Navigation.PopModalAsync(true);
        }

        private async void PasswordForgotClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new GenerateTokenPage());
        }
        private async void RequireConfirmEmail(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ConfirmEmailPage());
        }

        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Autentificare esuata", "OK");
        }
    }
}
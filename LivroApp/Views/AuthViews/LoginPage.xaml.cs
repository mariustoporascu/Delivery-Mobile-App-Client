using LivroApp.Constants;
using LivroApp.ViewModels.AuthVModels;
using OneSignalSDK.Xamarin;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new LoginViewModel();
            viewModel.SuccessDelegate += OnSignIn;
            viewModel.FailedDelegate += OnSignInFailed;
            viewModel.RequireConfirmEmail += RequireConfirmEmail;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                viewModel.FacebookLoginEnabled = await viewModel.AuthController.FbLoginEnabled();

            }
            catch (Exception) { }
            if (string.IsNullOrWhiteSpace(App.FirebaseUserToken))
            {
                App.FirebaseUserToken = OneSignal.Default.DeviceState.userId;
                try
                {
                    SecureStorage.SetAsync(App.FIREBASE_TOKEN, App.FirebaseUserToken).Wait();
                }
                catch (Exception) { }
            }
            if (App.IsLoggedIn)
            {
                OnSignIn(this, new EventArgs());
            }

        }
        private async void TermeniClicked(object sender, EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    using (var httpclient = new HttpClient())
                    {
                        var stream = await httpclient.GetStreamAsync(ServerConstants.Termeni);
                        using (var memStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memStream);
                            await CrossXamarinFormsSaveOpenPDFPackage.Current
                                .SaveAndView("TermeniLivro.pdf", "application/pdf", memStream, PDFOpenContext.InApp);
                        }
                    }
                }
                else
                    await Navigation.PushModalAsync(new GoogleDriveViewerPage(ServerConstants.Termeni));

            }
            catch (Exception) { }
        }
        private async void GDPRclicked(object sender, EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    using (var httpclient = new HttpClient())
                    {
                        var stream = await httpclient.GetStreamAsync(ServerConstants.Gdpr);
                        using (var memStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memStream);
                            await CrossXamarinFormsSaveOpenPDFPackage.Current
                                .SaveAndView("GDPRLivro.pdf", "application/pdf", memStream, PDFOpenContext.InApp);
                        }
                    }
                }
                else
                    await Navigation.PushModalAsync(new GoogleDriveViewerPage(ServerConstants.Gdpr));

            }
            catch (Exception) { }
        }
        private async void RedirSignUp(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void OnSignIn(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Ai fost autentificat.", 1500);
            }
            catch (Exception) { }
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
using LivroApp.Constants;
using LivroApp.ViewModels.UserVModels;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class UserProfilePage : ContentPage
    {
        UserProfileViewModel viewModel;
        public UserProfilePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new UserProfileViewModel();
            viewModel.SuccessDelegate += OnDeleteAcc;
            viewModel.FailedDelegate += DeleteAccFailed;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoggedIn = App.IsLoggedIn;
            await viewModel.RefreshProfile();
        }
        private async void RedirSignIn(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
        private async void EditLocation(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new UserLocationsListPage());
            }
            catch (Exception) { }
        }
        private async void EditInfo(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new UserDetailsPage());
            }
            catch (Exception) { }
        }
        private async void SetPassword(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new SetPasswordPage());
            }
            catch (Exception) { }
        }
        private async void ChangePassword(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new ChangePasswordPage());

            }
            catch (Exception) { }
        }
        private void AskHelpClicked(object sender, EventArgs e)
        {
            Navigation.ShowPopup(new AskHelpPopUp());
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
        private async void IntrebariClicked(object sender, EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    using (var httpclient = new HttpClient())
                    {
                        var stream = await httpclient.GetStreamAsync(ServerConstants.Intrebari);
                        using (var memStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memStream);
                            await CrossXamarinFormsSaveOpenPDFPackage.Current
                                .SaveAndView("IntrebariLivro.pdf", "application/pdf", memStream, PDFOpenContext.InApp);
                        }
                    }
                }
                else
                    await Navigation.PushModalAsync(new GoogleDriveViewerPage(ServerConstants.Intrebari));

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
        private async void OnDeleteAcc(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Contul a fost sters.", 1500);
            }
            catch (Exception) { }
        }
        private async void DeleteAccFailed(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Contul nu s-a putut sterge.", 1500);
            }
            catch (Exception) { }
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var orders = await viewModel.DataStore.GetServerOrders(viewModel.Email);
                if (orders.Count > 0 && orders.Any(or => or.Status != "Livrata" && or.Status != "Anulata" && or.Status != "Refuzata"))
                {
                    await DisplayAlert("Eroare", "Nu puteti sterge contul pana nu se onoreaza comenzile nefinalizate.", "OK");
                    return;
                }
                var prompt = await DisplayAlert("Confirmati", "Apasand acest buton confirmati ca doriti sa stergeti contul definitiv.", "OK", "Cancel");
                if (prompt)
                {

                    viewModel.DeleteProfile.Execute(null);
                }
            }
            catch (Exception) { }
        }
        private async void Deconectare_clicked(object sender, EventArgs e)
        {
            var prompt = await DisplayAlert("Confirmati", "Esti sigur ca vrei sa te deloghezi?", "OK", "Cancel");
            if (prompt)
            {
                try
                {
                    viewModel.Logout.Execute(null);
                }
                catch (Exception) { }
            }
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            viewModel.IsBusy = false;
        }
    }
}
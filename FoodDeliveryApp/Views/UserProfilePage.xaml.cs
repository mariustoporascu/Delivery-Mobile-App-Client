using FoodDeliveryApp.Constants;
using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Views
{
    public partial class UserProfilePage : ContentPage
    {
        UserProfileViewModel viewModel;
        public UserProfilePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new UserProfileViewModel();
            viewModel.OnDeleteAcc += OnDeleteAcc;
            viewModel.DeleteAccFailed += DeleteAccFailed;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.IsBusy = true;
            viewModel.RefreshProfile();
            viewModel.IsBusy = false;

        }
        private async void RedirSignIn(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
        private async void EditLocation(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new UserLocationPage());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void EditInfo(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new UserDetailsPage());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void SetPassword(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new SetPasswordPage());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void ChangePassword(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new ChangePasswordPage());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
        private async void OnDeleteAcc(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Contul a fost sters.", 1300);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void DeleteAccFailed(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Contul nu s-a putut sterge.", 1300);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
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
                try
                {
                    viewModel.DeleteProfile.Execute(null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
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
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
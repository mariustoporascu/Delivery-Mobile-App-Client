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
        Geocoder geoCoder;
        public UserProfilePage()
        {
            InitializeComponent();
            geoCoder = new Geocoder();
            BindingContext = viewModel = new UserProfileViewModel();
            viewModel.OnUpdateProfile += OnUpdateProfile;
            if (!App.isLoggedIn)
            {
                RedirSignIn(this, new EventArgs());
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.RefreshProfile();
        }
        private async void RedirSignIn(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
        private async void OnUpdateProfile(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Profilul a fost actualizat.", 1300);
        }
        private void CheckFieldNumeComplet(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (NumeComplet.Text.Split(null).Count() < 2)
                {
                    NumeCompletEntry.IsNotValid = true;
                    NumeCompletEntry.IsValid = false;
                }
                if (!NumeCompletEntry.IsValid)
                {
                    NumeComplet.TextColor = Color.Red;
                    return;
                }
                NumeComplet.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private void CheckFieldNrTelefon(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(NrTelefon.Text, @"^\d+$"))
                {
                    NrTelefonEntry.IsNotValid = true;
                    NrTelefonEntry.IsValid = false;
                }
                if (!NrTelefonEntry.IsValid)
                {
                    NrTelefon.TextColor = Color.Red;
                    return;
                }
                NrTelefon.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private void CheckFieldCladireAp(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!CladireApEntry.IsValid)
                {
                    CladireAp.TextColor = Color.Red;
                    return;
                }
                CladireAp.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private async void CheckFieldNumeNrStrada(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!NumeNrStradaEntry.IsValid)
                {
                    NumeNrStrada.TextColor = Color.Red;
                    return;
                }
                if (!(await VerifyLocation()))
                {
                    NumeNrStrada.TextColor = Color.Red;
                    return;
                }
                Oras.TextColor = Color.Black;

                NumeNrStrada.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private async void CheckFieldOras(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!OrasEntry.IsValid)
                {
                    Oras.TextColor = Color.Red;
                    return;
                }
                if (!(await VerifyLocation()))
                {
                    Oras.TextColor = Color.Red;
                    return;
                }
                NumeNrStrada.TextColor = Color.Black;

                Oras.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private async Task<bool> IsProfileValid()
        {
            try
            {
                if (OrasEntry.IsValid && NumeNrStradaEntry.IsValid &&
                CladireApEntry.IsValid && NrTelefonEntry.IsValid &&
                NumeCompletEntry.IsValid && (await VerifyLocation()))
                    return true;
                return false;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;

            }
        }
        private async Task<bool> VerifyLocation()
        {
            try
            {
                if (OrasEntry.IsValid && NumeNrStradaEntry.IsValid)
                {

                    IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync(NumeNrStrada.Text + ", " + Oras.Text + ", Romania").ConfigureAwait(false);
                    if (aproxLocation.Count() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (await IsProfileValid())
                    viewModel.SaveProfile.Execute(null);
                else
                    await DisplayAlert("Eroare", "Datele profilului nu sunt complete.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
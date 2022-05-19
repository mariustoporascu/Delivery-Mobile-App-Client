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
            if (App.isLoggedIn)
            {
                viewModel.RefreshProfile();
                if (viewModel.CoordX != 0 && viewModel.CoordY != 0)
                {
                    try
                    {

                        if (AppMap.Pins.Count > 0)
                        {
                            Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label == "Adresa mea");
                            if (pinTo != null)
                                AppMap.Pins.Remove(pinTo);
                        }

                        Pin goToPin = new Pin()
                        {
                            Label = "Adresa mea",
                            Type = PinType.Place,
                            Position = new Position(viewModel.CoordX, viewModel.CoordY),

                        };
                        AppMap.Pins.Add(goToPin);
                        AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(goToPin.Position, Distance.FromMeters(100)));

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }


                }
            }

        }
        private async void RedirSignIn(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
        private async void OnUpdateProfile(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Profilul a fost actualizat.", 1300);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
                if (!await VerifyLocation(true))
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
                if (!await VerifyLocation(true))
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
                NumeCompletEntry.IsValid && await VerifyLocation(false))
                    return true;
                return false;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;

            }
        }
        private async Task<bool> VerifyLocation(bool changeLocation)
        {
            try
            {
                if (OrasEntry.IsValid && NumeNrStradaEntry.IsValid && App.isLoggedIn)
                {

                    IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync(NumeNrStrada.Text + ", " + Oras.Text + ", Romania").ConfigureAwait(false);
                    if (aproxLocation.Count() > 0 && !string.IsNullOrWhiteSpace(NumeNrStrada.Text) && !string.IsNullOrWhiteSpace(Oras.Text))
                    {
                        if (changeLocation && (Oras.Text != App.userInfo.City || NumeNrStrada.Text != App.userInfo.Street))
                        {
                            var posn = aproxLocation.First();
                            await Device.InvokeOnMainThreadAsync(() =>
                            {
                                if (AppMap.Pins.Count > 0)
                                {
                                    Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label == "Adresa mea");
                                    if (pinTo != null)
                                        AppMap.Pins.Remove(pinTo);

                                }
                                Pin goToPin = new Pin()
                                {
                                    Label = "Adresa mea",
                                    Type = PinType.Place,
                                    Position = posn,

                                };
                                AppMap.Pins.Add(goToPin);
                                AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(aproxLocation.First(), Distance.FromMeters(100)));
                            });

                            App.userInfo.CoordX = posn.Latitude;
                            App.userInfo.CoordY = posn.Longitude;
                        }

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
        void PickupButton_Clicked(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            try
            {
                
                Pin goToPin;
                var map = (Map)sender;
                //User Actual Location
                if (map.IsVisible)
                {
                    if (AppMap.Pins.Count == 0)
                    {
                        goToPin = new Pin()
                        {
                            Label = "Adresa mea",
                            Type = PinType.Place,
                            Position = new Position(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude)
                        };
                        AppMap.Pins.Add(goToPin);

                    }
                    else
                    {
                        Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label == "Adresa mea");
                        pinTo.Position = new Position(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude);
                    }
                    App.userInfo.CoordX = map.VisibleRegion.Center.Latitude;
                    App.userInfo.CoordY = map.VisibleRegion.Center.Longitude;
                }
                    
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }



        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!this.AppMap.IsVisible)
            {
                this.AppMap.IsVisible = !this.AppMap.IsVisible;
                this.AppMap.AnchorX = 1;
                this.AppMap.AnchorY = 1;
                ShowHide.Text = "Inchide Harta";
                Animation scaleAnimation = new Animation(
                    f => this.AppMap.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => this.AppMap.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(this.AppMap, "popupScaleAnimation", 250);
                fadeAnimation.Commit(this.AppMap, "popupFadeAnimation", 250);
            }
            else
            {

                await Task.WhenAny<bool>
                  (
                    this.AppMap.FadeTo(0, 200, Easing.SinInOut)
                  );
                ShowHide.Text = "Deschide Harta";

                this.AppMap.IsVisible = !this.AppMap.IsVisible;
            }
        }
    }
}
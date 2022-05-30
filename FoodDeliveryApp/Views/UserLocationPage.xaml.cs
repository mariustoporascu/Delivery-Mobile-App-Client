using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class UserLocationPage : ContentPage
    {
        Geocoder geoCoder;
        UserLocationViewModel viewModel;
        public UserLocationPage()
        {
            InitializeComponent();
            geoCoder = new Geocoder();
            BindingContext = viewModel = new UserLocationViewModel();
            viewModel.OnUpdateLocation += OnUpdateLocation;
            viewModel.UpdateLocationFailed += UpdateLocationFailed;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (App.UserInfo.Location != null)
            {
                viewModel.City = App.UserInfo.Location.City;
                viewModel.BuildingInfo = App.UserInfo.Location.BuildingInfo;
                viewModel.Street = App.UserInfo.Location.Street;
                viewModel.CoordX = App.UserInfo.Location.CoordX;
                viewModel.CoordY = App.UserInfo.Location.CoordY;
            }

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

            };
            if (viewModel.CoordX != 0 && viewModel.CoordY != 0)
            {
                try
                {

                    goToPin.Position = new Position(viewModel.CoordX, viewModel.CoordY);

                    AppMap.Pins.Add(goToPin);
                    AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(goToPin.Position, Distance.FromMeters(100)));

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


            }
            else
            {
                IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync("Centru, Cernavoda, Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    goToPin.Position = position1;
                    AppMap.Pins.Add(goToPin);
                    if (AppMap.IsVisible)
                        AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(goToPin.Position, Distance.FromMeters(100)));
                }
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
        private async void CheckFieldNumeNrStrada(object sender, EventArgs e)
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
        private async void CheckFieldOras(object sender, EventArgs e)
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
                CladireApEntry.IsValid && await VerifyLocation(false))
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
                if (OrasEntry.IsValid && NumeNrStradaEntry.IsValid && App.IsLoggedIn)
                {

                    IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync(NumeNrStrada.Text + ", " + Oras.Text + ", Romania");
                    if (aproxLocation.Count() > 0 && !string.IsNullOrWhiteSpace(NumeNrStrada.Text) && !string.IsNullOrWhiteSpace(Oras.Text))
                    {
                        if (changeLocation)
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

                            viewModel.CoordX = posn.Latitude;
                            viewModel.CoordY = posn.Longitude;
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
        void UserMovedView(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            try
            {

                Pin goToPin;
                var map = (Map)sender;
                //User Actual Location

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
                viewModel.CoordX = map.VisibleRegion.Center.Latitude;
                viewModel.CoordY = map.VisibleRegion.Center.Longitude;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (await IsProfileValid())
                    viewModel.SaveLocation.Execute(null);
                else
                    await DisplayAlert("Eroare", "Datele locatiei nu sunt complete.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void OnUpdateLocation(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Locatia a fost actualizata.", 1300);
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void UpdateLocationFailed(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Incercare esuata.", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void DismissClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
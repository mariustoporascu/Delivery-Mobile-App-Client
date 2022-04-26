using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Views
{
    public partial class MapsPage : ContentPage
    {
        MapsViewModel mapsViewModel;
        private float totalDistance = 0.0f;
        private int timeToGo = 0;
        public MapsPage()
        {
            InitializeComponent();
            BindingContext = mapsViewModel = new MapsViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await mapsViewModel.LoadMyLocation();
            if (mapsViewModel.pinRoute1.Position != null)
            {
                if (AppMap.Pins.FirstOrDefault(pin => pin.Label == "Adresa mea") == null)
                    AppMap.Pins.Add(mapsViewModel.pinRoute1);
                else
                    AppMap.Pins.FirstOrDefault(pin => pin.Label == "Adresa mea").Position = mapsViewModel.pinRoute1.Position;
                AppMap.MoveToRegion(new MapSpan(mapsViewModel.pinRoute1.Position, 0.01, 0.01));
            }
        }

        void PickupButton_Clicked(object sender, MapClickedEventArgs e)
        {
            //User Actual Location
            if (AppMap.Pins.Count > 0)
            {
                Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label == "Curier");
                if (pinTo != null)
                    AppMap.Pins.Remove(pinTo);
            }

            Pin goToPin = new Pin()
            {
                Label = "Curier",
                Type = PinType.Place,
                Position = e.Position,

            };
            AppMap.Pins.Add(goToPin);
        }
        async void PickupButton_Clicked2(object sender, MapClickedEventArgs e)
        {
            //User Actual Location
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
                Position = e.Position,

            };
            var userLocation = await mapsViewModel.geoCoder.GetAddressesForPositionAsync(e.Position).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(userLocation.FirstOrDefault()))
            {
                var split = userLocation.FirstOrDefault().Split(',');
                App.userInfo.City = split[split.Count() - 2];
                App.userInfo.Street = split.FirstOrDefault(str => str.ToLower().StartsWith("str"));
            }
            AppMap.Pins.Add(goToPin);
        }

        async void TrackPath_Clicked(object sender, EventArgs e)
        {
            Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label == "Curier");
            if (pinTo == null)
                return;
            var route = await mapsViewModel.LoadRoute(pinTo);
            if (route == null)
                return;
            var pathcontent = Enumerable.ToList(Models.MapsModels.PolylineHelper.Decode(route.Routes.First().OverviewPolyline.Points));
            if (pathcontent == null)
                return;
            AppMap.MapElements.Clear();

            var polyline = new Polyline();
            polyline.StrokeColor = Color.Black;
            polyline.StrokeWidth = 3;
            totalDistance = 0.0f;
            for (int i = 0; i < pathcontent.Count; i++)
            {
                var line = pathcontent[i];
                Position nextline;
                if (i != pathcontent.Count - 1)
                {
                    nextline = pathcontent[i + 1];
                    totalDistance += (float)Distance.BetweenPositions(line, nextline).Kilometers;
                }
                polyline.Geopath.Add(line);
            }

            AppMap.MapElements.Add(polyline);

            AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(polyline.Geopath[0].Latitude, polyline.Geopath[0].Longitude), Distance.FromMiles(0.30f)));

            var positionIndex = 1;
            if (totalDistance < 1.0f)
            {
                int convertedDistance = (int)Math.Round(totalDistance * 1000);
                DistToGo.Text = convertedDistance.ToString() + " m";
            }
            else
            {
                DistToGo.Text = totalDistance.ToString().Substring(0, totalDistance.ToString().IndexOf(".") + 3) + " km";
            }
            timeToGo = (int)Math.Round(((totalDistance * 60) / 40));
            if (timeToGo > 0)
                TimeToGo.Text = timeToGo.ToString() + " min";
            else
                TimeToGo.Text = "1 min";
            Device.StartTimer(TimeSpan.FromMilliseconds(1500), () =>
            {
                if (pathcontent.Count > positionIndex)
                {
                    UpdatePostions(pathcontent[positionIndex]);
                    positionIndex++;
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        async void UpdatePostions(Position position)
        {
            if (AppMap.Pins.Count == 1 && AppMap.MapElements != null && AppMap.MapElements?.Count > 1)
                return;

            var cPin = AppMap.Pins.FirstOrDefault(pin => pin.Label == "Curier");

            if (cPin != null)
            {
                cPin.Position = new Position(position.Latitude, position.Longitude);
                AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(cPin.Position, Distance.FromMeters(200)));
                var previousPosition = ((Polyline)AppMap.MapElements?.FirstOrDefault()).Geopath.FirstOrDefault();
                ((Polyline)AppMap.MapElements?.FirstOrDefault()).Geopath?.Remove(previousPosition);
                var currPosition = ((Polyline)AppMap.MapElements?.FirstOrDefault()).Geopath.FirstOrDefault();
                try
                {
                    totalDistance -= (float)Distance.BetweenPositions(previousPosition, currPosition).Kilometers;
                    if (totalDistance > 0.0f)
                    {
                        if (totalDistance < 1.0f)
                        {
                            var convertedDistance = (int)Math.Round(totalDistance * 1000);
                            DistToGo.Text = convertedDistance.ToString() + " m";
                        }
                        else
                        {
                            DistToGo.Text = totalDistance.ToString().Substring(0, totalDistance.ToString().IndexOf(".") + 3) + " km";
                        }
                        timeToGo = (int)Math.Round(((totalDistance * 60) / 40));
                        if (timeToGo > 0)
                            TimeToGo.Text = timeToGo.ToString() + " min";
                        else
                            TimeToGo.Text = "1 min";
                    }
                    else
                    {
                        AppMap.MapElements?.Clear();
                        DistToGo.Text = "0 km";
                        TimeToGo.Text = "0 min";

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


            }
            else
            {
                AppMap.MapElements?.Clear();
                DistToGo.Text = "0 km";
                TimeToGo.Text = "0 min";

            }
        }
    }
}
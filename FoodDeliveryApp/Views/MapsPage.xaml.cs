using FoodDeliveryApp.Models.MapsModels;
using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Views
{
    public partial class MapsPage : ContentPage
    {
        MapsViewModel mapsViewModel;
        Dictionary<int, GoogleDirection> routes;
        private float totalDistance = 0.0f;
        private int timeToGo = 0;
        private bool calculateRoute = false;
        private bool viewLeaved = false;
        public MapsPage()
        {
            InitializeComponent();
            BindingContext = mapsViewModel = new MapsViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            calculateRoute = true;
            AppMap.MapElements.Clear();
            await mapsViewModel.LoadMyLocation();
            if (mapsViewModel.pinRoute1.Position != null)
            {
                try
                {
                    if (AppMap.Pins.FirstOrDefault(pin => pin.Label == "Adresa mea") == null)
                        AppMap.Pins.Add(mapsViewModel.pinRoute1);
                    else
                        AppMap.Pins.FirstOrDefault(pin => pin.Label == "Adresa mea").Position = mapsViewModel.pinRoute1.Position;
                    AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapsViewModel.pinRoute1.Position, Distance.FromMeters(100)));
                    viewLeaved = false;
                    TrackPath_Clicked();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            calculateRoute = false;
            viewLeaved = true;
            DistToGo.Text = "";
            TimeToGo.Text = "";
            routes = null;
            mapsViewModel.HasRoute = false;
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


        void TrackPath_Clicked()
        {

            Device.StartTimer(TimeSpan.FromMilliseconds(3000), () =>
            {
                if (!viewLeaved)
                {
                    RouteAsync();
                    Device.BeginInvokeOnMainThread(() => DrawElements());
                }

                return calculateRoute;
            });
        }
        async void RouteAsync()
        {
            routes = await mapsViewModel.DrawDriverRoute();
        }
        void DrawElements()
        {
            if (routes != null)
            {
                if (routes.Count > 0)
                {
                    Debug.WriteLine("Drawing routes.");

                    UpdatePostions(routes);
                    calculateRoute = true;
                }
                else
                {
                    if (AppMap.Pins.Count > 0)
                    {
                        Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label.Contains("Curier"));
                        if (pinTo != null)
                            AppMap.Pins.Remove(pinTo);
                    }
                    AppMap.MapElements.Clear();
                    Debug.WriteLine("No routes found.");
                    calculateRoute = false;
                }
            }
        }

        void UpdatePostions(Dictionary<int, GoogleDirection> routes)
        {
            AppMap.MapElements.Clear();
            foreach (var route in routes)
            {
                List<Pin> pinTo = AppMap.Pins.Where(pins => pins.Label.Contains("Curier")).ToList();
                foreach (var pin in pinTo)
                    AppMap.Pins.Remove(pin);
                var pathcontent = Enumerable.ToList(Models.MapsModels.PolylineHelper.Decode(route.Value.Routes.First().OverviewPolyline.Points));
                if (pathcontent == null)
                    return;
                AppMap.Pins.Add(new Pin()
                {
                    Label = $"Curier Comanda {route.Key}",
                    Type = PinType.Place,
                    Position = new Position(pathcontent[0].Latitude, pathcontent[0].Longitude),
                });

                var polyline = new Xamarin.Forms.Maps.Polyline();
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


                if (totalDistance < 1.0f)
                {
                    int convertedDistance = (int)Math.Round(totalDistance * 1000);
                    DistToGo.Text = convertedDistance.ToString() + " m";
                }
                else
                {
                    var index = totalDistance.ToString().IndexOf('.');
                    var indexRo = totalDistance.ToString().IndexOf(',');
                    DistToGo.Text = totalDistance.ToString().Substring(0, index > 0 ? index : indexRo + 2) + " km";
                }
                timeToGo = (int)Math.Round(((totalDistance * 60) / 40));
                if (timeToGo > 0)
                    TimeToGo.Text = timeToGo.ToString() + " min";
                else
                    TimeToGo.Text = "1 min";
            }

        }
    }
}
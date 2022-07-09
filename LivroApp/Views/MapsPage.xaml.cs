using LivroApp.Controls;
using LivroApp.Models.MapsModels;
using LivroApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LivroApp.Views
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
            AppMap.Pins.Clear();
            mapsViewModel.MyLocations.Clear();
            await mapsViewModel.LoadMyLocation();
            if (mapsViewModel.MyLocations != null && mapsViewModel.MyLocations.Count > 0)
            {
                try
                {
                    foreach (var pin in mapsViewModel.MyLocations)
                        AppMap.Pins.Add(pin);
                    AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapsViewModel.MyLocations.First().Position, Distance.FromMeters(100)));
                    viewLeaved = false;
                    TrackPath_Clicked();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
            else
            {
                IEnumerable<Position> aproxLocation = await mapsViewModel.GeoCoder.GetPositionsForAddressAsync("Dacia, Cernavoda, Constanta, Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    var pin = new CustomPin
                    {
                        Label = "Cernavoda",
                        Type = PinType.Place,
                        Position = position1
                    };
                    AppMap.Pins.Add(pin);
                    AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)));
                }
            }

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            calculateRoute = false;
            viewLeaved = true;
            routes = null;
            mapsViewModel.Items.Clear();
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

            Device.StartTimer(TimeSpan.FromMilliseconds(5000), () =>
            {
                if (!viewLeaved)
                {
                    RouteAsync();
                    DrawElements();
                }

                return calculateRoute;
            });
        }
        async void RouteAsync()
        {
            routes = await mapsViewModel.DrawDriverRoute().ConfigureAwait(false);
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
            mapsViewModel.Items.Clear();
            List<StatsModel> dict = new List<StatsModel>();
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

                var distTG = string.Empty;
                if (totalDistance < 1.0f)
                {
                    int convertedDistance = (int)Math.Round(totalDistance * 1000);
                    distTG = convertedDistance.ToString() + " m";
                }
                else
                {
                    var index = totalDistance.ToString().IndexOf('.');
                    var indexRo = totalDistance.ToString().IndexOf(',');
                    distTG = totalDistance.ToString().Substring(0, index > 0 ? index : indexRo + 2) + " km";
                }
                timeToGo = (int)Math.Round(((totalDistance * 60) / 40));
                var timeTG = string.Empty;
                if (timeToGo > 0)
                    timeTG = timeToGo.ToString() + " min";
                else
                    timeTG = "1 min";
                dict.Add(new StatsModel
                {
                    Title = $"Comanda {route.Key}",
                    DistToGo = $"Distanta ramasa {distTG}",
                    TimeToGo = $"Timp ramas {timeTG}"
                });

            }
            mapsViewModel.Items.AddRange(dict);
        }
    }
}
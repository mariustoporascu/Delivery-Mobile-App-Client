using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
                AppMap.MoveToRegion(new MapSpan(mapsViewModel.pinRoute1.Position, 0.01, 0.01));
        }

        private bool TimerStarted()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Compass.Start(SensorSpeed.UI, applyLowPassFilter: true);
                AppMap.Pins.Clear();
                AppMap.MapElements.Clear();
                //Get the cars nearrby from api but here we are hardcoding the contents
            }

            );
            Compass.Stop();
            return true;
        }

        void PickupButton_Clicked(object sender, MapClickedEventArgs e)
        {
            //User Actual Location
            if (AppMap.Pins.Count > 0)
            {
                Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label == "Direction To Pin");
                if (pinTo != null)
                    AppMap.Pins.Remove(pinTo);
            }

            Pin goToPin = new Pin()
            {
                Label = "Direction To Pin",
                Type = PinType.Place,
                //Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("PickupPin.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "PickupPin.png", WidthRequest = 30, HeightRequest = 30 }),
                Position = e.Position,
                //IsDraggable = true

            };
            AppMap.Pins.Add(goToPin);
            //This is my actual location as of now we are taking it from google maps. But you have to use location plugin to generate latitude and longitude.
            var positions = e.Position;//Latitude, Longitude
            //AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(positions, Distance.FromMeters(500)));
        }

        async void TrackPath_Clicked(object sender, EventArgs e)
        {
            Pin pinTo = AppMap.Pins.FirstOrDefault(pins => pins.Label == "Direction To Pin");
            if (pinTo == null)
                return;
            var pathcontent = await mapsViewModel.LoadRoute(pinTo);
            if (pathcontent == null)
                return;


            //var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            //var stream = assembly.GetManifestResourceStream($"XamGMaps.MapResources.TrackPath.json");
            //string trackPathFile;

            //using (var reader = new System.IO.StreamReader(stream))
            //{
            //    trackPathFile = reader.ReadToEnd();
            //}

            //var myJson = JsonConvert.DeserializeObject<List<Position>>(trackPathFile);


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

            AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(polyline.Geopath[0].Latitude, polyline.Geopath[0].Longitude), Distance.FromMiles(0.50f)));

            var pin = new Pin
            {
                Type = PinType.SearchResult,
                Position = new Position(polyline.Geopath.First().Latitude, polyline.Geopath.First().Longitude),
                Label = "Pin",
                Address = "Pin",
                //Tag = "CirclePoint",
                //Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("ic_circle_point.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "ic_circle_point.png", WidthRequest = 25, HeightRequest = 25 })

            };
            AppMap.Pins.Add(pin);

            var positionIndex = 1;
            if (totalDistance < 1.0f)
            {
                var convertedDistance = totalDistance * 1000;
                DistToGo.Text = convertedDistance.ToString().Substring(0, convertedDistance.ToString().IndexOf(".")) + " m";
            }
            else
            {
                DistToGo.Text = totalDistance.ToString().Substring(0, totalDistance.ToString().IndexOf(".") + 3) + " km";
            }
            timeToGo = (int)((totalDistance * 60) / 40);
            if (timeToGo > 0)
                TimeToGo.Text = timeToGo.ToString() + " min";
            else
                TimeToGo.Text = timeToGo.ToString() + "1 min";
            Device.StartTimer(TimeSpan.FromMilliseconds(2500), () =>
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

            var cPin = AppMap.Pins.FirstOrDefault();

            if (cPin != null)
            {
                cPin.Position = new Position(position.Latitude, position.Longitude);
                //cPin.Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("CarPins.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "CarPins.png", WidthRequest = 25, HeightRequest = 25 });
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
                            var convertedDistance = totalDistance * 1000;
                            DistToGo.Text = convertedDistance.ToString().Substring(0, convertedDistance.ToString().IndexOf(".")) + " m";
                        }
                        else
                        {
                            DistToGo.Text = totalDistance.ToString().Substring(0, totalDistance.ToString().IndexOf(".") + 3) + " km";
                        }
                        timeToGo = (int)((totalDistance * 60) / 40);
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
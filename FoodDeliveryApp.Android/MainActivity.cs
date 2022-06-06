using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Plugin.FacebookClient;
using System;

namespace FoodDeliveryApp.Droid
{
    [Activity(Label = "FoodDeliveryApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.ColorMode, ScreenOrientation = ScreenOrientation.Portrait, Exported = true)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);

            FacebookClientManager.Initialize(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            //GetLocationPermissions();
            LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            var app = (FoodDeliveryApp.App)App.Current;
            if (app.PromptToConfirmExit)
            {
                ConfirmWithDialog();
                return;
            }
            base.OnBackPressed();
        }


        //private async void GetLocationPermissions()
        //{
        //    var status = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
        //    if (status == Xamarin.Essentials.PermissionStatus.Granted)
        //        return;
        //    var getPerm = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
        //    if (getPerm == Xamarin.Essentials.PermissionStatus.Granted)
        //        return;
        //    else
        //        Toast.MakeText(this, "You will not be able to use map features without the permission.", ToastLength.Long);
        //}
        private void ConfirmWithDialog()
        {
            using (var alert = new AndroidX.AppCompat.App.AlertDialog.Builder(this))
            {
                alert.SetTitle("Confirma inchiderea");
                alert.SetMessage("Esti sigur ca doresti sa inchizi aplicatia?");
                alert.SetPositiveButton("Da", (sender, args) => { FinishAffinity(); });
                alert.SetNegativeButton("Nu", (sender, args) => { }); // do nothing

                var dialog = alert.Create();
                dialog.Show();
            }
            return;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
        }
    }
}
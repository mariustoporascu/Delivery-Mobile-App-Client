using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Exported = true)]
    public class SplashActivity : AppCompatActivity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            try
            {
                Task startupWork = new Task(() => { SimulateStartup(); });
                startupWork.Start();
            }
            catch (Exception)
            {

            }
        }

        // Simulates background work that happens behind the splash screen
        void SimulateStartup()
        {
            try
            {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));

            }
            catch (Exception)
            {

            }
        }
        public override void OnBackPressed() { }
    }
}
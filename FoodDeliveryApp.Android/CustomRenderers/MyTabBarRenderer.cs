using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using FoodDeliveryApp;
using FoodDeliveryApp.Droid.CustomRenderers;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

//[assembly: ExportRenderer(typeof(AppShell), typeof(MyTabBarRenderer))]
namespace FoodDeliveryApp.Droid.CustomRenderers
{
    public class MyTabBarRenderer : ShellRenderer
    {
        public MyTabBarRenderer(Context context) : base(context)
        {
        }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            base.CreateBottomNavViewAppearanceTracker(shellItem);
            return new CustomBottomNavAppearance();
        }
    }

    public class CustomBottomNavAppearance : IShellBottomNavViewAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(BottomNavigationView bottomView)
        {

        }

        public void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {

            IMenu menu = bottomView.Menu;
            for (int i = 0; i < bottomView.Menu.Size(); i++)
            {
                IMenuItem menuItem = menu.GetItem(i);
                var title = menuItem.TitleFormatted;
                SpannableStringBuilder sb = new SpannableStringBuilder(title);
                int a = sb.Length();

                //here I set fontsize 20
                sb.SetSpan(new AbsoluteSizeSpan(18, true), 0, a, SpanTypes.ExclusiveExclusive);

                menuItem.SetTitle(sb);
            }

        }
    }
}
using Android.Content;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

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
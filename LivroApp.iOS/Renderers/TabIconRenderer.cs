using CoreGraphics;
using LivroApp;
using LivroApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AppShell), typeof(TabIconRenderer))]
namespace LivroApp.iOS.Renderers
{
    public class TabIconRenderer : ShellRenderer
    {
        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            return new CustomShellSectionRenderer(this);
        }

        private class CustomShellSectionRenderer : ShellSectionRenderer
        {
            public CustomShellSectionRenderer(IShellContext context) : base(context)
            {
            }

            protected override void UpdateTabBarItem()
            {
                base.UpdateTabBarItem();
                //TODO: Calculate the size according the screen.
                //According to Apple:
                //@1x: 48x32
                //@2x: 96x64
                //@3x: 144x96
                TabBarItem.Image = ScalingImageToSize(TabBarItem.Image, new CGSize(48, 32)); // set the size here as you want 
            }

            public UIImage ScalingImageToSize(UIImage sourceImage, CGSize newSize)
            {

                if (UIScreen.MainScreen.Scale == 2.0) //@2x iPhone 6 7 8 
                {
                    UIGraphics.BeginImageContextWithOptions(newSize, false, 2.0f);
                }


                else if (UIScreen.MainScreen.Scale == 3.0) //@3x iPhone 6p 7p 8p...
                {
                    UIGraphics.BeginImageContextWithOptions(newSize, false, 3.0f);
                }

                else
                {
                    UIGraphics.BeginImageContext(newSize);
                }

                sourceImage.Draw(new CGRect(0, 0, newSize.Width, newSize.Height));

                UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();

                UIGraphics.EndImageContext();

                return newImage;

            }
        }
    }
}
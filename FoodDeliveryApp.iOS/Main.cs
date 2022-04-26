using UIKit;

namespace FoodDeliveryApp.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            /*CultureInfo newCulture = new CultureInfo("en-US");// whatever :)
                                                              // make sure the following is executed on the UI thread
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;*/
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}

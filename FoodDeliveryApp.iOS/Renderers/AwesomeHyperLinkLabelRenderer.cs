using CoreGraphics;
using FoodDeliveryApp.Controls;
using FoodDeliveryApp.iOS.Renderers;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AwesomeHyperLinkLabel), typeof(AwesomeHyperLinkLabelRenderer))]
namespace FoodDeliveryApp.iOS.Renderers

{
    public class AwesomeHyperLinkLabelRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var view = (AwesomeHyperLinkLabel)Element;
            if (view == null) return;

            UITextView uilabelleftside = new UITextView(new CGRect(0, 0, view.Width, view.Height));
            uilabelleftside.Text = view.Text;
            uilabelleftside.Font = UIFont.SystemFontOfSize((float)view.FontSize);
            uilabelleftside.Editable = false;
            uilabelleftside.Center = new CGPoint(0, 0);

            // Setting the data detector types mask to capture all types of link-able data
            uilabelleftside.DataDetectorTypes = UIDataDetectorType.All;
            uilabelleftside.BackgroundColor = UIColor.Clear;

            // overriding Xamarin Forms Label and replace with our native control
            SetNativeControl(uilabelleftside);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Controls
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; } = new List<CustomPin>();
    }
    public class CustomPin : Pin
    {
        public int LocationId { get; set; }
    }
}

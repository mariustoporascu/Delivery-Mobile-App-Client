using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace LivroApp.Controls
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

using System;
using Xamarin.Forms;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Companie
    {
        public int RestaurantId { get; set; }
        public int SuperMarketId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Image { get; set; }
        public Image ImageFinal { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Categ
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? RestaurantRefId { get; set; }
        public int? SuperMarketRefId { get; set; }

        public string Image { get; set; }
        public Image ImageFinal { get; set; }
    }
}
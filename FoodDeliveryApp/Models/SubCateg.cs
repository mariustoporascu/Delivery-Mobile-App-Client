using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FoodDeliveryApp.Models
{
    public class SubCateg
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryRefId { get; set; }

        public string Image { get; set; }
        public Image ImageFinal { get; set; }
    }
}
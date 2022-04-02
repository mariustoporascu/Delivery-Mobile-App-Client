using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public decimal TotalOrdered { get; set; }
        public string CustomerId { get; set; }
    }
}

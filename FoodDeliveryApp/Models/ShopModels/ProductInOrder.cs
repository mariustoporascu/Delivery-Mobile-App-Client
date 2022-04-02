using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class ProductInOrder
    {
        public int OrderRefId { get; set; }

        public int ProductRefId { get; set; }

        public int UsedQuantity { get; set; }
    }
}

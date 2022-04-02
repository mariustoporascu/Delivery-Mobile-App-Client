using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class OrderInfo
    {
        public int OrderInfoId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }

        public int OrderRefId { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class ServerOrder
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public decimal TotalOrdered { get; set; }
        public string CustomerId { get; set; }
        public string DriverRefId { get; set; }
        public DateTime Created { get; set; }
        [JsonProperty("productsInOrder")]
        public List<ProductInOrder> ProductsInOrder { get; set; }
        [JsonProperty("orderInfo")]
        public OrderInfo OrderInfos { get; set; }
        [JsonProperty("driver")]
        public Driver OrderDriver { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LivroApp.Models.ShopModels
{
    public class ServerOrder : Order
    {
        public bool TelephoneOrdered { get; set; }
        public bool IsOrderPayed { get; set; }
        public int UserLocationId { get; set; }
        [JsonProperty("productsInOrder")]
        public List<ProductInOrder> ProductsInOrder { get; set; }
        [JsonProperty("orderInfo")]
        public OrderInfo OrderInfos { get; set; }
        [JsonProperty("driver")]
        public Driver OrderDriver { get; set; }
    }
}

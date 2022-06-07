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
        public decimal TransportFee { get; set; }
        public string PaymentMethod { get; set; }
        public string Comments { get; set; }
        public string CustomerId { get; set; }
        public string DriverRefId { get; set; }
        public int CompanieRefId { get; set; }
        public bool TelephoneOrdered { get; set; }
        public string EstimatedTime { get; set; }
        public string NumeCompanie { get; set; }

        public bool IsOrderPayed { get; set; }
        public bool ClientGaveRatingDriver { get; set; } = false;
        public bool ClientGaveRatingCompanie { get; set; } = false;
        public bool? HasUserConfirmedET { get; set; }
        //public int RatingClient { get; set; }
        public int UserLocationId { get; set; }
        public int RatingDriver { get; set; }
        public int RatingCompanie { get; set; }
        public DateTime Created { get; set; }
        [JsonProperty("productsInOrder")]
        public List<ProductInOrder> ProductsInOrder { get; set; }
        [JsonProperty("orderInfo")]
        public OrderInfo OrderInfos { get; set; }
        [JsonProperty("driver")]
        public Driver OrderDriver { get; set; }
    }
}

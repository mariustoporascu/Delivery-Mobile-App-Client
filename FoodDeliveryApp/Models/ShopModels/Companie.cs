using FoodDeliveryApp.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Companie
    {
        public int CompanieId { get; set; }
        public string Name { get; set; }
        public string TelefonNo { get; set; }
        public DateTime Opening { get; set; }
        public int TipCompanieRefId { get; set; }
        public bool IsActive { get; set; }
        public bool TemporaryClosed { get; set; }
        public bool VisibleInApp { get; set; }

        [JsonProperty("transportFees")]
        public List<TransportFee> TransportFees { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
    new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png") :
    new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");
        public string Photo { get; set; }
    }
}
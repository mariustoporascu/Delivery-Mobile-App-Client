using FoodDeliveryApp.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class TipCompanie : BaseModel
    {
        public int TipCompanieId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
            new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png") :
            new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");
        public bool IsOpen { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
    }
}

using FoodDeliveryApp.Constants;
using System;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Categ
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanieRefId { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
            new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png") :
            new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");
        public string Photo { get; set; }
    }
}
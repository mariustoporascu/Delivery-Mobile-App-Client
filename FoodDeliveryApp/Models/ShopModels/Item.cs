using FoodDeliveryApp.Constants;
using System;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Item : BaseModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Gramaj { get; set; }
        public string GramajInterfata { get; set; }
        public decimal Price { get; set; }
        public string PretInterfata { get; set; }
        public int CategoryRefId { get; set; }

        public int MeasuringUnitId { get; set; }
        public int? SubCategoryRefId { get; set; }
        public int? RestaurantRefId { get; set; }
        public int? SuperMarketRefId { get; set; }
        public string Photo { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
    new Uri($"{ServerConstants.BaseUrl}/content/No_image_available.png") :
    new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");

        private int _cantitate;
        public int Cantitate
        {
            get => _cantitate;
            set => SetProperty(ref _cantitate, value);
        }

    }
}
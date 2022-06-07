using System;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Order : BaseModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public decimal TotalOrdered { get; set; }
        public decimal TransportFee { get; set; }
        public string TotalOrderedInterfata { get; set; }
        public string CustomerId { get; set; }
        public string PaymentMethod { get; set; }
        public int CompanieRefId { get; set; }
        public string Comments { get; set; }

        public string NumeCompanie { get; set; }
        public string EstimatedTime { get; set; }
        public string DriverRefId { get; set; }
        public DateTime Created { get; set; }

        private bool? _hasUserConfirmedET;
        public bool? HasUserConfirmedET { get => _hasUserConfirmedET; set => SetProperty(ref _hasUserConfirmedET, value); }
        private bool _giveratingdriver;
        public bool ClientGaveRatingDriver { get => _giveratingdriver; set => SetProperty(ref _giveratingdriver, value); }

        private bool _giveratingrestaurant;
        public bool ClientGaveRatingCompanie { get => _giveratingrestaurant; set => SetProperty(ref _giveratingrestaurant, value); }
        //public int _ratingClient;
        private int _ratingDriver;
        private int _ratingRestaurant;
        //public int RatingClient { get => _ratingClient; set => SetProperty(ref _ratingClient, value); }
        public int RatingDriver { get => _ratingDriver; set => SetProperty(ref _ratingDriver, value); }
        public int RatingCompanie { get => _ratingRestaurant; set => SetProperty(ref _ratingRestaurant, value); }
    }
}

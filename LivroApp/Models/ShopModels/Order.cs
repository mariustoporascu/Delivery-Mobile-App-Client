using System;

namespace LivroApp.Models.ShopModels
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
        private bool _giveratingdriver;
        private bool _giveratingrestaurant;
        private int _ratingDriver;
        private int _ratingRestaurant;
        public bool? HasUserConfirmedET { get => _hasUserConfirmedET; set => SetProperty(ref _hasUserConfirmedET, value); }
        public bool ClientGaveRatingDriver { get => _giveratingdriver; set => SetProperty(ref _giveratingdriver, value); }
        public bool ClientGaveRatingCompanie { get => _giveratingrestaurant; set => SetProperty(ref _giveratingrestaurant, value); }
        public int RatingDriver { get => _ratingDriver; set => SetProperty(ref _ratingDriver, value); }
        public int RatingCompanie { get => _ratingRestaurant; set => SetProperty(ref _ratingRestaurant, value); }
    }
}

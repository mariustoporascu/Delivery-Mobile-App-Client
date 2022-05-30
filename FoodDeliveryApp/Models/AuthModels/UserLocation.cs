using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.AuthModels
{
    public class UserLocation : BaseModel
    {
        private double _coordX;
        private double _coordY;
        private string _city = string.Empty;
        private string _buildinginfo = string.Empty;
        private string _street = string.Empty;
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
        public double CoordX { get => _coordX; set => SetProperty(ref _coordX, value); }
        public double CoordY { get => _coordY; set => SetProperty(ref _coordY, value); }
    }
}

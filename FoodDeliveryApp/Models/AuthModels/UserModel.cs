using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.AuthModels
{
    public class UserModel : BaseModel
    {
        private string _fullName = string.Empty;
        private string _city = string.Empty;
        private string _buildinginfo = string.Empty;
        private string _street = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _userIdentification = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        public string? Email { get => _email; set => SetProperty(ref _email, value); }
        public string? FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string? Password { get => _password; set => SetProperty(ref _password, value); }
        public string? UserIdentification { get => _userIdentification; set => SetProperty(ref _userIdentification, value); }
        public string? PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string? Street { get => _street; set => SetProperty(ref _street, value); }
        public string? City { get => _city; set => SetProperty(ref _city, value); }
        public string? BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
    }
}

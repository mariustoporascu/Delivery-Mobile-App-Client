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
        private bool _completeProfile = false;
        private double _coordX;
        private double _coordY;
        private string _loginToken;
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string UserIdentification { get => _userIdentification; set => SetProperty(ref _userIdentification, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
        public string LoginToken { get => _loginToken; set => SetProperty(ref _loginToken, value); }
        public bool CompleteProfile { get => _completeProfile; set => SetProperty(ref _completeProfile, value); }
        public double CoordX { get => _coordX; set => SetProperty(ref _coordX, value); }
        public double CoordY { get => _coordY; set => SetProperty(ref _coordY, value); }
    }
}

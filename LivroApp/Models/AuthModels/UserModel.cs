using Newtonsoft.Json;
using System.Collections.Generic;

namespace LivroApp.Models.AuthModels
{
    public class UserModel : BaseModel
    {
        private string _fullName = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _userIdentification = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _completeProfile = false;
        private bool _hasSetPassword = false;
        private string _loginToken = string.Empty;
        private string _resetTokenPass = string.Empty;
        private string _fireBaseToken = string.Empty;
        private string _newPassword = string.Empty;
        private List<UserLocation> _locations = new List<UserLocation>();

        public UserLocation? Location { get; set; }
        public int LocationDeleteId { get; set; }
        [JsonProperty("locations")]
        public List<UserLocation> Locations { get => _locations; set => SetProperty(ref _locations, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string UserIdentification { get => _userIdentification; set => SetProperty(ref _userIdentification, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string LoginToken { get => _loginToken; set => SetProperty(ref _loginToken, value); }
        public bool CompleteProfile { get => _completeProfile; set => SetProperty(ref _completeProfile, value); }
        public bool HasSetPassword { get => _hasSetPassword; set => SetProperty(ref _hasSetPassword, value); }
        public string ResetTokenPass { get => _resetTokenPass; set => SetProperty(ref _resetTokenPass, value); }
        public string FireBaseToken { get => _fireBaseToken; set => SetProperty(ref _fireBaseToken, value); }
        public string NewPassword { get => _newPassword; set => SetProperty(ref _newPassword, value); }

    }
}

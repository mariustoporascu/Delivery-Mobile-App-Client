using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.AuthModels
{
    public class HelpMe : BaseModel
    {
        private string _email = string.Empty;
        private string _telNo = string.Empty;
        private string _name = string.Empty;
        private string _message = string.Empty;
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string TelNo { get => _telNo; set => SetProperty(ref _telNo, value); }
        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
    }
}

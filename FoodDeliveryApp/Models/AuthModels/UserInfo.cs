using System.Collections.Generic;
using System.Security.Claims;

namespace FoodDeliveryApp.Models.AuthModels
{
    public class UserInfo
    {
        public IEnumerable<Claim> UserClaim { get; set; }
    }
}
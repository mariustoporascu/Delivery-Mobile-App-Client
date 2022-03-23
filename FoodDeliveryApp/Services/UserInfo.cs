using IdentityModel.OidcClient.Results;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FoodDeliveryApp.Services
{
    public class UserInfo
    {
        public IEnumerable<Claim> UserClaim { get; set; }
    }
}
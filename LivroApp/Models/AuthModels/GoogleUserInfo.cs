using System.Collections.Generic;
using System.Security.Claims;

namespace LivroApp.Models.AuthModels
{
    public class GoogleUserInfo
    {
        public IEnumerable<Claim>? UserClaim { get; set; }
    }
}
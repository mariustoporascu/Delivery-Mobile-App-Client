using FoodDeliveryApp.Models.AuthModels;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;

namespace FoodDeliveryApp.Services
{
    public static class OidcClientExt
    {
        public static Credentials ToCredentials(this LoginResult loginResult)
            => new Credentials
            {
                AccessToken = loginResult.AccessToken,
                IdentityToken = loginResult.IdentityToken,
                RefreshToken = loginResult.RefreshToken,
                AccessTokenExpiration = loginResult.AccessTokenExpiration
            };
        public static UserInfo ToUserInfo(this UserInfoResult userInfoResult)
            => new UserInfo
            {
                UserClaim = userInfoResult.Claims,
            };

        public static Credentials ToCredentials(this RefreshTokenResult refreshTokenResult)
            => new Credentials
            {
                AccessToken = refreshTokenResult.AccessToken,
                IdentityToken = refreshTokenResult.IdentityToken,
                RefreshToken = refreshTokenResult.RefreshToken,
                AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration
            };
    }
}
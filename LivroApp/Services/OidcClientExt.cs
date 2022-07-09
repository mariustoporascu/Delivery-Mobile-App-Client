using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using LivroApp.Models.AuthModels;

namespace LivroApp.Services
{
    public static class OidcClientExt
    {
        public static GoogleCredentials ToCredentials(this LoginResult loginResult)
            => new GoogleCredentials
            {
                AccessToken = loginResult.AccessToken,
                IdentityToken = loginResult.IdentityToken,
                RefreshToken = loginResult.RefreshToken,
                AccessTokenExpiration = loginResult.AccessTokenExpiration
            };
        public static GoogleUserInfo ToUserInfo(this UserInfoResult userInfoResult)
            => new GoogleUserInfo
            {
                UserClaim = userInfoResult.Claims,
            };

        public static GoogleCredentials ToCredentials(this RefreshTokenResult refreshTokenResult)
            => new GoogleCredentials
            {
                AccessToken = refreshTokenResult.AccessToken,
                IdentityToken = refreshTokenResult.IdentityToken,
                RefreshToken = refreshTokenResult.RefreshToken,
                AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration
            };
    }
}
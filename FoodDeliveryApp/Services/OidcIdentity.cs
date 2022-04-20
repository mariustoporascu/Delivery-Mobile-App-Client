using System;
using System.Threading.Tasks;
using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.AuthModels;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;

namespace FoodDeliveryApp.Services
{
    public class OidcIdentity
    {
        public OidcIdentity()
        {

        }

        public async Task<Credentials> Authenticate()
        {
            try
            {
                OidcClient oidcClient = CreateOidcClient();
                LoginResult loginResult = await oidcClient.LoginAsync(new LoginRequest());
                return loginResult.ToCredentials();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Credentials { Error = ex.ToString() };
            }
        }
        public async Task<UserInfo> GetUserInfo(string? accessToken)
        {
            try
            {
                OidcClient oidcClient = CreateOidcClient();
                UserInfoResult userInfoResult = await oidcClient.GetUserInfoAsync(accessToken);
                return userInfoResult.ToUserInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UserInfo();
            }
        }

        private OidcClient CreateOidcClient()
        {
            var options = new OidcClientOptions
            {
                Authority = GoogleConstants.GoogleUrl,
                ClientId = GoogleConstants.ClientId,
                Scope = GoogleConstants.Scope,
                RedirectUri = App.CallbackScheme,
                PostLogoutRedirectUri = App.SignoutCallbackScheme,
                Browser = new WebAuthBrowser(),
                ProviderInformation = new ProviderInformation
                {
                    IssuerName = GoogleConstants.IssuerName,
                    KeySet = new IdentityModel.Jwk.JsonWebKeySet(),
                    TokenEndpoint = GoogleConstants.TokenEndpoint,
                    AuthorizeEndpoint = GoogleConstants.AuthorizeEndpoint,
                    UserInfoEndpoint = GoogleConstants.UserInfoEndpoint,
                    EndSessionEndpoint = GoogleConstants.RevokeEndpoint,
                }

            };

            var oidcClient = new OidcClient(options);
            return oidcClient;
        }
    }
}
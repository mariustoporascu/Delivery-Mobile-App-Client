using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using LivroApp.Constants;
using LivroApp.Models.AuthModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LivroApp.Services
{
    public class OidcIdentity
    {
        public OidcIdentity()
        {

        }

        public async Task<GoogleCredentials> Authenticate()
        {
            try
            {
                OidcClient oidcClient = CreateOidcClient();
                LoginResult loginResult = await oidcClient.LoginAsync(new LoginRequest());
                return loginResult.ToCredentials();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new GoogleCredentials { Error = ex.ToString() };
            }
        }
        public async Task<GoogleUserInfo> GetUserInfo(string? accessToken)
        {
            try
            {
                OidcClient oidcClient = CreateOidcClient();
                UserInfoResult userInfoResult = await oidcClient.GetUserInfoAsync(accessToken);
                return userInfoResult.ToUserInfo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new GoogleUserInfo();
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
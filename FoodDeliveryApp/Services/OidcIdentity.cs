using System;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;

namespace FoodDeliveryApp.Services
{
    public class OidcIdentity
    {
        private readonly string _authorityUrl;
        private readonly string _clientId;
        private readonly string _redirectUrl;
        private readonly string _postLogoutRedirectUrl;
        private readonly string _scope;
        private readonly string? _clientSecret;
        private readonly string _issuerName = "accounts.google.com";
        private readonly string _tokenEndpoint = "https://oauth2.googleapis.com/token";
        private readonly string _authorizeEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private readonly string _userInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo";
        private readonly string _revokeEndpoint = "https://oauth2.googleapis.com/revoke";
        private readonly string _certEndpoint = "https://www.googleapis.com/oauth2/v3/certs";

        public OidcIdentity(string clientId, string redirectUrl, string postLogoutRedirectUrl, string scope, string authorityUrl, string? clientSecret = null)
        {
            _authorityUrl = authorityUrl;
            _clientId = clientId;
            _redirectUrl = redirectUrl;
            _postLogoutRedirectUrl = postLogoutRedirectUrl;
            _scope = scope;
            _clientSecret = clientSecret;
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

        public async Task<LogoutResult> Logout(string? identityToken)
        {
            OidcClient oidcClient = CreateOidcClient();
            return null;
        }

        public async Task<Credentials> RefreshToken(string refreshToken)
        {
            try
            {
                OidcClient oidcClient = CreateOidcClient();
                RefreshTokenResult refreshTokenResult = await oidcClient.RefreshTokenAsync(refreshToken);
                return refreshTokenResult.ToCredentials();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Credentials { Error = ex.ToString() };
            }
        }

        private OidcClient CreateOidcClient()
        {
            var options = new OidcClientOptions
            {
                Authority = _authorityUrl,
                ClientId = _clientId,
                Scope = _scope,
                RedirectUri = _redirectUrl,
                ClientSecret = _clientSecret,
                PostLogoutRedirectUri = _postLogoutRedirectUrl,
                Browser = new WebAuthBrowser(),
                ProviderInformation = new ProviderInformation
                {
                    IssuerName = _issuerName,
                    KeySet = new IdentityModel.Jwk.JsonWebKeySet(),
                    TokenEndpoint = _tokenEndpoint,
                    AuthorizeEndpoint = _authorizeEndpoint,
                    UserInfoEndpoint = _userInfoEndpoint,
                    EndSessionEndpoint = _revokeEndpoint,
                }

            };

            var oidcClient = new OidcClient(options);
            return oidcClient;
        }
    }
}
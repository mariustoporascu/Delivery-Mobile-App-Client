namespace LivroApp.Constants
{
    public class GoogleConstants
    {
        public const string IssuerName = "accounts.google.com";
        public const string TokenEndpoint = "https://oauth2.googleapis.com/token";
        public const string AuthorizeEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string UserInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo";
        public const string RevokeEndpoint = "https://oauth2.googleapis.com/revoke";
        public const string CertEndpoint = "https://www.googleapis.com/oauth2/v3/certs";
        public const string GoogleUrl = "https://accounts.google.com";
        public const string Scope = "openid profile email";
#if !DEBUG
        public const string ClientId = "121275221168-i230p391b86n5rfu4quoq7b6tijitu72.apps.googleusercontent.com";
#else
        public const string ClientId = "121275221168-s0m4efvqr705h17jb744h7hqb92id0bp.apps.googleusercontent.com";
#endif
    }
}

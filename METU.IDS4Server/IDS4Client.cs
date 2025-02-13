using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.IDS4Server
{
    /// <summary>
    /// Client
    /// </summary>
    public class IDS4Client
    {
       public  string ID { get; set; }
        public string CID { get; set; }
        public string AppName { get; set; }
        public string PID { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether [allow offline access]. Defaults to false.
        public bool AllowOfflineAccess { get; set; }
        //
        // 摘要:
        //     Lifetime of identity token in seconds (defaults to 300 seconds / 5 minutes)
        public int IdentityTokenLifetime { get; set; }
        //
        // 摘要:
        //     Signing algorithm for identity token. If empty, will use the server default signing
        //     algorithm.
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }
        //
        // 摘要:
        //     Lifetime of access token in seconds (defaults to 3600 seconds / 1 hour)
        public int AccessTokenLifetime { get; set; }
        //
        // 摘要:
        //     Lifetime of authorization code in seconds (defaults to 300 seconds / 5 minutes)
        public int AuthorizationCodeLifetime { get; set; }
        //
        // 摘要:
        //     Maximum lifetime of a refresh token in seconds. Defaults to 2592000 seconds /
        //     30 days
        public int AbsoluteRefreshTokenLifetime { get; set; }
        //
        // 摘要:
        //     Sliding lifetime of a refresh token in seconds. Defaults to 1296000 seconds /
        //     15 days
        public int SlidingRefreshTokenLifetime { get; set; }
        //
        // 摘要:
        //     Lifetime of a user consent in seconds. Defaults to null (no expiration)
        public int? ConsentLifetime { get; set; }
        //
        // 摘要:
        //     ReUse: the refresh token handle will stay the same when refreshing tokens OneTime:
        //     the refresh token handle will be updated when refreshing tokens
        public string RefreshTokenUsage { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the access token (and its claims) should
        //     be updated on a refresh token request. Defaults to false.
        //
        // 值:
        //     true if the token should be updated; otherwise, false.
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        //
        // 摘要:
        //     Absolute: the refresh token will expire on a fixed point in time (specified by
        //     the AbsoluteRefreshTokenLifetime) Sliding: when refreshing the token, the lifetime
        //     of the refresh token will be renewed (by the amount specified in SlidingRefreshTokenLifetime).
        //     The lifetime will not exceed AbsoluteRefreshTokenLifetime.
        public string RefreshTokenExpiration { get; set; }
        //
        // 摘要:
        //     Specifies whether the access token is a reference token or a self contained JWT
        //     token (defaults to Jwt).
        public string AccessTokenType { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether the local login is allowed for this client.
        //     Defaults to true.
        //
        // 值:
        //     true if local logins are enabled; otherwise, false.
        public bool EnableLocalLogin { get; set; }
        //
        // 摘要:
        //     Specifies which external IdPs can be used with this client (if list is empty
        //     all IdPs are allowed). Defaults to empty.
        public string IdentityProviderRestrictions { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether JWT access tokens should include an identifier.
        //     Defaults to true.
        //
        // 值:
        //     true to add an id; otherwise, false.
        public bool IncludeJwtId { get; set; }
        //
        // 摘要:
        //     Allows settings claims for the client (will be included in the access token).
        //
        // 值:
        //     The claims.
        public string Claims { get; set; }
        //
        // 摘要:
        //     Gets or sets a value indicating whether client claims should be always included
        //     in the access tokens - or only for client credentials flow. Defaults to false
        //
        // 值:
        //     true if claims should always be sent; otherwise, false.
        public bool AlwaysSendClientClaims { get; set; }
        //
        // 摘要:
        //     Gets or sets a value to prefix it on client claim types. Defaults to client_.
        //
        // 值:
        //     Any non empty string if claims should be prefixed with the value; otherwise,
        //     null.
        public string ClientClaimsPrefix { get; set; }
        //
        // 摘要:
        //     Gets or sets a salt value used in pair-wise subjectId generation for users of
        //     this client.
        public string PairWiseSubjectSalt { get; set; }
        //
        // 摘要:
        //     The maximum duration (in seconds) since the last time the user authenticated.
        public int? UserSsoLifetime { get; set; }
        //
        // 摘要:
        //     Gets or sets the type of the device flow user code.
        //
        // 值:
        //     The type of the device flow user code.
        public string UserCodeType { get; set; }
        //
        // 摘要:
        //     Gets or sets the device code lifetime.
        //
        // 值:
        //     The device code lifetime.
        public int DeviceCodeLifetime { get; set; }
        //
        // 摘要:
        //     When requesting both an id token and access token, should the user claims always
        //     be added to the id token instead of requiring the client to use the userinfo
        //     endpoint. Defaults to false.
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        //
        // 摘要:
        //     Specifies the api scopes that the client is allowed to request. If empty, the
        //     client can't access any scope
        public  string  AllowedScopes { get; set; }
        //
        // 摘要:
        //     Gets or sets the custom properties for the client.
        //
        // 值:
        //     The properties.
        public   string Properties { get; set; }
        //
        // 摘要:
        //     Specifies if the user's session id should be sent to the BackChannelLogoutUri.
        //     Defaults to true.
        public bool BackChannelLogoutSessionRequired { get; set; }
        //
        // 摘要:
        //     Specifies if client is enabled (defaults to true)
        public bool Enabled { get; set; }
        //
        // 摘要:
        //     Unique ID of the client
        public string ClientId { get; set; }
        //
        // 摘要:
        //     Gets or sets the protocol type.
        //
        // 值:
        //     The protocol type.
        public string ProtocolType { get; set; }
        //
        // 摘要:
        //     Client secrets - only relevant for flows that require a secret
        public string ClientSecrets { get; set; }
        //
        // 摘要:
        //     If set to false, no client secret is needed to request tokens at the token endpoint
        //     (defaults to true)
        public bool RequireClientSecret { get; set; }
        //
        // 摘要:
        //     Client display name (used for logging and consent screen)
        public string ClientName { get; set; }
        //
        // 摘要:
        //     Description of the client.
        public string Description { get; set; }
        //
        // 摘要:
        //     URI to further information about client (used on consent screen)
        public string ClientUri { get; set; }
        //
        // 摘要:
        //     URI to client logo (used on consent screen)
        public string LogoUri { get; set; }
        //
        // 摘要:
        //     Specifies whether a consent screen is required (defaults to false)
        public bool RequireConsent { get; set; }
        //
        // 摘要:
        //     Specifies whether user can choose to store consent decisions (defaults to true)
        public bool AllowRememberConsent { get; set; }
        //
        // 摘要:
        //     Specifies the allowed grant types (legal combinations of AuthorizationCode, Implicit,
        //     Hybrid, ResourceOwner, ClientCredentials).
        public  string  AllowedGrantTypes { get; set; }
        //
        // 摘要:
        //     Specifies whether a proof key is required for authorization code based token
        //     requests (defaults to true).
        public bool RequirePkce { get; set; }
        //
        // 摘要:
        //     Specifies whether a proof key can be sent using plain method (not recommended
        //     and defaults to false.)
        public bool AllowPlainTextPkce { get; set; }
        //
        // 摘要:
        //     Specifies whether the client must use a request object on authorize requests
        //     (defaults to false.)
        public bool RequireRequestObject { get; set; }
        //
        // 摘要:
        //     Controls whether access tokens are transmitted via the browser for this client
        //     (defaults to false). This can prevent accidental leakage of access tokens when
        //     multiple response types are allowed.
        //
        // 值:
        //     true if access tokens can be transmitted via the browser; otherwise, false.
        public bool AllowAccessTokensViaBrowser { get; set; }
        //
        // 摘要:
        //     Specifies allowed URIs to return tokens or authorization codes to
        public  string  RedirectUris { get; set; }
        //
        // 摘要:
        //     Specifies allowed URIs to redirect to after logout
        public  string  PostLogoutRedirectUris { get; set; }
        //
        // 摘要:
        //     Specifies logout URI at client for HTTP front-channel based logout.
        public string FrontChannelLogoutUri { get; set; }
        //
        // 摘要:
        //     Specifies if the user's session id should be sent to the FrontChannelLogoutUri.
        //     Defaults to true.
        public bool FrontChannelLogoutSessionRequired { get; set; }
        //
        // 摘要:
        //     Specifies logout URI at client for HTTP back-channel based logout.
        public string BackChannelLogoutUri { get; set; }
        //
        // 摘要:
        //     Gets or sets the allowed CORS origins for JavaScript clients.
        //
        // 值:
        //     The allowed CORS origins.
        public  string  AllowedCorsOrigins { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string descriptions { get; set; }
        public string auth2claims { get; set; }

    }
}

using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class Id4server : IEntity
    {
        public string id { get; set; }
        public string AppName { get; set; }
        public string Pid { get; set; }
        public string Cid { get; set; }
        public bool? AllowOfflineAccess { get; set; }
        public int? IdentityTokenLifetime { get; set; }
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }
        public int? AccessTokenLifetime { get; set; }
        public int? AuthorizationCodeLifetime { get; set; }
        public int? AbsoluteRefreshTokenLifetime { get; set; }
        public int? SlidingRefreshTokenLifetime { get; set; }
        public int? ConsentLifetime { get; set; }
        public string RefreshTokenUsage { get; set; }
        public bool? UpdateAccessTokenClaimsOnRefresh { get; set; }
        public string RefreshTokenExpiration { get; set; }
        public string AccessTokenType { get; set; }
        public bool? EnableLocalLogin { get; set; }
        public string IdentityProviderRestrictions { get; set; }
        public bool? IncludeJwtid { get; set; }
        public string Claims { get; set; }
        public bool? AlwaysSendClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; }
        public string PairWiseSubjectSalt { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string UserCodeType { get; set; }
        public int? DeviceCodeLifetime { get; set; }
        public bool? AlwaysIncludeUserClaimsInIdToken { get; set; }
        public string AllowedScopes { get; set; }
        public string Properties { get; set; }
        public bool? BackChannelLogoutSessionRequired { get; set; }
        public string Clientid { get; set; }
        public string ProtocolType { get; set; }
        public string ClientSecrets { get; set; }
        public bool? RequireClientSecret { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool? RequireConsent { get; set; }
        public bool? AllowRememberConsent { get; set; }
        public string AllowedGrantTypes { get; set; }
        public bool? RequirePkce { get; set; }
        public bool? RequireRequestObject { get; set; }
        public bool? AllowAccessTokensViaBrowser { get; set; }
        public string RedirectUris { get; set; }
        public string PostLogoutRedirectUris { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public bool? FrontChannelLogoutSessionRequired { get; set; }
        public string BackChannelLogoutUri { get; set; }
        public string AllowedCorsOrigins { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public string Auth2claims { get; set; }
    }
}

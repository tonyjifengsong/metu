using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MSSQL
{
   public static class BaseSQL
    {
        public static string SQL_ACCOUNT= @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Account](
	[Id] [nvarchar](100) NOT NULL,
	[Type] [int] NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[LoginTime] [datetime2](7) NULL,
	[LoginIP] [nvarchar](50) NULL,
	[Status] [smallint] NULL,
	[IsLock] [bit] NULL,
	[CreatedTime] [datetime2](7) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ClosedTime] [datetime2](7) NULL,
	[ClosedBy] [uniqueidentifier] NULL,
	[Deleted] [bit] NULL,
	[DeletedTime] [datetime2](7) NULL,
	[DeletedBy] [uniqueidentifier] NULL,
	[CID] [uniqueidentifier] NULL,
	[Pid] [uniqueidentifier] NULL,
	[DomainId] [nvarchar](50) NULL,
	[isdeleted] [int] NULL,
	[isenabled] [int] NULL,
	[createdate] [datetime2](7) NULL,
	[updatedate] [datetime2](7) NULL,
	[createuserid] [uniqueidentifier] NULL,
	[updateuserid] [uniqueidentifier] NULL,
 CONSTRAINT [PK__account__3214EC071E3612B8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] END";

		public static string SQL_AspNetRoleClaims = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] END";
		public static string SQL_AspNetRoles = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END";
		public static string SQL_AspNetUserClaims = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END";
		public static string SQL_AspNetUserLogins = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END";
		public static string SQL_AspNetUserRoles = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_AspNetUsers = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[Tel] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[CDate] [datetime2](7) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END";

		public static string SQL_AspNetUserTokens = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END";
		public static string SQL_ID4Client = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ID4Client]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ID4Client](
	[ID] [nvarchar](100) NOT NULL,
	[AppName] [nvarchar](50) NULL,
	[PID] [nvarchar](100) NULL,
	[CID] [nvarchar](100) NULL,
	[AllowOfflineAccess] [bit] NULL,
	[IdentityTokenLifetime] [int] NULL,
	[AllowedIdentityTokenSigningAlgorithms] [nvarchar](4000) NULL,
	[AccessTokenLifetime] [int] NULL,
	[AuthorizationCodeLifetime] [int] NULL,
	[AbsoluteRefreshTokenLifetime] [int] NULL,
	[SlidingRefreshTokenLifetime] [int] NULL,
	[ConsentLifetime] [int] NULL,
	[RefreshTokenUsage] [nvarchar](50) NULL,
	[UpdateAccessTokenClaimsOnRefresh] [bit] NULL,
	[RefreshTokenExpiration] [nvarchar](4000) NULL,
	[AccessTokenType] [nvarchar](50) NULL,
	[EnableLocalLogin] [bit] NULL,
	[IdentityProviderRestrictions] [nvarchar](4000) NULL,
	[IncludeJwtId] [bit] NULL,
	[Claims] [nvarchar](4000) NULL,
	[AlwaysSendClientClaims] [bit] NULL,
	[ClientClaimsPrefix] [nvarchar](50) NULL,
	[PairWiseSubjectSalt] [nvarchar](50) NULL,
	[UserSsoLifetime] [int] NULL,
	[UserCodeType] [nvarchar](50) NULL,
	[DeviceCodeLifetime] [int] NULL,
	[AlwaysIncludeUserClaimsInIdToken] [bit] NULL,
	[AllowedScopes] [nvarchar](4000) NULL,
	[Properties] [nvarchar](4000) NULL,
	[BackChannelLogoutSessionRequired] [bit] NULL,
	[ClientId] [nvarchar](50) NULL,
	[ProtocolType] [nvarchar](50) NULL,
	[ClientSecrets] [nvarchar](4000) NULL,
	[RequireClientSecret] [bit] NULL,
	[ClientName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[ClientUri] [nvarchar](4000) NULL,
	[LogoUri] [nvarchar](4000) NULL,
	[RequireConsent] [bit] NULL,
	[AllowRememberConsent] [bit] NULL,
	[AllowedGrantTypes] [nvarchar](4000) NULL,
	[RequirePkce] [bit] NULL,
	[RequireRequestObject] [bit] NULL,
	[AllowAccessTokensViaBrowser] [bit] NULL,
	[RedirectUris] [nvarchar](4000) NULL,
	[PostLogoutRedirectUris] [nvarchar](4000) NULL,
	[FrontChannelLogoutUri] [nvarchar](500) NULL,
	[FrontChannelLogoutSessionRequired] [bit] NULL,
	[BackChannelLogoutUri] [nvarchar](500) NULL,
	[AllowedCorsOrigins] [nvarchar](500) NULL,
	[type] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[descriptions] [nvarchar](50) NULL,
	[auth2claims] [nvarchar](4000) NULL,
 CONSTRAINT [PK_ID4Clients] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_ID4Resource = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ID4Resource]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ID4Resource](
	[ID] [nvarchar](100) NOT NULL,
	[AppName] [nvarchar](50) NULL,
	[PID] [nvarchar](100) NULL,
	[CID] [nvarchar](100) NULL,
	[AllowOfflineAccess] [bit] NULL,
	[IdentityTokenLifetime] [int] NULL,
	[AllowedIdentityTokenSigningAlgorithms] [nvarchar](4000) NULL,
	[AccessTokenLifetime] [int] NULL,
	[AuthorizationCodeLifetime] [int] NULL,
	[AbsoluteRefreshTokenLifetime] [int] NULL,
	[SlidingRefreshTokenLifetime] [int] NULL,
	[ConsentLifetime] [int] NULL,
	[RefreshTokenUsage] [nvarchar](50) NULL,
	[UpdateAccessTokenClaimsOnRefresh] [bit] NULL,
	[RefreshTokenExpiration] [nvarchar](4000) NULL,
	[AccessTokenType] [nvarchar](50) NULL,
	[EnableLocalLogin] [bit] NULL,
	[IdentityProviderRestrictions] [nvarchar](4000) NULL,
	[IncludeJwtId] [bit] NULL,
	[Claims] [nvarchar](4000) NULL,
	[AlwaysSendClientClaims] [bit] NULL,
	[ClientClaimsPrefix] [nvarchar](50) NULL,
	[PairWiseSubjectSalt] [nvarchar](50) NULL,
	[UserSsoLifetime] [int] NULL,
	[UserCodeType] [nvarchar](50) NULL,
	[DeviceCodeLifetime] [int] NULL,
	[AlwaysIncludeUserClaimsInIdToken] [bit] NULL,
	[AllowedScopes] [nvarchar](4000) NULL,
	[Properties] [nvarchar](4000) NULL,
	[BackChannelLogoutSessionRequired] [bit] NULL,
	[ClientId] [nvarchar](50) NULL,
	[ProtocolType] [nvarchar](50) NULL,
	[ClientSecrets] [nvarchar](4000) NULL,
	[RequireClientSecret] [bit] NULL,
	[ClientName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[ClientUri] [nvarchar](4000) NULL,
	[LogoUri] [nvarchar](4000) NULL,
	[RequireConsent] [bit] NULL,
	[AllowRememberConsent] [bit] NULL,
	[AllowedGrantTypes] [nvarchar](4000) NULL,
	[RequirePkce] [bit] NULL,
	[RequireRequestObject] [bit] NULL,
	[AllowAccessTokensViaBrowser] [bit] NULL,
	[RedirectUris] [nvarchar](4000) NULL,
	[PostLogoutRedirectUris] [nvarchar](4000) NULL,
	[FrontChannelLogoutUri] [nvarchar](500) NULL,
	[FrontChannelLogoutSessionRequired] [bit] NULL,
	[BackChannelLogoutUri] [nvarchar](500) NULL,
	[AllowedCorsOrigins] [nvarchar](500) NULL,
	[type] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[descriptions] [nvarchar](50) NULL,
	[auth2claims] [nvarchar](4000) NULL,
 CONSTRAINT [PK_ID4Resources] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";

		public static string SQL_ID4Server = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ID4Server]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ID4Server](
	[ID] [nvarchar](100) NOT NULL,
	[AppName] [nvarchar](50) NULL,
	[PID] [nvarchar](100) NULL,
	[CID] [nvarchar](100) NULL,
	[AllowOfflineAccess] [bit] NULL,
	[IdentityTokenLifetime] [int] NULL,
	[AllowedIdentityTokenSigningAlgorithms] [nvarchar](4000) NULL,
	[AccessTokenLifetime] [int] NULL,
	[AuthorizationCodeLifetime] [int] NULL,
	[AbsoluteRefreshTokenLifetime] [int] NULL,
	[SlidingRefreshTokenLifetime] [int] NULL,
	[ConsentLifetime] [int] NULL,
	[RefreshTokenUsage] [nvarchar](50) NULL,
	[UpdateAccessTokenClaimsOnRefresh] [bit] NULL,
	[RefreshTokenExpiration] [nvarchar](4000) NULL,
	[AccessTokenType] [nvarchar](50) NULL,
	[EnableLocalLogin] [bit] NULL,
	[IdentityProviderRestrictions] [nvarchar](4000) NULL,
	[IncludeJwtId] [bit] NULL,
	[Claims] [nvarchar](4000) NULL,
	[AlwaysSendClientClaims] [bit] NULL,
	[ClientClaimsPrefix] [nvarchar](50) NULL,
	[PairWiseSubjectSalt] [nvarchar](50) NULL,
	[UserSsoLifetime] [int] NULL,
	[UserCodeType] [nvarchar](50) NULL,
	[DeviceCodeLifetime] [int] NULL,
	[AlwaysIncludeUserClaimsInIdToken] [bit] NULL,
	[AllowedScopes] [nvarchar](4000) NULL,
	[Properties] [nvarchar](4000) NULL,
	[BackChannelLogoutSessionRequired] [bit] NULL,
	[ClientId] [nvarchar](50) NULL,
	[ProtocolType] [nvarchar](50) NULL,
	[ClientSecrets] [nvarchar](4000) NULL,
	[RequireClientSecret] [bit] NULL,
	[ClientName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[ClientUri] [nvarchar](4000) NULL,
	[LogoUri] [nvarchar](4000) NULL,
	[RequireConsent] [bit] NULL,
	[AllowRememberConsent] [bit] NULL,
	[AllowedGrantTypes] [nvarchar](4000) NULL,
	[RequirePkce] [bit] NULL,
	[RequireRequestObject] [bit] NULL,
	[AllowAccessTokensViaBrowser] [bit] NULL,
	[RedirectUris] [nvarchar](4000) NULL,
	[PostLogoutRedirectUris] [nvarchar](4000) NULL,
	[FrontChannelLogoutUri] [nvarchar](500) NULL,
	[FrontChannelLogoutSessionRequired] [bit] NULL,
	[BackChannelLogoutUri] [nvarchar](500) NULL,
	[AllowedCorsOrigins] [nvarchar](500) NULL,
	[type] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[descriptions] [nvarchar](50) NULL,
	[auth2claims] [nvarchar](4000) NULL,
 CONSTRAINT [PK_ID4Server] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_Menu = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Menu]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Menu](
	[Id] [nvarchar](100) NOT NULL,
	[DOMAINID] [nvarchar](100) NULL,
	[ModuleCode] [varchar](50) NULL,
	[Type] [smallint] NULL,
	[ParentId] [nvarchar](100) NULL,
	[Name] [nvarchar](50) NULL,
	[RouteName] [nvarchar](100) NULL,
	[RouteParams] [nvarchar](500) NULL,
	[RouteQuery] [nvarchar](500) NULL,
	[Icon] [nvarchar](20) NULL,
	[IconColor] [nvarchar](10) NULL,
	[Url] [nvarchar](255) NULL,
	[Level] [int] NULL,
	[Show] [bit] NULL,
	[Sort] [int] NULL,
	[Target] [smallint] NULL,
	[DialogWidth] [nvarchar](20) NULL,
	[DialogHeight] [nvarchar](20) NULL,
	[DialogFullscreen] [bit] NULL,
	[Remarks] [nvarchar](255) NULL,
	[CreatedTime] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[CID] [nvarchar](100) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
 CONSTRAINT [PK__menu__3214EC0736145F4E] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_Strategy = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Strategy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Strategy](
	[Id] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[StrategyName] [nvarchar](50) NULL,
	[IsEnabled] [smallint] NULL,
	[CID] [nvarchar](100) NULL,
	[PID] [nvarchar](100) NULL,
	[CreateUserId] [nvarchar](100) NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [smallint] NULL
) ON [PRIMARY]
END";

		public static string SQL_StrategyServices = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StrategyServices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StrategyServices](
	[Id] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[ServiceName] [nvarchar](50) NULL,
	[ServiceDesc] [nvarchar](50) NULL,
	[SysPageServiceId] [nvarchar](100) NULL,
	[ServiceOrder] [int] NULL,
	[IsEnabled] [smallint] NULL,
	[CID] [nvarchar](100) NULL,
	[PID] [nvarchar](100) NULL,
	[CreateUserId] [nvarchar](100) NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [smallint] NULL
) ON [PRIMARY]
END";
		public static string SQL_SYSAccountRole = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SYSAccountRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SYSAccountRole](
	[Id] [nvarchar](100) NOT NULL,
	[Softtype] [nvarchar](100) NULL,
	[AccountId] [nvarchar](100) NULL,
	[RoleId] [nvarchar](100) NULL,
	[CID] [nvarchar](100) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
 CONSTRAINT [PK_SYSAccount_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_sysAdmin = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sysAdmin]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[sysAdmin](
	[Id] [nvarchar](100) NOT NULL,
	[CreateDate] [datetime] NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
 CONSTRAINT [PK_sysAdmin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_SysPage = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SysPage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SysPage](
	[ID] [nvarchar](100) NOT NULL,
	[IDOrderNO] [bigint] IDENTITY(1,1) NOT NULL,
	[PageName] [varchar](250) NOT NULL,
	[ObjectType] [varchar](250) NULL,
	[ObjectName] [varchar](250) NULL,
	[PageNameEns] [nvarchar](250) NULL,
	[sourcedata] [nvarchar](550) NULL,
	[InterfaceAddress] [nvarchar](1000) NULL,
	[CID] [nvarchar](100) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[CreateDate] [datetime] NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
	[version] [nvarchar](50) NULL,
	[cname] [nvarchar](100) NULL,
	[appname] [nvarchar](100) NULL,
	[pid] [nvarchar](100) NULL,
 CONSTRAINT [PK_SysPage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_SysPageConfigs = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SysPageConfigs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SysPageConfigs](
	[ID] [nvarchar](100) NOT NULL,
	[SysPageID] [nvarchar](100) NULL,
	[ControlName] [varchar](150) NULL,
	[ControlCaption] [varchar](150) NULL,
	[ControlType] [varchar](150) NULL,
	[Placeholder] [varchar](150) NULL,
	[Require] [bit] NULL,
	[Msg] [varchar](150) NULL,
	[Explain] [varchar](150) NULL,
	[Enabled] [bit] NULL,
	[SourceData] [varchar](4500) NULL,
	[InterfaceAddress] [varchar](4000) NULL,
	[ControlOrder] [int] NULL,
	[IsGroup] [bit] NULL,
	[GroupField] [varchar](150) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[CreateDate] [datetime] NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
	[CID] [nvarchar](100) NULL,
	[version] [nvarchar](50) NULL,
 CONSTRAINT [PK_SysPageConfigs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_SYSPageDic = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SYSPageDic]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SYSPageDic](
	[SetKey] [nvarchar](50) NULL,
	[SortName] [nvarchar](50) NULL,
	[SortCode] [nvarchar](50) NULL,
	[Rank] [nvarchar](50) NULL,
	[parentid] [nvarchar](100) NULL,
	[ID] [nvarchar](100) NOT NULL,
	[pid] [nvarchar](100) NULL,
	[cid] [nvarchar](100) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[CreateDate] [datetime] NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
	[version] [nvarchar](50) NULL,
	[cname] [nvarchar](64) NULL,
	[appname] [nvarchar](64) NULL,
 CONSTRAINT [PK__SYSPageDic__3214EC075FA57F8D] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END"; 
		public static string SQL_SYSPageDICConfig = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SYSPageDICConfig]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SYSPageDICConfig](
	[SetKey] [nvarchar](50) NULL,
	[ConfigName] [nvarchar](50) NULL,
	[ConfigValue] [nvarchar](max) NULL,
	[ConfigExplain] [nvarchar](255) NULL,
	[Rank] [nvarchar](50) NULL,
	[ID] [nvarchar](100) NOT NULL,
	[PID] [nvarchar](100) NULL,
	[CID] [nvarchar](100) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
	[DICID] [varchar](50) NULL,
	[version] [nvarchar](50) NULL,
 CONSTRAINT [PK__SYSPageDICConfig__3214EC279FD78795] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END";
		public static string SQL_SysPageService = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SysPageService]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SysPageService](
	[Id] [nvarchar](100) NOT NULL,
	[DOMAINID] [nvarchar](100) NULL,
	[servicename] [nvarchar](255) NULL,
	[ServiceEvents] [nvarchar](max) NULL,
	[Remarks] [nvarchar](2555) NULL,
	[ReturnType] [nvarchar](50) NULL,
	[CreatedTime] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedTime] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[CID] [nvarchar](100) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[CreateDate] [datetime] NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
	[version] [nvarchar](50) NULL,
	[methodname] [nvarchar](250) NULL,
	[pid] [nvarchar](100) NULL,
	[appname] [nvarchar](100) NULL,
	[cname] [nvarchar](100) NULL,
 CONSTRAINT [PK_SysPageService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END";
		public static string SQL_SYSRoleMenu = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SYSRoleMenu]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SYSRoleMenu](
	[Id] [nvarchar](100) NOT NULL,
	[softtype] [nvarchar](100) NOT NULL,
	[RoleId] [nvarchar](100) NOT NULL,
	[MenuId] [nvarchar](100) NOT NULL,
	[CID] [nvarchar](100) NULL,
	[UpdateUserID] [nvarchar](100) NULL,
	[CreateUserID] [nvarchar](100) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NULL,
	[IsEnabled] [smallint] NULL,
	[IsDeleted] [smallint] NULL,
 CONSTRAINT [PK__role_menu__3214EC0772E9B7E0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END";
		public static string SQL_addextendedproperty = @"IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]'))
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]'))
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]'))
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]'))
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]'))
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]'))
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]'))
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]'))
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Type'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Type'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'UserName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'UserName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Password'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Password'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Name'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Name'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Phone'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Phone'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Email'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Email'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'LoginTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'LoginTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'LoginIP'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后登录IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'LoginIP'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Status'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态：0、未激活 1、正常 2、禁用 3、注销' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Status'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'CreatedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'CreatedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'CreatedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'ModifiedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'ModifiedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'ModifiedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'ModifiedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'ClosedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注销时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'ClosedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'ClosedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注销人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'ClosedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Deleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'DeletedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'DeletedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'DeletedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'DeletedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Account', N'COLUMN',N'Pid'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Account', @level2type=N'COLUMN',@level2name=N'Pid'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Id'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'DOMAINID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'软件分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'DOMAINID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'ModuleCode'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属模块' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'ModuleCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Type'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型，0、节点 1、链接' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Type'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'ParentId'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父菜单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Name'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Name'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'RouteName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'路由名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'RouteName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'RouteParams'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'路由参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'RouteParams'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'RouteQuery'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'路由参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'RouteQuery'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Icon'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Icon'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'IconColor'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图标颜色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'IconColor'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Url'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Url'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Level'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Level'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Show'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Show'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Sort'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Sort'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Target'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'打开方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Target'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'DialogWidth'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对话框宽度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'DialogWidth'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'DialogHeight'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对话框高度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'DialogHeight'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'DialogFullscreen'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对话框可全屏' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'DialogFullscreen'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'Remarks'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Remarks'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'CreatedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreatedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'CreatedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'ModifiedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'ModifiedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'ModifiedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'ModifiedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'CID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业所属ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UpdateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'CreateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'UpdateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'CreateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'IsEnabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Menu', N'COLUMN',N'IsDeleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'Softtype'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'软件分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'Softtype'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'AccountId'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'AccountId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'RoleId'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'CID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'CID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'UpdateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'CreateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'UpdateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'CreateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'IsEnabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSAccountRole', N'COLUMN',N'IsDeleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSAccountRole', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'PageName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'PageName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'ObjectType'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对象类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'ObjectType'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'ObjectName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对象名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'ObjectName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'PageNameEns'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面英文名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'PageNameEns'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'CID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属企业ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'CID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'UpdateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'CreateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'UpdateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新DATE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'CreateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加DATE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'IsEnabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPage', N'COLUMN',N'IsDeleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPage', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'SysPageID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'SysPageID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'ControlName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'控件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'ControlName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'ControlCaption'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'控件标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'ControlCaption'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'ControlType'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'控件类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'ControlType'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'Placeholder'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'输入框里面的文字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'Placeholder'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'Require'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'必填项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'Require'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'Msg'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'必填的提示信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'Msg'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'Explain'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'输入框下方说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'Explain'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'Enabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可修改' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'Enabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'SourceData'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'SourceData'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'InterfaceAddress'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接口地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'InterfaceAddress'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'ControlOrder'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'控件顺序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'ControlOrder'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'IsGroup'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否组合' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'IsGroup'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'GroupField'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组合字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'GroupField'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'UpdateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'CreateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'UpdateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新DATE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'CreateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加DATE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'IsEnabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'IsDeleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageConfigs', N'COLUMN',N'CID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属企业ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageConfigs', @level2type=N'COLUMN',@level2name=N'CID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'SetKey'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'SetKey'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'SortName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'SortName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'SortCode'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'SortCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'Rank'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'Rank'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'parentid'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置上级菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'parentid'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'pid'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'低级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'pid'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'cid'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'cid'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'UpdateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'CreateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'UpdateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'、更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'CreateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'IsEnabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', N'COLUMN',N'IsDeleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDic', NULL,NULL))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典主表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDic'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'SetKey'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'SetKey'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'ConfigName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'ConfigName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'ConfigValue'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'ConfigValue'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'ConfigExplain'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'ConfigExplain'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'Rank'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'Rank'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'PID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父类ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'PID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'CID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'CID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'UpdateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'CreateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'UpdateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'CreateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'IsEnabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'IsDeleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSPageDICConfig', N'COLUMN',N'DICID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置分组ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSPageDICConfig', @level2type=N'COLUMN',@level2name=N'DICID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'DOMAINID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'DOMAINID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'servicename'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'servicename'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'ServiceEvents'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'ServiceEvents'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'Remarks'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'Remarks'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'CreatedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'CreatedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'CreatedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'ModifiedTime'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'ModifiedTime'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SysPageService', N'COLUMN',N'ModifiedBy'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPageService', @level2type=N'COLUMN',@level2name=N'ModifiedBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'softtype'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'软件分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'softtype'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'RoleId'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'MenuId'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'CID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'CID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'UpdateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'CreateUserID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'UpdateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'CreateDate'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'IsEnabled'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'SYSRoleMenu', N'COLUMN',N'IsDeleted'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYSRoleMenu', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
";
		public static string SQL_DefaultValue = @"SET ANSI_PADDING ON
GO
/****** Object:  Index [AK_KEY_1_SYSPAGE]    Script Date: 2022/4/18 17:25:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SysPage]') AND name = N'AK_KEY_1_SYSPAGE')
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [AK_KEY_1_SYSPAGE] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Account_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Account__LoginTi__5DCAEF64]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__LoginTi__5DCAEF64]  DEFAULT (getdate()) FOR [LoginTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Account__IsLock__5EBF139D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__IsLock__5EBF139D]  DEFAULT ((0)) FOR [IsLock]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Account__Created__5FB337D6]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__Created__5FB337D6]  DEFAULT (getdate()) FOR [CreatedTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Account__Modifie__60A75C0F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__Modifie__60A75C0F]  DEFAULT (getdate()) FOR [ModifiedTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Account__ClosedT__619B8048]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__ClosedT__619B8048]  DEFAULT (getdate()) FOR [ClosedTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Account__Deleted__628FA481]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__Deleted__628FA481]  DEFAULT ((0)) FOR [Deleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Account__Deleted__6383C8BA]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__Deleted__6383C8BA]  DEFAULT (getdate()) FOR [DeletedTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Account_isdeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_isdeleted]  DEFAULT ((0)) FOR [isdeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Account_isenabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_isenabled]  DEFAULT ((1)) FOR [isenabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Account_createdate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_createdate]  DEFAULT (getdate()) FOR [createdate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Account_updatedate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_updatedate]  DEFAULT (getdate()) FOR [updatedate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_AspNetRoles_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AspNetRoles] ADD  CONSTRAINT [DF_AspNetRoles_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_AspNetUsers_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AspNetUsers] ADD  CONSTRAINT [DF_AspNetUsers_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_ID4Clients_AppName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ID4Client] ADD  CONSTRAINT [DF_ID4Clients_AppName]  DEFAULT (N'metu') FOR [AppName]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_ID4Resources_AppName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ID4Resource] ADD  CONSTRAINT [DF_ID4Resources_AppName]  DEFAULT (N'metu') FOR [AppName]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_ID4Server_AppName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ID4Server] ADD  CONSTRAINT [DF_ID4Server_AppName]  DEFAULT (N'metu') FOR [AppName]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_Level]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_Level]  DEFAULT ((0)) FOR [Level]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_Show]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_Show]  DEFAULT ((1)) FOR [Show]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_Sort]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_Sort]  DEFAULT ((1)) FOR [Sort]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_Target]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_Target]  DEFAULT ((0)) FOR [Target]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_DialogFullscreen]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_DialogFullscreen]  DEFAULT ((1)) FOR [DialogFullscreen]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_CreatedTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_CreatedTime]  DEFAULT (getdate()) FOR [CreatedTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_ModifiedTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_ModifiedTime]  DEFAULT (getdate()) FOR [ModifiedTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Menu_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_IsDeleted]  DEFAULT ((1)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Strategy_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Strategy] ADD  CONSTRAINT [DF_Strategy_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Strategy_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Strategy] ADD  CONSTRAINT [DF_Strategy_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Strategy_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Strategy] ADD  CONSTRAINT [DF_Strategy_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Strategy_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Strategy] ADD  CONSTRAINT [DF_Strategy_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Strategy_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Strategy] ADD  CONSTRAINT [DF_Strategy_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_StrategyServices_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[StrategyServices] ADD  CONSTRAINT [DF_StrategyServices_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_StrategyServices_ServiceOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[StrategyServices] ADD  CONSTRAINT [DF_StrategyServices_ServiceOrder]  DEFAULT ((0)) FOR [ServiceOrder]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_StrategyServices_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[StrategyServices] ADD  CONSTRAINT [DF_StrategyServices_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_StrategyServices_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[StrategyServices] ADD  CONSTRAINT [DF_StrategyServices_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_StrategyServices_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[StrategyServices] ADD  CONSTRAINT [DF_StrategyServices_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_StrategyServices_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[StrategyServices] ADD  CONSTRAINT [DF_StrategyServices_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSAccountRole_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSAccountRole] ADD  CONSTRAINT [DF_SYSAccountRole_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSAccountRole_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSAccountRole] ADD  CONSTRAINT [DF_SYSAccountRole_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSAccountRole_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSAccountRole] ADD  CONSTRAINT [DF_SYSAccountRole_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSAccountRole_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSAccountRole] ADD  CONSTRAINT [DF_SYSAccountRole_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSAccountRole_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSAccountRole] ADD  CONSTRAINT [DF_SYSAccountRole_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_sysAdmin_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[sysAdmin] ADD  CONSTRAINT [DF_sysAdmin_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_sysAdmin_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[sysAdmin] ADD  CONSTRAINT [DF_sysAdmin_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_sysAdmin_Password]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[sysAdmin] ADD  CONSTRAINT [DF_sysAdmin_Password]  DEFAULT ((123456789)) FOR [Password]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_sysAdmin_Remark]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[sysAdmin] ADD  CONSTRAINT [DF_sysAdmin_Remark]  DEFAULT (N'memo') FOR [Remark]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_ID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_ID]  DEFAULT (newid()) FOR [ID]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_version]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_version]  DEFAULT ((0)) FOR [version]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_cname]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_cname]  DEFAULT (N'metu') FOR [cname]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPage_appname]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPage] ADD  CONSTRAINT [DF_SysPage_appname]  DEFAULT (N'metu') FOR [appname]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_ID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_ID]  DEFAULT (newid()) FOR [ID]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_Enabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_Enabled]  DEFAULT ((1)) FOR [Enabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_ControlOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_ControlOrder]  DEFAULT ((100)) FOR [ControlOrder]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_IsGroup]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_IsGroup]  DEFAULT ((1)) FOR [IsGroup]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageConfigs_version]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageConfigs] ADD  CONSTRAINT [DF_SysPageConfigs_version]  DEFAULT ((0)) FOR [version]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_ID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_ID]  DEFAULT (newid()) FOR [ID]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_version]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_version]  DEFAULT ((0)) FOR [version]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_Cname]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_Cname]  DEFAULT (N'metu') FOR [cname]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDic_AppName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDic] ADD  CONSTRAINT [DF_SYSPageDic_AppName]  DEFAULT (N'metuapp') FOR [appname]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDICConfig_ID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDICConfig] ADD  CONSTRAINT [DF_SYSPageDICConfig_ID]  DEFAULT (newid()) FOR [ID]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDICConfig_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDICConfig] ADD  CONSTRAINT [DF_SYSPageDICConfig_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDICConfig_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDICConfig] ADD  CONSTRAINT [DF_SYSPageDICConfig_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDICConfig_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDICConfig] ADD  CONSTRAINT [DF_SYSPageDICConfig_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDICConfig_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDICConfig] ADD  CONSTRAINT [DF_SYSPageDICConfig_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSPageDICConfig_version]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSPageDICConfig] ADD  CONSTRAINT [DF_SYSPageDICConfig_version]  DEFAULT ((0)) FOR [version]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_servicename]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_servicename]  DEFAULT (N'service') FOR [servicename]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_CreatedTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_CreatedTime]  DEFAULT (getdate()) FOR [CreatedTime]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_version]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_version]  DEFAULT ((0)) FOR [version]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_methodname]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_methodname]  DEFAULT (N'call') FOR [methodname]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_pid]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_pid]  DEFAULT (N'metu') FOR [pid]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_appname]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_appname]  DEFAULT (N'metu') FOR [appname]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SysPageService_cname]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SysPageService] ADD  CONSTRAINT [DF_SysPageService_cname]  DEFAULT (N'metu') FOR [cname]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSRoleMenu_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSRoleMenu] ADD  CONSTRAINT [DF_SYSRoleMenu_Id]  DEFAULT (newid()) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSRoleMenu_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSRoleMenu] ADD  CONSTRAINT [DF_SYSRoleMenu_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSRoleMenu_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSRoleMenu] ADD  CONSTRAINT [DF_SYSRoleMenu_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSRoleMenu_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSRoleMenu] ADD  CONSTRAINT [DF_SYSRoleMenu_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_SYSRoleMenu_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SYSRoleMenu] ADD  CONSTRAINT [DF_SYSRoleMenu_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
END
GO
";
		public static string SQL_InitialData = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'00000000-0000-0000-0000-000000000000', NULL, N'tony', NULL, NULL, NULL, NULL, CAST(N'2021-10-06T05:59:58.6970000' AS DateTime2), NULL, NULL, 0, CAST(N'2021-10-06T05:59:58.6970000' AS DateTime2), NULL, CAST(N'2021-10-06T05:59:58.6970000' AS DateTime2), NULL, CAST(N'2021-10-06T05:59:58.6970000' AS DateTime2), NULL, 0, CAST(N'2021-10-06T05:59:58.6970000' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'02333C65-C68A-417D-88C1-17FAF4F19364', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'1F57184E-9A07-4B84-9F57-950DBA22BB6D', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'27B9D961-E268-4F58-A453-9D41034F73AB', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'2f6c7d85-fdc2-4419-925f-4256d62b8501', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'3B2EE1FC-43C8-42CA-82A7-7ADDE1931495', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'44358A1A-E6FA-432D-A182-156DA9E3B2FF', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'551d3d88-241f-4267-820c-2129657cea17', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'74F5F329-8005-4413-9388-D8E15B20B37E', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'7504CCA5-626B-47F2-B2D4-0694D0F807D0', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'925415e1-a0e3-49a2-be42-b785401aa109', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'9d541ae5-3868-49a0-b08e-c9b75d27cf62', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'A9A9C31D-3FA3-4F7F-8EAA-D52ACA5DD32A', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'BF85C2C6-A37E-4F40-B60B-CB8CBFA4B2B3', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'C99DC6E6-5F8F-461D-94B9-21598EB4733B', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'c9a32c40-0e96-449f-afdd-15dbe9ba4e24', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'd1b4c5dd-1e17-42ee-822b-edeb147cc8bb', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'DA5F835F-7DC1-4A4C-BEC7-74F288A28E2E', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'dd5f59ae-4835-45b6-a732-2d307eff4dc3', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'EA7C9400-4D25-4C6F-924E-16D76EC5C9F5', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Account] ([Id], [Type], [UserName], [Password], [Name], [Phone], [Email], [LoginTime], [LoginIP], [Status], [IsLock], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [ClosedTime], [ClosedBy], [Deleted], [DeletedTime], [DeletedBy], [CID], [Pid], [DomainId], [isdeleted], [isenabled], [createdate], [updatedate], [createuserid], [updateuserid]) VALUES (N'EC53F0CC-C128-4D70-8B39-A52DAC780A88', 0, N'tony', NULL, NULL, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[ID4Server] ([ID], [AppName], [PID], [CID], [AllowOfflineAccess], [IdentityTokenLifetime], [AllowedIdentityTokenSigningAlgorithms], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [ConsentLifetime], [RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IdentityProviderRestrictions], [IncludeJwtId], [Claims], [AlwaysSendClientClaims], [ClientClaimsPrefix], [PairWiseSubjectSalt], [UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [AlwaysIncludeUserClaimsInIdToken], [AllowedScopes], [Properties], [BackChannelLogoutSessionRequired], [ClientId], [ProtocolType], [ClientSecrets], [RequireClientSecret], [ClientName], [Description], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowedGrantTypes], [RequirePkce], [RequireRequestObject], [AllowAccessTokensViaBrowser], [RedirectUris], [PostLogoutRedirectUris], [FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], [BackChannelLogoutUri], [AllowedCorsOrigins], [type], [name], [descriptions], [auth2claims]) VALUES (N'1111111111111111', N'1111', N'1111', N'11111111', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'clientid', NULL, NULL, NULL, N'clientname', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'http://www.baidu.com;http://www.163.com;http://www.263.com', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[ID4Server] ([ID], [AppName], [PID], [CID], [AllowOfflineAccess], [IdentityTokenLifetime], [AllowedIdentityTokenSigningAlgorithms], [AccessTokenLifetime], [AuthorizationCodeLifetime], [AbsoluteRefreshTokenLifetime], [SlidingRefreshTokenLifetime], [ConsentLifetime], [RefreshTokenUsage], [UpdateAccessTokenClaimsOnRefresh], [RefreshTokenExpiration], [AccessTokenType], [EnableLocalLogin], [IdentityProviderRestrictions], [IncludeJwtId], [Claims], [AlwaysSendClientClaims], [ClientClaimsPrefix], [PairWiseSubjectSalt], [UserSsoLifetime], [UserCodeType], [DeviceCodeLifetime], [AlwaysIncludeUserClaimsInIdToken], [AllowedScopes], [Properties], [BackChannelLogoutSessionRequired], [ClientId], [ProtocolType], [ClientSecrets], [RequireClientSecret], [ClientName], [Description], [ClientUri], [LogoUri], [RequireConsent], [AllowRememberConsent], [AllowedGrantTypes], [RequirePkce], [RequireRequestObject], [AllowAccessTokensViaBrowser], [RedirectUris], [PostLogoutRedirectUris], [FrontChannelLogoutUri], [FrontChannelLogoutSessionRequired], [BackChannelLogoutUri], [AllowedCorsOrigins], [type], [name], [descriptions], [auth2claims]) VALUES (N'22222222', N'22', N'2222', N'22222222', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'clientid', NULL, NULL, NULL, N'clientname', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'http://www.baidu.com;http://www.163.com;http://www.263.com', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'036018A7-93AD-4132-BFEC-2AB1C7B22EBA', N'WMS', NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'盘点管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'03C05B81-F558-42CC-A679-8DC7D9E05783', NULL, NULL, NULL, NULL, N'库存管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'04D166CD-80B1-40D7-8898-134650AF9150', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'补货管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'0F8412D5-BF4E-4F29-A3FA-F39C8494EF75', NULL, N'00', NULL, N'2084AE9D-31F9-4725-A8D7-7617784E2790', N'仓库设置', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'17F51D71-9995-45C7-B8C6-F4AC1BB73AE2', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'库存调整', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'2084AE9D-31F9-4725-A8D7-7617784E2790', NULL, N'WMS', NULL, NULL, N'系统设置', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'22119DBD-17B8-4172-95CA-BF3BD82698A5', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'移位管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'2F96A908-EDD6-44EE-8752-57AF2F639EA5', NULL, N'01', NULL, N'2084AE9D-31F9-4725-A8D7-7617784E2790', N'分区设置', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'32D13CE4-20DF-4098-A483-607EB96D2CB3', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'库存锁定管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'3400CEEA-FC95-41EB-A567-69570B5B2F10', NULL, N'02', NULL, N'2084AE9D-31F9-4725-A8D7-7617784E2790', N'储位设置', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'3B8B7945-7B24-4392-963F-66A721E94220', NULL, N'03', NULL, N'2084AE9D-31F9-4725-A8D7-7617784E2790', N'产品设置', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'582D6033-B420-42B7-9FB4-038590B63A3E', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'交付订单管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'673DA08C-3AC2-464A-8EB8-CC56FD92508E', NULL, NULL, NULL, N'EFF6EAF6-38AA-4C74-AEE1-2DA5365F84A2', N'出库通知', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'69D50424-8386-459A-AAC4-C756ABF36917', NULL, N'WMS', NULL, NULL, N'报表管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'6BED9ABC-CB8D-412D-AEAC-548AA5CD8725', NULL, N'WMS', NULL, NULL, N'集成管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'6EED8F76-2C81-46BF-A30A-E66DF7F9EB17', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'入库质检', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'883D0234-DF97-443A-A248-2D5B5D096DED', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'出库拼托包装', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'88CD6ECC-E5E1-4BAF-9ECE-55AD75F87679', NULL, N'WMS', NULL, NULL, N'SMT管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'8B1273D7-C2E9-4B61-BC8F-56019D434272', NULL, NULL, NULL, N'EFF6EAF6-38AA-4C74-AEE1-2DA5365F84A2', N'波次管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'9653670F-BE61-4C03-8CA4-C4D8EA2176AF', NULL, NULL, NULL, N'EFF6EAF6-38AA-4C74-AEE1-2DA5365F84A2', N'拣货管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'96C61873-0F4D-4FB3-9E25-4F20989AD4F7', NULL, NULL, NULL, N'2084AE9D-31F9-4725-A8D7-7617784E2790', N'产品分类', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'9835BB41-357B-47D6-A7C6-BC6ED159CE09', NULL, NULL, NULL, N'A0DA81DD-0AF3-4CCF-85B7-E46B06C2A2D7', N'库位预约管理等', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'A0DA81DD-0AF3-4CCF-85B7-E46B06C2A2D7', NULL, N'WMS', NULL, NULL, N'入库', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'A0FB1EF1-C8AC-409E-97B1-4C12183BD54D', NULL, NULL, NULL, N'69D50424-8386-459A-AAC4-C756ABF36917', N'库存预警', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'A44076A9-1F83-4784-8EEC-3D0E96FBD627', NULL, NULL, NULL, N'69D50424-8386-459A-AAC4-C756ABF36917', N'交付状态跟踪', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'BDCF872C-A94A-4D0A-88DC-D2ADE1D99422', NULL, NULL, NULL, N'69D50424-8386-459A-AAC4-C756ABF36917', N'盘点管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'BDF7045E-13CC-4C13-9E66-4E157CD402F6', NULL, NULL, NULL, N'69D50424-8386-459A-AAC4-C756ABF36917', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'DC9F0A9A-0746-40F8-9171-33CA6BB8E29D', NULL, NULL, NULL, N'A0DA81DD-0AF3-4CCF-85B7-E46B06C2A2D7', N'入库通知', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'E24BAE4C-9BED-48D5-AC7A-D40257DEC516', NULL, NULL, NULL, N'EFF6EAF6-38AA-4C74-AEE1-2DA5365F84A2', N'集货管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'E3ECDE07-E672-4581-9048-329C5BB4E865', NULL, NULL, NULL, N'A0DA81DD-0AF3-4CCF-85B7-E46B06C2A2D7', N'收货管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'EC293C20-7719-4CE2-BD61-B5456F70BBF1', NULL, NULL, NULL, N'A0DA81DD-0AF3-4CCF-85B7-E46B06C2A2D7', N'检验管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'EFC58575-70E6-4CEB-80D9-DC6441BE9388', NULL, NULL, NULL, N'EFF6EAF6-38AA-4C74-AEE1-2DA5365F84A2', N'发货管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'EFF6EAF6-38AA-4C74-AEE1-2DA5365F84A2', NULL, N'WMS', NULL, NULL, N'出库', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'F5525638-7494-4DAB-BCF3-7CE25F8FAABB', NULL, NULL, NULL, N'EFF6EAF6-38AA-4C74-AEE1-2DA5365F84A2', N'库位分配管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'F77B7020-4774-4040-AA2B-79D3930ED185', NULL, NULL, NULL, N'A0DA81DD-0AF3-4CCF-85B7-E46B06C2A2D7', N'上架管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'FB33F366-29DA-4EDB-B7B3-3DB1B7513CEC', NULL, NULL, NULL, N'03C05B81-F558-42CC-A679-8DC7D9E05783', N'报费管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Menu] ([Id], [DOMAINID], [ModuleCode], [Type], [ParentId], [Name], [RouteName], [RouteParams], [RouteQuery], [Icon], [IconColor], [Url], [Level], [Show], [Sort], [Target], [DialogWidth], [DialogHeight], [DialogFullscreen], [Remarks], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted]) VALUES (N'FBBF3B46-1217-4A87-9E8B-6E168CCBD223', NULL, NULL, NULL, N'A0DA81DD-0AF3-4CCF-85B7-E46B06C2A2D7', N'装箱管理', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'0246DFB1-145F-4B99-8BBB-2A024B7974A0', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'136C1A39-B6F5-48EA-B86E-88C095101CBC', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'37ED24CD-8394-483A-8472-8E5507C7528B', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'75DDDED4-1738-431C-83EF-AE69B2885B8D', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'97eaa080-b849-4a40-b066-3bf8b934e185', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'aa013dd3-225d-46a9-97e9-ee79cc83be2a', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'AD06568B-C6A5-4C6B-87E3-5AE13FC25066', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'AE66A01E-7881-49A8-AB0F-B49B364BC6F7', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'afb6b09a-2e64-4c15-91a0-e91fb0ced043', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'B26384B3-78E5-452A-B4AC-224BCC9DCAF8', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'BE6ABDD2-FEFE-4FAD-B50D-2D75ADECBA04', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'BE9D6AF4-58FC-40FA-9021-B9617AF4A41B', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'CB16EC65-DD01-4B5E-9567-F24E362C3E0E', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'd76372ff-1270-420d-b75f-7f76c343c75a', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'd810bf2a-84ea-40cf-a3ce-dc0c7a99f1e4', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'DF03D408-0408-4651-AE05-B2B8575B4AD3', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'ebb0b0c3-f3b6-4b60-a9e3-3988bea03733', NULL, N'tony', NULL, NULL)
GO
INSERT [dbo].[sysAdmin] ([Id], [CreateDate], [UserName], [Password], [Remark]) VALUES (N'f864e978-704f-426f-b318-123781df86e5', NULL, N'tony', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[SysPage] ON 
GO
INSERT [dbo].[SysPage] ([ID], [IDOrderNO], [PageName], [ObjectType], [ObjectName], [PageNameEns], [sourcedata], [InterfaceAddress], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [cname], [appname], [pid]) VALUES (N'00000000-0000-0000-0000-000000000001', 1, N'WMS服务配置', N'WMSservicePage', N'WMSservicePage', N'WMSservicePage', NULL, NULL, N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2019-09-07T00:00:00.000' AS DateTime), NULL, 1, 0, N'0', N'metu', N'metu', N'ES')
GO
INSERT [dbo].[SysPage] ([ID], [IDOrderNO], [PageName], [ObjectType], [ObjectName], [PageNameEns], [sourcedata], [InterfaceAddress], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [cname], [appname], [pid]) VALUES (N'11C80289-8B69-4EC7-A661-D1FD8DC9D85B', 9, N'平台-货主审核', N'table', N'{objectame}', N'wmspthzsh', N'{sourcedata}', N'', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2019-09-11T10:36:31.433' AS DateTime), CAST(N'2019-09-11T10:19:12.660' AS DateTime), 1, 0, N'0', N'metu', N'metu', N'ES')
GO
INSERT [dbo].[SysPage] ([ID], [IDOrderNO], [PageName], [ObjectType], [ObjectName], [PageNameEns], [sourcedata], [InterfaceAddress], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [cname], [appname], [pid]) VALUES (N'2BD544EF-EC33-49A4-9A4A-0335825EAE35', 7, N'wmsconfig', N'table', N'{objectame}', N'wmsconfig', N'{sourcedata}', N'wmsconfig', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2019-09-10T21:12:02.390' AS DateTime), CAST(N'2019-09-10T21:10:53.210' AS DateTime), 1, 0, N'0', N'metu', N'metu', N'ES')
GO
INSERT [dbo].[SysPage] ([ID], [IDOrderNO], [PageName], [ObjectType], [ObjectName], [PageNameEns], [sourcedata], [InterfaceAddress], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [cname], [appname], [pid]) VALUES (N'43D5833D-EE2E-4D08-B69C-A9ED1DF1D2DC', 5, N'仓库分区', N'table', N'{objectame}', N'wmspthzshselect', N'{sourcedata}', N'{interfaceaddress}', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2019-09-26T13:52:36.980' AS DateTime), CAST(N'2019-09-09T14:54:16.450' AS DateTime), 1, 0, N'0', N'metu', N'metu', N'ES')
GO
INSERT [dbo].[SysPage] ([ID], [IDOrderNO], [PageName], [ObjectType], [ObjectName], [PageNameEns], [sourcedata], [InterfaceAddress], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [cname], [appname], [pid]) VALUES (N'79D10C5B-DB9A-4C10-A516-EDD7397E0D0F', 11, N'tonypagename', N'table', N'{objectame}', N'tonypagename', N'{sourcedata}', N'tonypagename', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2019-09-12T14:25:33.863' AS DateTime), CAST(N'2019-09-12T14:25:33.863' AS DateTime), 1, 0, N'0', N'metu', N'metu', N'ES')
GO
SET IDENTITY_INSERT [dbo].[SysPage] OFF
GO
INSERT[dbo].[SYSPageDic] ([SetKey], [SortName], [SortCode], [Rank], [parentid], [ID], [pid], [cid], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [cname], [appname]) VALUES(N'DBCONFIG', N'数据库配置', N'DBCONFIG', N'1', NULL, N'0334e6cc-600c-4337-8f75-b4cf39579fa2', N'ES', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', NULL, NULL, 1, 0, N'0', N'metu', N'metu')
GO
INSERT[dbo].[SYSPageDICConfig] ([SetKey], [ConfigName], [ConfigValue], [ConfigExplain], [Rank], [ID], [PID], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [DICID], [version]) VALUES(N'WMS', N'WMS', N'Data Source=192.168.1.2;Initial Catalog=METUCORE;Persist Security Info=True;User ID=sa;Password=19810301s', N'WMS', N'1', N'00000000-0000-0000-0000-000000000001', N'ES', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2019-09-07T00:00:00.0000000' AS DateTime2), CAST(N'2019-09-07T00:00:00.0000000' AS DateTime2), 1, 0, N'0334e6cc-600c-4337-8f75-b4cf39579fa2', N'0')
GO
INSERT[dbo].[SysPageService] ([Id], [DOMAINID], [servicename], [ServiceEvents], [Remarks], [ReturnType], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [methodname], [pid], [appname], [cname]) VALUES(N'00000000-0000-0000-0000-000000000001', N'WMS', N'WMSservicePage', N'SELECT TOP (1000) [Id]
	 ,[DOMAINID]
	 ,[servicename]
	 ,[ServiceEvents]
	 ,[Remarks]
	 ,[ReturnType]
	 ,[CreatedTime]
	 ,[CreatedBy]
	 ,[ModifiedTime]
	 ,[ModifiedBy]
	 ,[CID]
	 ,[UpdateUserID]
	 ,[CreateUserID]
	 ,[UpdateDate]
	 ,[CreateDate]
	 ,[IsEnabled]
	 ,[IsDeleted]
	 ,[version]
	 ,[methodname]
	 ,[pid]
	 ,[appname]
	 ,[cname]

 FROM[METUCORE].[dbo].[SysPageService]', N'1', N'0', CAST(N'2019 - 09 - 07T00: 00:00.000' AS DateTime), N'tony', CAST(N'2019 - 09 - 07T00: 00:00.000' AS DateTime), N'tony', N'00000000 - 0000 - 0000 - 0000 - 000000000000', N'00000000 - 0000 - 0000 - 0000 - 000000000000', N'00000000 - 0000 - 0000 - 0000 - 000000000000', CAST(N'2019 - 09 - 07T00: 00:00.000' AS DateTime), CAST(N'2019 - 09 - 07T00: 00:00.000' AS DateTime), 1, 0, N'0', N'call', N'ES', N'metu', N'metu')
GO
INSERT[dbo].[SysPageService]([Id], [DOMAINID], [servicename], [ServiceEvents], [Remarks], [ReturnType], [CreatedTime], [CreatedBy], [ModifiedTime], [ModifiedBy], [CID], [UpdateUserID], [CreateUserID], [UpdateDate], [CreateDate], [IsEnabled], [IsDeleted], [version], [methodname], [pid], [appname], [cname]) VALUES(N'1AA012C3-B1D2-4808-ABB4-49975149CDA1', N'WMS', N'WMSservicePageget', N'SELECT TOP (1) [Id]
   ,[DOMAINID]
   ,[servicename]
   ,[ServiceEvents]
   ,[Remarks]
   ,[ReturnType]
   ,[CreatedTime]
   ,[CreatedBy]
   ,[ModifiedTime]
   ,[ModifiedBy]
   ,[CID]
   ,[UpdateUserID]
   ,[CreateUserID]
   ,[UpdateDate]
   ,[CreateDate]
   ,[IsEnabled]
   ,[IsDeleted]
   ,[version]
   ,[methodname]
   ,[pid]
   ,[appname]
   ,[cname]

FROM[dbo].[SysPageService] where id = N''{ ID}
''', N'1', N'0', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, N'0', N'call ', N'ES ', N'metu ', N'metu ')
GO ";
		public static string SQL_GET_PAGECONTROL_LIST = @"SELECT   SysPage.pagename, SysPage.objecttype, SysPage.objectname, SysPage.pagenameens, SysPageConfigs.sourcedata, 
                SysPageConfigs.interfaceaddress, SysPageConfigs.sourcedata AS fsourcedata, 
                SysPageConfigs.InterfaceAddress AS finterfaceaddress, SysPage.cid, SysPageConfigs.id, SysPageConfigs.syspageid, 
                SysPageConfigs.ControlName,SysPageConfigs.ControlName as english, SysPageConfigs.ControlCaption as chinese, SysPageConfigs.ControlType as datatype, SysPageConfigs.controlcaption, SysPageConfigs.controltype, 
                SysPageConfigs.placeholder, SysPageConfigs.require, SysPageConfigs.msg, SysPageConfigs.explain, 
                SysPageConfigs.enabled,
                SysPageConfigs.ControlOrder, SysPageConfigs.isgroup, SysPageConfigs.groupfield
FROM      SysPage INNER JOIN
                SysPageConfigs ON SysPage.ID = SysPageConfigs.SysPageID AND SysPage.CID = SysPageConfigs.CID where SysPage.PageNameens='{0}' ";

		public static string SQL_GET_PAGE_DATA_LIST = @"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
		public static string SQL_GET_PAGE_LIST_COMMON = @"SELECT   TOP (200)   
               lower( a.name) AS english, 
           (CASE WHEN  b.name ='datetime'  THEN 'Date'    WHEN  b.name ='bit'  THEN 'switch'  ELSE 'input' END) AS datatype,
                (CASE WHEN a.isnullable = 1 THEN 'True' ELSE 'False' END) AS require,
  (CASE WHEN lower(a.name) ='id' THEN '主键' WHEN lower(a.name) ='cid' THEN '所属企业主键' WHEN lower(a.name) ='updatedate' THEN '更新日期' WHEN lower(a.name) ='createdate' THEN '添加日期' WHEN lower(a.name) ='isenabled' THEN '是否启用' WHEN lower(a.name) ='isdeleted' THEN '是否删除' WHEN lower(a.name) ='updateuserid' THEN '更新人ID' WHEN lower(a.name) ='createuserid' THEN '创建人ID' ELSE cast( ISNULL(g.value, lower(a.name))  as nvarchar(100)) END) AS chinese 
,concat('请输入',cast(ISNULL(g.value, ' ') as nvarchar)) as msg,concat('请输入',cast(ISNULL(g.value, ' ') as nvarchar)) as placeholder,'' as explain
             
FROM      sys.syscolumns AS a LEFT OUTER JOIN
                sys.systypes AS b ON a.xtype = b.xusertype INNER JOIN
                sys.sysobjects AS d ON a.id = d.id AND d.xtype = 'U' AND d.name <> 'dtproperties' LEFT OUTER JOIN
                sys.syscomments AS e ON a.cdefault = e.id LEFT OUTER JOIN
                sys.extended_properties AS g ON a.id = g.major_id AND a.colid = g.minor_id LEFT OUTER JOIN
                sys.extended_properties AS f ON d.id = f.class AND f.minor_id = 0
WHERE   (b.name IS NOT NULL) and d.name='{0}' 
ORDER BY a.id ";
		public static string SQL_SysPageService_List = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";
		 
	}
}

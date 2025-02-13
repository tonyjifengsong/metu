using IdentityModel;
using METU.IDS4Server.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace METU.IDS4Server
{
    public static class UserIdentityExtension
    {
        /// <summary>
        /// 获得用户的Name
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string Name(this ClaimsPrincipal @this)
        {
            return @this?.Identity?.Name;
        }

        /// <summary>
        /// 获得DisplayName
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string DisplayName(this ClaimsPrincipal @this)
        {
            var value = @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.DisplayName.ToString())?.Value;
            return value;
        }

        public static string UserId(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.UserId.ToString())?.Value;
        }

        public static string MerchantId(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.MerchantId.ToString())?.Value;
        }

        public static string Role(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == JwtClaimTypes.Role.ToString())?.Value;
        }

        public static string APPID(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.APPID.ToString())?.Value;
        }

        public static string APPKEY(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.APPKEY.ToString())?.Value;
        }

        public static string APPNAME(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.APPNAME.ToString())?.Value;
        }

        public static string APPSECRET(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.APPSECRET.ToString())?.Value;
        }

        public static string CID(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.CID.ToString())?.Value;
        }

        public static string MEMO(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.MEMO.ToString())?.Value;
        }
        public static string APP(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.APP.ToString())?.Value;
        }

        public static string Sub(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.Sub.ToString())?.Value;
        }

        public static string Project(this ClaimsPrincipal @this)
        {
            return @this?.Claims?.FirstOrDefault(p => p.Type == EnumUserClaim.Project.ToString())?.Value;
        }
    }
}

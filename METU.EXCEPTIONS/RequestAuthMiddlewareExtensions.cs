using Microsoft.AspNetCore.Builder;

namespace METU.EXCEPTIOINS
{/// <summary>
/// 
/// </summary>
    public static class RequestCultureMiddlewareExtensions
    {/// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
        public static IApplicationBuilder UseRequestAuths(
            this IApplicationBuilder builder)
        {
            
            return builder.UseMiddleware<RequestAuthMiddleware>();
        }
    }
}

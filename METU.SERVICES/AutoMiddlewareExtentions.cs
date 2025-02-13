using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMiddlewareExtentions
    {
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="extStr"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutoMiddleware(
            this IApplicationBuilder builder, string extStr= "TonyMiddleWare")
        {
            FileHelper.Writelog("UseAutoMiddleware suffix:");
            FileHelper.Writelog(extStr);
            try { 
            var types = ASMHelper.GetMidwareAllClass();
                if (types == null)
                {
                    FileHelper.Writelog("UseAutoMiddleware types  is null！");
                    return builder;
                }
                if (types.Count < 1)
                {
                    FileHelper.Writelog("UseAutoMiddleware types  is Zero！");
                    return builder;
                }
                var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.Name.EndsWith(extStr, StringComparison.OrdinalIgnoreCase));
                if (interfaces == null)
                {
                    FileHelper.Writelog("UseAutoMiddleware interfaces  is null！");
                    return builder;
                }
                if (interfaces.Count() < 1)
                {
                    FileHelper.Writelog("UseAutoMiddleware interfaces  is Zero！");
                    return builder;
                }
                foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    FileHelper.Writelog("UseMiddleware:");
                    FileHelper.Writelog(implementType.FullName);

                    builder.UseMiddleware<AutoMiddleware>(Activator.CreateInstance(implementType, null));

                }
            }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("UseAutoMiddleware Exceptionis:" + ex.Message);

            }
            return builder;
        }
      
    }
}

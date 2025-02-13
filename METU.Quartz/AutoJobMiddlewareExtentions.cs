using METU.Quartz;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoJobMiddlewareExtentions
    {
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="extStr"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutoJobMiddleware(
            this IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            var types = ASMHelper.GetMidwareAllClass("JobContext");
            var interfaces = types.Where(t => t.FullName != null && t.IsInterface   && t.FullName.EndsWith("JobContext", StringComparison.OrdinalIgnoreCase));
            foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && !m.IsInterface && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    var quartz = app.ApplicationServices.GetRequiredService(implementType);
                    IJobContext jobc = (IJobContext)quartz;
                    lifetime.ApplicationStarted.Register(jobc.Start);
                    lifetime.ApplicationStopped.Register(jobc.Stop);                   
                }
            }


            return app;
        }
      
    }
}

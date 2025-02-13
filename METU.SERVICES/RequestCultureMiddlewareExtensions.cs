using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static partial class RequestCultureMiddlewareExtensions
    {/// <summary>
     /// 
     /// </summary>
     /// <param name="builder"></param>
     /// <returns></returns>
        public static IApplicationBuilder UseRequestAuth(
            this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<RequestAuthMiddleware>();
        }
    }
}

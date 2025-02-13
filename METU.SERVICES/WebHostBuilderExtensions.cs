using Microsoft.AspNetCore.Hosting;

namespace System
{
    public static partial class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseLogging(this IWebHostBuilder builder)
        {
            return builder;
        }
    }
}

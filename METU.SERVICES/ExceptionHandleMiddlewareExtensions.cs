using Microsoft.AspNetCore.Builder;

namespace System
{
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandleMiddleware>();

            return app;
        }
    }
}

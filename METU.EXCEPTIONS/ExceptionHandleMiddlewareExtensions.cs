

using Microsoft.AspNetCore.Builder;

namespace METU.EXCEPTIOINS
{
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandleMiddleware>();

            return app;
        }
    }
}

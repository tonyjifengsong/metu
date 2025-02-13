using System;
using System.Net;
using System.Threading.Tasks;
 
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace METU.EXCEPTIOINS
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
      

        public ExceptionHandleMiddleware(RequestDelegate next, IHostEnvironment env )
        {
            _next = next;
            _env = env;
          
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = _env.IsDevelopment() ? exception.ToString() : exception.Message;

          

            return context.Response.WriteAsync(JsonConvert.SerializeObject( error , new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}

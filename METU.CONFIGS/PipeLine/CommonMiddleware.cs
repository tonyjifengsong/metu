using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace METU.CONFIGS.PipeLine
{
    public class CommonMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly object[] _options;
        private readonly METUContext metuParam;  
        public CommonMiddleware(RequestDelegate next,params object[] options)
        {
            System.Diagnostics.Debug.WriteLine("CommonMiddleware");
            metuParam = new METUContext();
               _next = next;
            _options = options;
            int i = 0;
            foreach (object obj in options)
            {
                System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(obj));

                metuParam.DicContext.Add("param" + i,obj);
                i++;
            }

        }

        
        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("CommonMiddleware-Invoke");

                metuParam.Name = "tony";
                WorkFlows.DoWork(metuParam);
                return   _next(httpContext); 
            }
            catch (Exception ex)
            {
              return    HandleExceptionAsync(httpContext, ex);
            }
        } 

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = metuParam.Evn=="Development" ? exception.ToString() : exception.Message;



            return context.Response.WriteAsync(JsonConvert.SerializeObject(error, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }


    }
}

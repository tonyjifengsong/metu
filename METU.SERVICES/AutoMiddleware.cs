using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMiddleware
    {
       

        private IBaseMiddleWare _IBus;
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public AutoMiddleware(RequestDelegate next, IBaseMiddleWare IBus )
        {

            _IBus = IBus;
           
            _next = next;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {         
            if (_IBus == null)
            {
                await _next(context);
            }
            else
            {
                if (!_IBus.DoWork(context, null)) return;
                if (_IBus.DoWorking(context, null))
                {
                    await _next(context);
                }
                else
                {
                    return;
                }
                if (!_IBus.DoWorked(context, null)) return;
            }
        }
    }
}

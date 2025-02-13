using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace METU.Extentions.Attributes
{   /// <summary>
    /// 
    /// </summary>
    public class ApiLogAttribute : ActionFilterAttribute

    {/// <summary>
     /// 
     /// </summary>
     /// <param name="context"></param>

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>

        public override void OnActionExecuting(ActionExecutingContext context)

        {
            FileHelper.Writelog("[地址]:" + context.HttpContext.Request.Path);

            FileHelper.Writelog("[Headers]:" + context.HttpContext.Request.Headers.toJson());
            FileHelper.Writelog("[参数]:" + context.ActionArguments.toJson());

            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>

        public override void OnResultExecuting(ResultExecutingContext context)

        {
            FileHelper.Writelog(context.Result.toJson());
            FileHelper.Writelog("[结果]:" + context.Result.toJson());


            base.OnResultExecuting(context);
        }


    }
}

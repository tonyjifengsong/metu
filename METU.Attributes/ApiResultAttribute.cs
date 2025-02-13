using METU.MODEL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace METU.Extentions.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResultAttribute : ActionFilterAttribute

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
            FileHelper.Writelog("==============================================Start=========================================================");

            FileHelper.Writelog("[OnActionExecuting地址]:");
            FileHelper.Writelog(context.HttpContext.Request.Path);
            FileHelper.Writelog("[OnActionExecuting-QueryString]:");
            FileHelper.Writelog(context.HttpContext.Request.QueryString);
            FileHelper.Writelog("[OnActionExecuting-Query]:");
            FileHelper.Writelog(context.HttpContext.Request.Query.toJson());

            FileHelper.Writelog("[OnActionExecutingHeaders]:");
            FileHelper.Writelog(context.HttpContext.Request.Headers.toJson());
            FileHelper.Writelog("[OnActionExecuting参数]:");
            FileHelper.Writelog(context.ActionArguments.toJson());

            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>

        public override void OnResultExecuting(ResultExecutingContext context)

        {
            Result result = new Result(context.Result);

            //根据实际需求进行具体实现
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                result = new Result(objectResult.Value);
                context.Result = new ObjectResult(result);
            }
            else if (context.Result is EmptyResult)
            {
                result = new Result("");
                context.Result = new ObjectResult(result);
            }
            else if (context.Result is ContentResult)
            {
                var objectResult = context.Result as ContentResult;
                result = new Result((int)objectResult.StatusCode, objectResult.Content);
                context.Result = new ObjectResult(result);
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(result);
            }

            base.OnResultExecuting(context);
            FileHelper.Writelog("[OnResultExecuting返回结果]:");
            FileHelper.Writelog(context.Result.toJson());

            FileHelper.Writelog("=====================================================END==================================================");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);


        }
    }
}

using METU.CACHES;
using METU.CONFIGS;
using METU.Core;
using METU.MODEL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;

using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace METU.EXCEPTIOINS
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestAuthMiddleware
    { 
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RequestAuthMiddleware(RequestDelegate next)
        {
             
            _next = next;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
           var Headers = JsonConvert.SerializeObject(context.Request.Headers);
            var token = context.Request.Headers["token"];

            if (StringValues.IsNullOrEmpty(token))
                token = context.Request.Query.FirstOrDefault(k => k.Key.ToLower() == "token").Value;
           

            //请求Url
            var questUrl = context.Request.Path.Value.ToLower().Split('?')[0];
            #region  权限白名单

            ConfHelper conf = new ConfHelper();
            CommonCache.SSOURL = conf.ReadConfig("SSOURL");
            if (CommonCache.SSOURL == null)
            {
                FileHelper.Writelog("没有配置登录验证");

                await CreateUnauthorizedResponse(context);

                return;
            }
            if (CommonCache.SSOURL.ToString().Trim().Length < 11)
            {
                FileHelper.Writelog("配置登录验证错误");

                await CreateUnauthorizedResponse(context);

                return;
            }
            bool isservice = true;
            string regstr = "";
            try
            {
                string[] urlarr = questUrl.Split('/');
                if(urlarr.Length==0) urlarr = questUrl.Split('\\');
                if (urlarr.Length ==ConstNum.Zero)
                {
                    await this._next(context);
                    return;
                }

                regstr = conf.ReadConfig("regwhite").ToLower();
                if (regstr != null) if (regstr.ToString().Trim().Length > 3)
                    {
                        string[] regarr = regstr.Split(';');

                        for(int i = 0; i < regarr.Length; i++)
                        {
                            for(int j = 0; j < urlarr.Length; j++)
                            {
                                if (urlarr[j].ToString().IndexOf(regarr[i].ToString()) >ConstNum.Zero)
                                {
                                    isservice = false;
                                    break;
                                }
                            }
                        }
                    }
               
            }
            catch
            {
                
            }
            if (!isservice)
            {
                await this._next(context);

            }
            else
            {
                #endregion
                if (!string.IsNullOrEmpty(token))
                {

                    string strbody = "{\"token\":\"{0}\"}";
                    if (string.IsNullOrEmpty(token))
                    {
                        strbody = strbody.Replace("{0}", "");
                    }
                    else
                    {
                        strbody = strbody.Replace("{0}", token.ToString());
                    }
                    Result rs = new Result();
                    try
                    {
                        FileHelper.Writelog(strbody);
                        rs = WebAPIHelper.HttpPost(CommonCache.SSOURL + "Check", strbody).toEntity<Result>();
                        FileHelper.Writelog(rs);
                        FileHelper.Writelog(CommonCache.SSOURL + "Check");
                        if (rs.IsSuccess)
                        {
                            var urs = WebAPIHelper.HttpPost(CommonCache.SSOURL + "GetUserinfo", strbody).toEntity<Result>();
                            FileHelper.Writelog(CommonCache.SSOURL + "GetUserinfo");
                            
                            if (urs.IsSuccess)
                            {
                                
                                 FileHelper.Writelog(urs);
                               
                                await this._next(context);
                            }
                            else
                            {
                                FileHelper.Writelog(urs.IsSuccess);

                                await CreateUnauthorizedResponse(context);

                                return;
                            }

                        }
                        else
                        {
                            if (questUrl.ToLower().EndsWith("login") || questUrl.ToLower().EndsWith("register") || questUrl.ToLower().EndsWith("loginout"))
                            {
                                await this._next(context);

                            }
                            else
                            {
                                if (rs.Code == 0)
                                {
                                    await CreateTokenResponse(context);
                                    return;
                                }
                                //无权限跳转到拒绝页面
                                await CreateUnauthorizedResponse(context);

                                return;
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        FileHelper.Writelog(ex);

                        await CreateUnauthorizedResponse(context);

                        return;
                    }

                }
                else
                {
                    //是否经过验证
                    var isAuthenticated = context.User.Identity.IsAuthenticated;
                    if (isAuthenticated)
                    {
                        
                                await this._next(context);

                            
                    }
                    else
                    {
                        if (questUrl.ToLower().EndsWith("login") || questUrl.ToLower().EndsWith("register") || questUrl.ToLower().EndsWith("loginout"))
                        {
                            await this._next(context);

                        }
                        else
                        {

                            //无权限跳转到拒绝页面
                            await CreateUnauthorizedResponse(context);

                            return;
                        }
                    }


                }
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task CreateUnauthorizedResponse(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json;charset=utf-8";

            Result result = new Result
            { Code = context.Response.StatusCode,
                Msg = context.Response.StatusCode.ToString()
            };
           // WebAPIHelper.WebclientPost(result.toJson());
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task CreateTokenResponse(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json;charset=utf-8";

            Result result = new Result
            {              
                Msg = "Token过期！",
                Code = 0 
            };
           // WebAPIHelper.PostLog(result.toJson());
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }
    }
}

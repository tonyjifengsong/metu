using METU.INTERFACE.ICore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;

namespace System
{
    public static  partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用WebHost
        /// </summary>
        /// <param name="app"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebHost(this IApplicationBuilder app, string fileName= "index.html")
        {

            #region 用户权限登录验证 added by tony 2019-11-29
            // app.UseAutoJobMiddleware(lifetime);
            app.UseAutoMiddleware("initialMiddleWare");//添加初始化中间件
                                                       //限制用户权限登录验证
                                                       // app.UseRequestAuth();
            #endregion
            //异常处理
            app.UseExceptionHandle();

            //设置默认文档
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add(fileName);
            app.UseDefaultFiles(defaultFilesOptions);
            
            app.UseDefaultPage();
           
            app.UseDocs();

            
          
            app.UseAutoMiddleware("ParamMiddleWare");//添加参数处理中间件
           
            app.UseAutoMiddleware();//添加通用中间件
                                    //启用中间件服务生成Swagger作为JSON终结点
           
            app.UseAutoMiddleware("ResultMiddleWare");//添加结果处理中间件
                                                      //
            return app;
        }

        /// <summary>
        /// 启用默认页
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDefaultPage(this IApplicationBuilder app, string pathStr = "metu")
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + pathStr);
            if (Directory.Exists(path))
            {
                var options = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = new PathString("/" + pathStr)
                };

                app.UseStaticFiles(options);
            }

            return app;
        }

        /// <summary>
        /// 启动文档页
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDocs(this IApplicationBuilder app, string pathStr = "docs")
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + pathStr);
            if (Directory.Exists(path))
            {
                var options = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = new PathString("/" + pathStr)
                };

                app.UseStaticFiles(options);
            }

            return app;
        }

        /// <summary>
        /// 启用应用停止处理
        /// </summary>
        /// <returns></returns>
        public static IApplicationBuilder UseShutdownHandler(this IApplicationBuilder app)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                var handlers = app.ApplicationServices.GetServices<IAppShutdownHandler>().ToList();
                foreach (var handler in handlers)
                {
                    handler.Handle();
                }
            });

            return app;
        }
        public static IApplicationBuilder ConfigureMidware(this IApplicationBuilder app, string midWareExtStr = "initialMiddleWare")
        {
            FileHelper.Writelog("UseAutoMiddleware midWareExtStr:");
            FileHelper.Writelog(midWareExtStr);
            FileHelper.Writelog("UseAutoMiddleware:");

            // app.UseAutoJobMiddleware(lifetime);
            app.UseAutoMiddleware(midWareExtStr);
            FileHelper.Writelog("UseWebHost:");
            app.UseWebHost();
            FileHelper.Writelog("UseShutdownHandler:");
            app.UseShutdownHandler();
            return app;
        }
    }
}

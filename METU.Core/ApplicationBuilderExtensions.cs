using System;
using System.IO;
using System.Linq;
using METU.EXCEPTIOINS;
using METU.INTERFACE.ICore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using HostOptions = METU.MODEL.HostOptions;

namespace System
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用WebHost
        /// </summary>
        /// <param name="app"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebSelfHost(this IApplicationBuilder app, HostOptions hostOptions=null, IHostEnvironment env=null)
        {

            #region 用户权限登录验证 added by tony 2019-11-29
         //app.UseAutoJobMiddleware(lifetime);
             app.UseAutoMiddleware("initialMiddleWare");//添加初始化中间件
            //限制用户权限登录验证
          app.UseRequestAuth();
            #endregion
            //异常处理
            app.UseExceptionHandle();

            //设置默认文档
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseCors();
            app.UseDefaultPage();
           
            app.UseDocs();

            //反向代理
            if (hostOptions.Proxy)
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

             app.UseAutoMiddleware("ParamMiddleWare");//添加参数处理中间件
            //配置端点
          
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
        public static IApplicationBuilder UseDefaultPage(this IApplicationBuilder app)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/metu");
            if (Directory.Exists(path))
            {
                var options = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = new PathString("/metu")
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
        public static IApplicationBuilder UseDocs(this IApplicationBuilder app)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/docs");
            if (Directory.Exists(path))
            {
                var options = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = new PathString("/docs")
                };

                app.UseStaticFiles(options);
            }

            return app;
        }

        /// <summary>
        /// 启用应用停止处理
        /// </summary>
        /// <returns></returns>
        public static IApplicationBuilder UseShutdownHandlers(this IApplicationBuilder app)
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

            //app.UseAutoJobMiddleware(lifetime);
            app.UseAutoMiddleware(midWareExtStr);
            app.UseWebHost();
            app.UseShutdownHandler();
            return app;
        }
    }
}

using System;
using Microsoft.AspNetCore.Builder;

namespace System
{
    public static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用WebHost
        /// </summary>
        /// <param name="app"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebSelfHost(this IApplicationBuilder app, string fileName= "index.html")
        {

            #region 用户权限登录验证 added by tony 2019-11-29

            //设置默认文档
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add(fileName);
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseCors();
            app.UseDefaultPage();
            app.UseDocs();
            #endregion

            return app;
        }
        
    }
}
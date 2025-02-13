using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using HostOptions = METU.MODEL.HostOptions;

namespace METU.Main
{
    public static class ServiceCollectionExtensions
    {
        #region added by tony 2019-12-31
        /// <summary>
        /// 添加CORS
        /// </summary>
        /// <param name="services"></param>
        public static void AddCORSALL(this IServiceCollection services)
        {

            services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials()));


        }
        /// <summary>
        /// 添加核心业务注入
        /// </summary>
        public static void AddBLLCore(this IServiceCollection services)
        {

         services.AddScoped(typeof(BaseBLL<>));
        }
       
        #endregion
        /// <summary>
        /// 添加WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env">环境</param>
        /// <returns></returns>
        public static IServiceCollection AddWebHost(this IServiceCollection services, HostOptions hostOptions, IHostEnvironment env)
        {
            // services.AddAutoJobConfig();
            services.AddAutoConfig();//自动添加注入服务
            services.AddSingleton(hostOptions);
            services.AddBLLCore();           
            services.AddUtilsMvc();
          
            //添加MVC功能
            services.AddMvc()
             .SetCompatibilityVersion(CompatibilityVersion.Version_2_2); //
           // services.AddCORSALL();
            // services.AddDynSwagger();//动态API
          //  services.AddAutoGroupSwagger();
            //解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            //添加HttpClient相关
            services.AddHttpClient();
            services.AddAutoService();//服务注入自动添加
            //services.AddSwaggerGen(c =>
            //{



            //    foreach (var asm in ASMHelper.GetAllAssembly())
            //    {
            //        if (asm.FullName.ToLower().ToString().IndexOf("etu") > 0)
            //        {
            //            c.SwaggerDoc(asm.FullName.Split(',')[0], new OpenApiInfo
            //            {
            //                Title = asm.FullName.Split(',')[0],
            //                Version = asm.FullName.Split('.')[0]
            //            });
            //            Type[] types = asm.GetTypes();
            //            foreach (var item in types)
            //            {
            //                //判断是否是接口

            //                var asmName = item.Assembly.GetName().Name;
            //                string filePath = "";
            //                filePath = Path.Combine(System.AppContext.BaseDirectory, asmName.Remove(asmName.LastIndexOf('.')) + ".xml");
            //                try
            //                {
            //                    FileHelpers.Writelog(filePath);
            //                    c.IncludeXmlComments(filePath);
            //                }
            //                catch { }

            //            }
            //        }
            //    }

            //});
            return services;
        }
    }
}

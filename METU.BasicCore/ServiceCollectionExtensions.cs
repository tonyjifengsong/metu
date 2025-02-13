using METU.CACHES;
using METU.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using HostOptions = METU.MODEL.HostOptions;

namespace Microsoft.Extensions.DependencyInjection
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
        /// 添加WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env">环境</param>
        /// <returns></returns>
        public static IServiceCollection AddBasicHost(this IServiceCollection services)
        {
            FileHelper.Writelog("AddBasicHost:InitialStart");

            services.InitialStart();
            FileHelper.Writelog("AddBasicHost:InitialStart");
            if (FileHelper.checkFileExists("config\\appsettings.json"))
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-config\\appsettings.json");

                services.AddAutoReadDBConfigServices("config\\appsettings.json");//获取数据配置信息
            }
            else
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-Default json file");

                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices");
                services.AddAutoReadDBConfigServices();//获取数据配置信息
            }
            FileHelper.Writelog("AddBasicHost:缓存注入");
            services.AddDBConfig("Cache");
            FileHelper.Writelog("AddBasicHost:数据库注入");
            services.AddDBConfig();
            FileHelper.Writelog("AddBasicHost:队列注入");
            services.AddDBConfig("queue");
            FileHelper.Writelog("AddBasicHost:服务层接口注入");
            services.AddServices();
            FileHelper.Writelog("AddBasicHost:服务注入自动添加");
            services.AddAutoService();
            FileHelper.Writelog("AddBasicHost:AddSingle");
            services.AddSingle();
            FileHelper.Writelog("AddBasicHost:AddBLLCore");
            services.AddBLLCore();
            FileHelper.Writelog("AddBasicHost:AddCoreBLL");
            services.AddCoreBLL();
            FileHelper.Writelog("AddBasicHost:AddBaseCore");
            //services.AddBaseCore();
            FileHelper.Writelog("AddBasicHost:AddService");
            services.AddService();
            return services;
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
            services.InitialStart();
            // services.AddAutoJobConfig();
            services.AddAutoConfig();//自动添加注入服务
            services.AddSingle();
            services.AddBLLCore();
            services.AddCoreBLL();
            //services.AddBaseCore();
            //services.AddUtilsMvc();
            services.AddService();
            ////添加MVC功能
            //services.AddMvc()
            // .SetCompatibilityVersion(CompatibilityVersion.Version_2_2); //
            services.AddCORSALL();
            //解决Multipart body length limit 134217728 exceeded
            //services.Configure<FormOptions>(x =>
            //{
            //    x.ValueLengthLimit = int.MaxValue;
            //    x.MultipartBodyLengthLimit = int.MaxValue;
            //});

           services.AddAutoService();//服务注入自动添加

            return services;
        }
        /// <summary>
        /// 添加WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env">环境</param>
        /// <returns></returns>
        public static IServiceCollection AddHost(this IServiceCollection services, HostOptions hostOptions = null, IHostEnvironment env = null)
        {
            FileHelper.Writelog("AddHost:InitialStart");
            services.InitialStart();
            FileHelper.Writelog("AddHost:TryAddSingleton");
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-config\\appsettings.json");
            if (FileHelper.checkFileExists("config\\appsettings.json"))
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-config\\appsettings.json");

                services.AddAutoReadDBConfigServices("config\\appsettings.json");//获取数据配置信息
            }
            else
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-Default json file");

                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices");
              services.AddAutoReadDBConfigServices();//获取数据配置信息
            }
            FileHelper.Writelog("AddHost:AddSingleton");
            services.AddSingle();
            FileHelper.Writelog("AddHost:AddBLLCore");
            services.AddBLLCore();
            FileHelper.Writelog("AddHost:AddCoreBLL");
            services.AddCoreBLL();
            FileHelper.Writelog("AddHost:AddBaseCore");
            //services.AddBaseCore();
            FileHelper.Writelog("AddHost:缓存注入");
            services.AddDBConfig("Cache");
            FileHelper.Writelog("AddHost:数据库注入");
            services.AddDBConfig();
            FileHelper.Writelog("AddHost:队列注入");
            services.AddDBConfig("queue");
            FileHelper.Writelog("AddHost:服务层接口注入");
            services.AddServices();
            FileHelper.Writelog("AddHost:业务服务层接口注入");
            services.AddService();
            FileHelper.Writelog("AddHost:服务注入自动添加");
            services.AddAutoService();
            return services;
        }


        /// <summary>
        /// 添加WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env">环境</param>
        /// <returns></returns>
        public static IServiceCollection InitialStart(this IServiceCollection services, HostOptions hostOptions = null, IHostEnvironment env = null)
        {
            if (!CommonCache.EnabledLog)
            {
                bool EnabledLog = false;
                FileHelper.Writelog("read config:EnabledLog=");
                string logs = ConfigHelper.GetConfigSettings("EnabledLog");// "http://localhost:2222";//"http://47.114.146.15:2222";
                FileHelper.Writelog(logs);
                if (logs==null||!logs.ToUpper().Equals("TRUE")|| !logs.ToUpper().Equals("1"))
                {
                    CommonCache.EnabledLog = EnabledLog;
                }
                else
                {
                    CommonCache.EnabledLog = true;
                }
            }
            FileHelper.Writelog("read config:configurl＝");
            string url = ConfigHelper.GetConfigSettings("configurl");// "http://localhost:2222";//"http://47.114.146.15:2222";
            FileHelper.Writelog(url);
            CommonCache.ConfigURL = url;
            FileHelper.Writelog("read config:ElasticSearchContext:esurl");
            CommonCache.LogURL = ConfigHelper.GetConfigSettings("ElasticSearchContext:esurl");
            List<string> lst = new List<string>();
            lst.Add(CommonCache.LogURL);
            CommonCache.ConfigCaches.Add("ESCONFIG", lst);
            CommonCache.APPBASEPATH = System.IO.Directory.GetCurrentDirectory();
            return services;
        }

        public static IServiceCollection AutoConfig(this IServiceCollection services)
        {
            FileHelper.Writelog("AutoConfig:InitialStart");

            services.InitialStart();
            FileHelper.Writelog("AutoConfig:InitialStart");
            if (FileHelper.checkFileExists("config\\appsettings.json"))
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-config\\appsettings.json");

                services.AddAutoReadDBConfigServices("config\\appsettings.json");//获取数据配置信息
            }
            else
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-Default json file");

                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices");
                services.AddAutoReadDBConfigServices();//获取数据配置信息
            }
            FileHelper.Writelog("AutoConfig:缓存注入");
            services.AddDBConfig("Cache");
            FileHelper.Writelog("AutoConfig:数据库注入");
            services.AddDBConfig();
            FileHelper.Writelog("AutoConfig:队列注入");
            services.AddDBConfig("queue");
            FileHelper.Writelog("AutoConfig:服务层接口注入");
            services.AddServices();
            FileHelper.Writelog("AutoConfig:服务注入自动添加");
            services.AddAutoService();
            FileHelper.Writelog("AutoConfig:AddSingle");
            services.AddSingle();
            FileHelper.Writelog("AutoConfig:AddBLLCore");
            services.AddBLLCore();
            FileHelper.Writelog("AutoConfig:AddCoreBLL");
            services.AddCoreBLL();
            FileHelper.Writelog("AutoConfig:AddBaseCore");
            //services.AddBaseCore();
            FileHelper.Writelog("AutoConfig:AddService");
            services.AddService();
            return services;
        }
    }
}

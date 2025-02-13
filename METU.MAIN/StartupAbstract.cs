using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using HostOptions = METU.MODEL.HostOptions;
using Microsoft.Extensions.Hosting;
using METU.Core;
using METU.CACHES;
using METU.MAIN;

namespace METU.Main
{
    public abstract class StartupAbstract
    {
        protected readonly HostOptions HostOptions;
        protected readonly IHostEnvironment Env;

        protected StartupAbstract(IHostEnvironment env)
        {
            Env = env;
            CommonCache.APPBASEPATH = System.IO.Directory.GetCurrentDirectory();
            //加载主机配置项
            HostOptions = ConfigurationHelper.Get<HostOptions>("Host", env.EnvironmentName) ?? new HostOptions();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();// services.AddCORSALL();
            //  services.AddAutoConfig();//自动配置
            services.AddWebSelfHost(HostOptions, Env);
         }

     
        public virtual void Configure(IApplicationBuilder app)
        {
           
        
            // app.UseAutoJobMiddleware(lifetime);
            app.UseAutoMiddleware("initialMiddleWare");
            app.UseWebSelfHost(HostOptions, Env);
             
            app.UseShutdownHandler();
        }
    }
}

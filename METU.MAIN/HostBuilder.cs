
using METU.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using HostOptions = METU.MODEL.HostOptions;

namespace METU.Main
{
    /// <summary>
    /// WebHost构造器
    /// </summary>
    public class HostBuilder
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args">启动参数</param>
        public void Run<TStartup>(string[] args) where TStartup : StartupAbstract
        {
            CreateBuilder<TStartup>(args).Build().Run();
        }

        /// <summary>
        /// 创建主机生成器
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public IHostBuilder CreateBuilder<TStartup>(string[] args) where TStartup : StartupAbstract
        {
           

            //加载主机配置项
            var hostOptions = ConfigurationHelper.Get<HostOptions>("Host") ?? new HostOptions();

            if (string.IsNullOrEmpty(hostOptions.Urls))
                hostOptions.Urls = "http://*:5000";

            return  Host.CreateDefaultBuilder(args)
                    .UseDefaultServiceProvider(options => { options.ValidateOnBuild = false; })
                   ;
        }
    }
}

using Consul;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace METU.WEBAPI
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加核心业务注入
        /// </summary>
        public static void AddBLLCore(this IServiceCollection services)
        {

            services.AddScoped(typeof(ConsulHelper));
        }
        /// <summary>
        /// 注册服务
        /// </summary>
        public static void AddConsul(this IServiceCollection services,string ConsoulHost= "http://localhost", int ConsulPort= 8500)
        {
            ConsulPort = ConsulPort < 0 ? 8500 : ConsulPort;
            ConsoulHost = ConsoulHost == null ? "http://localhost" : ConsoulHost;
            string consul = ConsoulHost + ":" + ConsulPort;
            ConsulClient clinet = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter ="DCC";
            });

            string ip = ConsoulHost;
            int port = ConsulPort;
            clinet.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service" + Guid.NewGuid(),
                Name = "DCC-ServiceName",
                Address = ip,
                Port = port,
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12),//间隔12秒一次
                    HTTP = $"http://{ip}:{port}/api/health/heartBeat",
                    Timeout = TimeSpan.FromSeconds(52),//检测等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60)//失败多久后移除，最小值60秒
                }
            }).Wait();
        }
    }
}

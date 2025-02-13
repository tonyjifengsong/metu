using Consul;
using METU.CONFIGS;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;

namespace METU.WEBAPI
{
    public static class ConsulBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ConsulOption consulOption=null)
        {
            if (consulOption == null)
            {
                consulOption = new ConsulOption();
                consulOption.ConsulAddress = AppconfigData.Read("Consul:ConsulAddress") ==null? "http://localhost:8500/": AppconfigData.Read("Consul:ConsulAddress") ;
                consulOption.ConsulServiceName = AppconfigData.Read("Consul:ConsulServiceName") == null ? "ServiceName" : AppconfigData.Read("Consul:ConsulServiceName");
                consulOption.ConsulIP = AppconfigData.Read("Consul:ConsulIP") == null ? "http://localhost" : AppconfigData.Read("Consul:ConsulIP");
                consulOption.ConsulPort = AppconfigData.Read("Consul:ConsulPort") == null ? 8500 :int.Parse(AppconfigData.Read("Consul:ConsulPort"));
                consulOption.ServiceHealthCheck = AppconfigData.Read("Consul:ServiceHealthCheck") == null ? $"http://localhost/api/health/heartBeat" : AppconfigData.Read("Consul:ServiceHealthCheck")+ "/api/health/heartBeat";
            }
             var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri(consulOption.ConsulAddress);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = consulOption.ID,
                Name = consulOption.ConsulServiceName,// 服务名
                Address = consulOption.ConsulIP, // 服务绑定IP
                Port = consulOption.ConsulPort, // 服务绑定端口
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulOption.DeregisterCriticalServiceAfter),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(consulOption.Interval),//健康检查时间间隔
                    HTTP = consulOption.ServiceHealthCheck,//健康检查地址
                    Timeout = TimeSpan.FromSeconds(consulOption.Timeout)
                }
            };
            try
            {
                // 服务注册
                consulClient.Agent.ServiceRegister(registration).Wait();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            // 应用程序终止时，服务取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
}

using Consul;
using Microsoft.Extensions.Configuration;
using System;

namespace METU.WEBAPI
{
    public static class ConsulHelper
    {
        public static void Init(this IConfiguration _configuration)
        {
            ConsulClient clinet = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dcl";
            });

            string ip = _configuration["ip"];
            int port = int.Parse(_configuration["port"]);
            clinet.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service" + Guid.NewGuid(),
                Name = "ServiceName",
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

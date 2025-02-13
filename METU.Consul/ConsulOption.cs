using Consul;
using System.Collections.Generic;

namespace METU.WEBAPI
{
    /// <summary>
    /// Consul 注册发现相关参数
    /// </summary>
    public class ConsulOption
    {
        public ConsulOption()
        {
            Datacenter = "DC1";
               ID = "MicroService";
            Timeout = 5;
            Interval = 10;
            DeregisterCriticalServiceAfter = 5;
            ConsulAddress = "http://localhost:8500/";
            ConsulServiceName = "ServiceName";
            ConsulIP = "http://localhost";
            ConsulPort = 8500;
            ServiceHealthCheck = $"http://{ConsulIP}:{ConsulPort}";
        }

        public string ID
        {
            get;
            set;
        }

        public string Datacenter
        {
            get;
            set;
        }

        /// <summary>
        /// 健康检查时间间隔
        /// </summary>
        public double Interval
        {
            get;
            set;
        }

        
        public double Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// 服务启动多久后注册
        /// </summary>
        public double DeregisterCriticalServiceAfter
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ConsulServiceName { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string ConsulIP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ConsulPort { get; set; }

        /// <summary>
        /// 服务健康检查地址
        /// </summary>
        public string ServiceHealthCheck { get; set; }

        /// <summary>
        /// Consul 地址
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}

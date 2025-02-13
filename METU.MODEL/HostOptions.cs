using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    /// <summary>
    /// 主机配置项
    /// </summary>
    public class HostOptions
    {
        /// <summary>
        /// 绑定的地址(默认：http://*:5000)
        /// </summary>
        public string Urls { get; set; }

        /// <summary>
        /// 开启Swagger
        /// </summary>
        public bool Swagger { get; set; }

        /// <summary>
        /// 启用代理
        /// </summary>
        public bool Proxy { get; set; }
    }
}

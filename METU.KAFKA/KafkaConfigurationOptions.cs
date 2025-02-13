using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
    public class KafkaConfigurationOptions : IConfigureOptions<KafkaOptions>
    {
        private readonly IConfiguration _configuration;
        public KafkaConfigurationOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(KafkaOptions options)
        {
           // options = _configuration.Get<KafkaOptions>();
            //这里仅仅自定义一些你自己的代码，使用上面configuration配置中的配置节，处理程序没法自动绑定的
            //  一些事情。
        }
    }
}
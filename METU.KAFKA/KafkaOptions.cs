using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
   public class KafkaOptions: ClientConfig
    { /// <summary>
      /// 服务器地址
      /// </summary>
        public string[] BootstrapServer { get; set; }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
    public class KafkaBuilder : IKafkaBuilder
    {
        public IServiceCollection Services { get; }
        public KafkaBuilder(IServiceCollection services)
        {
           Services = services;
        }
    } 
}

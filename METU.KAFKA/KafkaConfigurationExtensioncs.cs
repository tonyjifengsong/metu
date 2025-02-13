using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
    public static class KafkaConfigurationExtensioncs
    {
        public static IKafkaBuilder AddConfiguration(this IKafkaBuilder builder , IConfiguration configuration)
        {

            InitService(builder, configuration);
            return builder;
        }
        public static void InitService(IKafkaBuilder builder, IConfiguration configuration)
        {
            builder.Services.TryAddSingleton<IConfigureOptions<KafkaOptions>>(new KafkaConfigurationOptions(configuration));  //配置类和配置内容
            builder.Services.TryAddSingleton
            (ServiceDescriptor.Singleton<IOptionsChangeTokenSource<KafkaOptions>>(new ConfigurationChangeTokenSource<KafkaOptions>(configuration)));//这个是观察类，如果更改，会激发onchange方法

            builder.Services
            .TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<KafkaOptions>>
            (new ConfigureFromConfigurationOptions<KafkaOptions>(configuration))); //这个是option类，没这个，配置无法将类绑定

            builder.Services.AddSingleton(new KafkaConfigurationOptions(configuration));
        }
    } 
}

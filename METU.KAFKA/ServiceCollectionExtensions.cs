using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
    public static class ServiceCollectionExtensions
    {        /// <summary>
             /// Adds logging services to the specified <see cref="IServiceCollection" />.        /// </summary>
             /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
             /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddKafka(this IServiceCollection services)
        {
            return AddKafka(services, builder => { });
        }
        public static IServiceCollection AddKafka(this IServiceCollection services , Action<IKafkaBuilder> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();
            configure(new KafkaBuilder(services));
            services.TryAddSingleton<IKafkaService, KafkaService>();  //kafka的服务类
           return services;
        }
    } 
}

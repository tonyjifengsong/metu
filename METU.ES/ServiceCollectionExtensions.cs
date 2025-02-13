using METU.CACHES;
using METU.ES.Elastic;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace System
{
    public static partial class ServiceCollectionExtensions
    {
        #region added by tony 2021-08-24
     
        /// <summary>
        /// 添加ElasticSearch
        /// </summary>
        /// <param name="services"></param>
        public static void AddElastic(this IServiceCollection services)
        {

            services.Configure<EsConfig>(options =>
            {
                options.Urls =(List<string>) CommonCache.ConfigCaches["ESCONFIG"];

            });
            services.AddSingleton<IEsClientProvider, EsClientProvider>();
            services.AddTransient<IBaseEsContext, ESBaseContext>();

        }
       

        #endregion

    }
}

using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.Elastic
{
    /// <summary>
    /// ElasticClient 提供者接口
    /// </summary>
    public interface IEsClientProvider
    {
        /// <summary>
        /// 获取ElasticClient
        /// </summary>
        /// <returns></returns>
        ElasticClient GetClient();
        /// <summary>
        /// 指定index获取ElasticClient
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        ElasticClient GetClient(string indexName);

        /// <summary>
        /// Linq查询的官方Client
        /// </summary>
        IElasticClient _elasticLinqClient { get; set; }
        /// <summary>
        /// Js查询的官方Client
        /// </summary>
        IElasticLowLevelClient _elasticJsonClient { get; set; }
        IElasticClient LinqClient();

        IElasticLowLevelClient JsonClient();

    }


}

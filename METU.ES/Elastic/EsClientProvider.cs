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
    /// ElasticClient提供者
    /// </summary>
    public class EsClientProvider : IEsClientProvider
    {
        private readonly IOptions<EsConfig> _EsConfig;
        public EsClientProvider(IOptions<EsConfig> esConfig)
        {
            _EsConfig = esConfig;
        }

        public IElasticClient _elasticLinqClient { get; set ; }
        public IElasticLowLevelClient _elasticJsonClient { get ; set ; }

        /// <summary>
        /// 获取elastic client
        /// </summary>
        /// <returns></returns>
        public ElasticClient GetClient()
        {
            if (_EsConfig == null || _EsConfig.Value == null || _EsConfig.Value.Urls == null || _EsConfig.Value.Urls.Count < 1)
            {
                throw new Exception("urls can not be null");
            }
            return GetClient(_EsConfig.Value.Urls.ToArray(), "indexname");
        }
        /// <summary>
        /// 指定index获取ElasticClient
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public ElasticClient GetClient(string indexName)
        {
            if (_EsConfig == null || _EsConfig.Value == null || _EsConfig.Value.Urls == null || _EsConfig.Value.Urls.Count < 1)
            {
                throw new Exception("urls can not be null");
            }
            return GetClient(_EsConfig.Value.Urls.ToArray(), indexName);
        }

        public IElasticLowLevelClient JsonClient()
        {
            return _elasticJsonClient; 
        }

        public IElasticClient LinqClient()
        {
            return _elasticLinqClient;
        }


        /// <summary>
        /// 根据url获取ElasticClient
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        private ElasticClient GetClient(string url, string defaultIndex = "")
        {
            var urls = "";
          
            var uris = url.Split(",").ToList().ConvertAll(x => new Uri(x));//配置节点地址，以，分开
            var connectionPool = new StaticConnectionPool(uris);//配置请求池
            var settings = new ConnectionSettings(connectionPool)                
                .RequestTimeout(TimeSpan.FromSeconds(30));//请求配置参数
       
            return new ElasticClient(settings);
        }
        /// <summary>
        /// 根据urls获取ElasticClient
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="defaultIndex"></param>
        /// <returns></returns>
        private ElasticClient GetClient(string[] urls, string defaultIndex = "")
        {
            if (urls == null || urls.Length < 1)
            {
                throw new Exception("urls can not be null");
            }
            var uris = urls.Select(p => new Uri(p)).ToArray();
            var connectionPool = new SniffingConnectionPool(uris);
            var connectionSetting = new ConnectionSettings(connectionPool);
            if (!string.IsNullOrWhiteSpace(defaultIndex))
            {
                connectionSetting.DefaultIndex(defaultIndex);
            }
          /* var username = null;
            var password = null;
            if (username != null && password != null)
            {
                connectionSetting.BasicAuthentication(username, password);
            }*/
            _elasticLinqClient = new ElasticClient(connectionSetting);
            _elasticJsonClient = new ElasticLowLevelClient(connectionSetting);          
            return new ElasticClient(connectionSetting);
        }
    }
}

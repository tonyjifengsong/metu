using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES
{
    public class EsServerClientProvider : IEsServerClientProvider
    {
        private readonly IConfiguration _configuration;
        private ElasticClient _client;
        public EsServerClientProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ElasticClient GetClient()
        {
            if (_client != null)
                return _client;

            InitClient();
            return _client;
        }

        private void InitClient()
        {
           // var uris = _configuration["ElasticSearchContext:Url"];//.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(x => new Uri(x));

            var uris = _configuration["ElasticSearchContext:Url"].Split(",").ToList().ConvertAll(x => new Uri(x));//配置节点地址，以，分开
            var connectionPool = new StaticConnectionPool(uris);//配置请求池
            var settings = new ConnectionSettings(connectionPool).RequestTimeout(TimeSpan.FromSeconds(30));//请求配置参数
            this._client = new ElasticClient(settings);//linq请求客户端初始化
      
        }
    }
}

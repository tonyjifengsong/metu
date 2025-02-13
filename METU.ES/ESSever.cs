using Elasticsearch.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace METU.ES
{
    public class ESSever : IESSever
    { 
        private readonly IConfiguration _configuration;
        private ElasticClient _client;
        public ESSever(IConfiguration configuration)
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
            var uris = _configuration["ElasticSearchContext:Url"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(x => new Uri(x));
            var connectionPool = new StaticConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool).RequestTimeout(TimeSpan.FromSeconds(30));//.BasicAuthentication("elastic", "n@@W#RJQ$z1#")
            this._client = new ElasticClient(settings);
          
        }
       

  


        /// <summary>
        /// 封装后的创建index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="shards">分片数量，即数据块最小单元</param>
        /// <returns></returns>
        public virtual async Task<bool> CreateIndexAsync(string indexName, int shards = 5)
        {
           
            return false;
        }


        /// <summary>
        /// 检测索引是否已经存在
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsIndexExsit(string index)
        {
          
            return false;
        }

        /// <summary>
        /// 封装后的删除index
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteIndexAsync(string indexName)
        {
           
            return false;
        }
        /// <summary>
        /// 插入单个文档
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="typeName">文档名称</param>
        /// <param name="objectDocment">文档内容</param>
        /// <param name="_id">自定义_id</param>
        /// <returns></returns>
        public virtual async Task<bool> InsertDocumentAsync(string indexName, string typeName, object objectDocment, string _id = "")
        {
           
            return false;
        }


        /// <summary>
        /// 优化写入性能
        /// </summary>
        /// <param name="index"></param>
        /// <param name="refresh"></param>
        /// <param name="replia"></param>
        /// <returns></returns>
        public virtual async Task<bool> SetIndexRefreshAndReplia(string index, string refresh = "30s", int replia = 1)
        {
            bool flag = false;
            StringResponse resStr = null;
            
            
            return flag;
        }

        /// <summary>
        /// 批量插入文档
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="typeName"></param>
        /// <param name="listDocment">数据集合</param>
        /// <returns></returns>
        [Obsolete]
        public virtual async Task<bool> InsertListDocumentAsync(string indexName, string typeName, List<object> listDocment)
        {
            
            return false;
        }

        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="typeName">类别名称</param>
        /// <param name="_id">elasticsearch的id</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteDocumentAsync(string indexName, string typeName, string _id)
        {
            bool flag = false;
            StringResponse resStr = null;
            

            return flag;
        }


        /// <summary>
        /// 更新文档  
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="typeName">类别名称</param>
        /// <param name="_id">elasticsearch的id</param>
        /// <param name="objectDocment">单条数据的所有内容</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateDocumentAsync(string indexName, string typeName, string _id, object objectDocment)
        {
            bool flag = false;
            
            return flag;
        }

        /// <summary>
        /// 批量更新文档
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="typeName">类别名称</param>
        /// <param name="listDocment">数据集合，注：docment 里要有_id,否则更新不进去</param>
        /// <returns></returns>

        public virtual async Task<bool> UpdateListDocumentAsync(string indexName, string typeName, List<object> listDocment)
        {
            bool flag = false;
            
            return flag;
        }

    } 
}

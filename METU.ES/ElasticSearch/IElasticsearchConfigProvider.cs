using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.ElasticSearch
{
    public interface IElasticsearchConfigProvider
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        Task<ElasticsearchConfig> GetConfigAsync();
    }
}

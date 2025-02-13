using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.ElasticSearch
{
    /// <summary>
    /// 指定字段查询
    /// </summary>
    public class PageParamWithSearch : PageParam
    {
        /// <summary>
        /// 查询字段列表
        /// </summary>
        public string[] SearchKeys { get; set; }
    }
}

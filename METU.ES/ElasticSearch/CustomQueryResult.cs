using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.ElasticSearch
{
    /// <summary>
    /// 自定义查询结果
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class CustomQueryResult<T> : IQueryResult<T>
    {
        /// <summary>
        /// 总行数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 查询占用时间
        /// </summary>
        public long Took { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.ElasticSearch
{
    /// <summary>
    /// 查询结果
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IQueryResult<T>
    {
        /// <summary>
        /// 总行数
        /// </summary>
        long TotalCount { get; set; }

        /// <summary>
        /// 查询占用时间
        /// </summary>
        long Took { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        IEnumerable<T> Data { get; }
    }

}

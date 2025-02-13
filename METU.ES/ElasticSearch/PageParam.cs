using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.ElasticSearch
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageParam : IPageParam
    {
        /// <summary>
        /// 页数，即第几页，从1开始
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页显示行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 获取跳过的行数
        /// </summary>
        /// <returns></returns>
        public int GetSkipCount() => (Page - 1) * PageSize;

        /// <summary>
        /// 运算符
        /// </summary>
        public Nest.Operator Operator { get; set; } = Nest.Operator.And;

        /// <summary>
        /// 高亮参数
        /// </summary>
        public HighlightParam Highlight { get; set; }
    }

}

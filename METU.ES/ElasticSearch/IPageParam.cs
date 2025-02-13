using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.ElasticSearch
{
    public interface IPageParam
    {
        /// <summary>
        /// 页数，即第几页，从1开始
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// 每页显示行数
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        string Keyword { get; set; }

        /// <summary>
        /// 获取跳过的行数
        /// </summary>
        /// <returns></returns>
        int GetSkipCount();

        /// <summary>
        /// 运算符
        /// </summary>
        Nest.Operator Operator { get; set; }

        /// <summary>
        /// 高亮参数
        /// </summary>
        HighlightParam Highlight { get; set; }
    }
}

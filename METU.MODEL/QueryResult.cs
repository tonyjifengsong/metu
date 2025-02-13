using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    /// <summary>
    /// 通用分页查询结果
    /// </summary>
    public class QueryResult : ParamPageEntity
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageNO { get; set; }
        /// <summary>
        /// 上一页 页码
        /// </summary>

        public int PrePage { get { return CurrentPageNO <= 1 ? 1 : CurrentPageNO - 1; } }
        /// <summary>
        /// 下一页面码
        /// </summary>
        public int NextPage { get { return CurrentPageNO >= 100 ? 100 : CurrentPageNO + 1; } }
        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; set; }
    }
}

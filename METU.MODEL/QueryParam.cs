using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class QueryParam : ParamPageEntity
    { /// <summary>
      /// 
      /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public string searchtxt { get; set; }
        public string sidx { get; set; }
    }
}

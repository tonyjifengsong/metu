using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    public class Pager
    {
        public Pager()
        {
            pagesize = 5;
            pageindex = 1;
        }
        /// <summary>
        /// 页容量
        /// </summary>
        public int pagesize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int pageindex { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// 正序还是倒序
        /// </summary>
        public string sord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long records { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long total { get; }
    }
}

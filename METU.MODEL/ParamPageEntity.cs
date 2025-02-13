using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    /// <summary>
    /// 分页基类
    /// </summary>
    public class ParamPageEntity 
    {/// <summary>
     /// 
     /// </summary>
        public string id { get; set; }
        int _pageindex = 0;
        int _pagesize = 1;
        /// <summary>
        /// 
        /// </summary>
        public ParamPageEntity()
        {
            //pageindex = 0;
            //pagesize =ConstNum.Ten;
        }
        /// <summary>
        /// 页码
        /// </summary>
        public int pageindex
        {
            get
            {
                if (_pageindex < 1 || _pageindex > 10000) return 1;
                return _pageindex;
            }
            set
            {
                _pageindex = value;
            }
        }
        /// <summary>
        /// 页大小 
        /// </summary>
        public int pagesize
        {
            get
            {
                if (_pagesize < 0 || _pagesize > 100) return 100;
                return _pagesize;
            }
            set
            {
                _pagesize = value;
            }
        }

    }
}

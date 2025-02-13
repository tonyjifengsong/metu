using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.ICore
{
    public interface IParamPageEntity
    {/// <summary>
     /// 
     /// </summary>
        int pageindex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        int pagesize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string id { get; set; }
    }
}

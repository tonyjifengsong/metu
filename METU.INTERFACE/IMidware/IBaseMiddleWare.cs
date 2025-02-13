using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseMiddleWare
    {/// <summary>
     /// 
     /// </summary>
     /// <param name="context"></param>
     /// <param name="paramlist"></param>
     /// <returns></returns>
        bool DoWork(HttpContext context, params object[] paramlist);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        bool DoWorking(HttpContext context, params object[] paramlist);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        bool DoWorked(HttpContext context, params object[] paramlist);
    }
}
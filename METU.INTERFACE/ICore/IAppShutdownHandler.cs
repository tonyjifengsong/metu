using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.ICore
{
    /// <summary>
    /// 应用关闭处理接口
    /// </summary>
    public interface IAppShutdownHandler
    {
        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        Task Handle();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
 public   interface IEventFunc
    {
        string MSG { get; set; }
        bool isSuccess { get; set; }
        /// <summary>
        /// 调用类中自定义方法
        /// </summary>
        /// <param name="paramlist">方法名称及参数</param>
        /// <returns>返回RESULTS结果</returns>
        dynamic DoWork(object paramlist, EventResultType resultType = EventResultType.LAST);
            
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Func<Dictionary<string, string>, object> BusinessBus;
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Func<Dictionary<string, object>, object> BsBus;

    }
}

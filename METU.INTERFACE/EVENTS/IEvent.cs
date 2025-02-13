using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    public interface IEvent
    { 
        bool isSuccess { get; set; }
        string MSG { get; set; }
        /// <summary>
        /// 调用类中自定义方法
        /// </summary>
        /// <param name="paramlist">方法名称及参数</param>
        /// <returns>返回RESULTS结果</returns>
        dynamic DoWork(object paramlist, EventResultType resultType = EventResultType.LAST);
        void DoAction(object paramlist, object obj);
        
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Func<Dictionary<string, string>, object> BusinessBus;
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Func<Dictionary<string, object>, object> BsBus;
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Action<Dictionary<string, string>, object> Business;
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Action<Dictionary<string, object>, object> Bs;
    }
}

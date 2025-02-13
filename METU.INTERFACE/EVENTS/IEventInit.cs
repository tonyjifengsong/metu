using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{

    public interface IEventInit
    {
        string MSG { get; set; }
        bool isSuccess { get; set; }
        /// <summary>
        /// 调用类中自定义方法
        /// </summary>
        /// <param name="paramlist">方法名称及参数</param>
        /// <returns>返回RESULTS结果</returns>
        void DoWork(object paramlist, object obj);
        
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Action<Dictionary<string, string>, object> BusinessBus;
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Action<Dictionary<string, object>, object> BsBus;
        /// <summary>
        /// 初始化业务总线事件（非队列）
        /// </summary>
        void InitialBus();
        /// <summary>
        /// 初始化业务总线事件（队列）
        /// </summary>
        void InitialQueue();
        /// <summary>
        /// 清空事务总线中事件
        /// </summary>
        void ClearEventBus();
        /// <summary>
        /// 清空队列事务中事件
        /// </summary>
        void ClearEventQueue();
     
        /// <summary>
        /// 执行队列中具体业务方法
        /// </summary>
        /// <param name="dic">参数</param>
        /// <param name="obj">参数</param>
        void DoQueueWork(Dictionary<string, string> dic, object obj);

    }
}

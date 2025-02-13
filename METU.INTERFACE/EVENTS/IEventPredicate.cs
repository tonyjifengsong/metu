using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
  public  interface IEventPredicate
    {
        string MSG { get; set; }
        bool isSuccess { get; set; }

        /// <summary>
        /// 调用类中自定义方法
        /// </summary>
        /// <param name="paramlist">方法名称及参数</param>
        /// <returns>返回RESULTS结果</returns>
        dynamic DoWork( object paramlist, EventResultType resultType = EventResultType.LAST);
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Predicate<Dictionary<string, string>> BusinessBus;
        /// <summary>
        /// 业务处理总线事件（非队列）
        /// </summary>
        event Predicate<Dictionary<string, object>> BsBus;
        event Predicate< object > BoBus;

    }
}

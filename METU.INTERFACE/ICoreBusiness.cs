using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{

    public interface ICoreBusiness
    {
        /// <summary>
        /// 调用类中自定义方法
        /// </summary>
        /// <param name="paramlist">方法名称及参数</param>
        /// <returns>返回RESULTS结果</returns>
        dynamic DoWork(params object[] paramlist);
        /// <summary>
        /// 通过方法名称及方法参数执行方法
        /// </summary>
        /// <param name="actionname">类中方法名称</param>
        /// <param name="paramlist">方法参数列表</param>
        /// <returns></returns>
        dynamic DoWork(string actionname = null, params object[] paramlist);

        /// <summary>
        /// 调用类中定义的方法；方法的参数必需是Dictionary<string_string> dic, object obj；并且无返回值
        /// </summary>
        /// <param name="dic">调用的方法参数</param>
        /// <param name="obj">调用的方法返回对象或参数</param>
        /// <param name="actionname">调用的方法名称</param>
        void DoWork(Dictionary<string, string> dic, object obj, string actionname = null);
        void DoWork(Dictionary<string, object> dic, object obj, string actionname = null);
    }
}

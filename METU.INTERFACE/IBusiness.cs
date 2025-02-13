using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    /// <summary>
    ///  created by  tony  2017-12-26
    /// </summary>
    public interface IBusiness : ICoreBusiness
    {
        /// <summary>
        /// 通过SQL模板执行SQL并返回结果
        /// </summary>
        /// <param name="dic">模板参数列表</param>
        /// <param name="ServiceName">模板名称</param>
        /// <returns></returns>
        DataTable ExecuteService(Dictionary<string, string> dic, string ServiceName);
       /// <summary>
        /// 执行SQL返回object
        /// </summary>
        /// <param name="Dic"></param>
        /// <param name="functionname"></param>
        /// <returns></returns>
        object ExecuteSQL(Dictionary<string, string> Dic, string functionname = null);
        /// <summary>
        /// 初始化缓存
        /// </summary>
        void InitialCaches();
        /// <summary>
        /// 执行通用业务
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="obj"></param>
        void ExecuteService(Dictionary<string, string> dic, object obj);
        /// <summary>
        /// 执行通用业务
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="obj"></param>
        void ExecuteoService(Dictionary<string, object> dic, object obj);
    }
}

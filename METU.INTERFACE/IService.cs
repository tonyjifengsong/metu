using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    public interface IService : IBase
    {
        bool CheckValidate(Dictionary<string, object> Dic);
        DataTable ExecuteDt(Dictionary<string, object> Dic);
        object Execute(Dictionary<string, object> Dic);
        object DoBefore(Dictionary<string, object> Dic);
        object DoAfter(Dictionary<string, object> Dic);
        bool CheckReturnValidate(Dictionary<string, object> Dic);
        string ExecuteDtByTemplate(Dictionary<string, object> Dic);
        /// <summary>
        /// BLL中方法参数及模板名称调用BLL层业务逻辑
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="servicename">及模板名称</param>
        /// <returns></returns>
        dynamic ExecuteService(Dictionary<string, string> dic, string servicename = null);
        /// <summary>
        /// BLL中方法参数及模板名称调用BLL层业务逻辑
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="servicename">及模板名称</param>
        /// <returns></returns>
        dynamic ExecuteService(Dictionary<string, object> dic, string servicename = null);
        /// <summary>
        /// 通过BLL层类名称及BLL中方法参数调用BLL层业务逻辑
        /// </summary>
        /// <param name="servicename">BLL层类名称</param>
        /// <param name="paramlist">BLL中方法参数（参数中必需包括所调用的方法）</param>
        /// <returns></returns>
        dynamic ExecuteService(string servicename = null, params object[] paramlist);
        /// <summary>
        /// 通过BLL名称及BLL中方法名称调用BLL层业务逻辑
        /// </summary>
        /// <param name="servicename">BLL层类名称</param>
        /// <param name="actionname">BLL方法名称</param>
        /// <param name="paramlist">BLL中方法参数</param>
        /// <returns></returns>
        dynamic ExecuteService(string servicename = null, string actionname = null, params object[] paramlist);
    }
}

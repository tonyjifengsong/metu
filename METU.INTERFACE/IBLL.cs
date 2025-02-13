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
    public interface IBLL
    {


        /// <summary>
        /// 获取模板的ＳＱＬ语句
        /// </summary>
        /// <param name="templatename">模板名称</param>
        /// <returns></returns>
        DataTable GetTemplateDt(string templatename = "");
        /// <summary>
        /// 判断返回值是否符合要求
        /// </summary>
        /// <param name="obj">返回值对象</param>
        /// <returns>返回是否符合要求，默认返回false</returns>
        bool CheckReturn(dynamic obj);

        object ExecuteSQL(Dictionary<string, string> Dic, string functionname = null);
      
        /// <summary>
        /// 通过模板名称及模板参数返回执行结果
        /// </summary>
        /// <param name="Dic">模板参数</param>
        /// <param name="functionname">模板名称</param>
        /// <returns>返回True或False</returns>
        bool ExecuteByTemplate(Dictionary<string, string> Dic, string functionname = null);
        /// <summary>
        /// 通过模板名称及模板参数返回执行结果
        /// </summary>
        /// <param name="Dic">模板参数</param>
        /// <param name="functionname">模板名称</param>
        /// <returns>返回DataTable</returns>
        DataTable ExecuteDtByTemplate(Dictionary<string, string> Dic, string functionname = null);
       
        DataTable ExecuteSQL(string sqlstr);
        object GetScalar(string sqlstr);
    }
}

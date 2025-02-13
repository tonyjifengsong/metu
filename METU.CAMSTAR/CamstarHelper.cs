using METU.INTERFACE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{
    public class CamstarHelper : CamstarBase, IDBHelper
    {


        /// <summary>
        /// 添加数据到数据库中
        /// </summary>
        /// <param name="model">参数对象</param>
        /// <param name="Sqlstr">添加SQL语句模板</param>
        /// <returns>默认返回false，操作失败</returns>
        public virtual bool Add(dynamic model, string Sqlstr = "")
        {
            Dictionary<string, string> dic = model.ToDictionary();
            foreach (var item in dic)
            {
                Sqlstr = Sqlstr.SetParmValue(item.Key, item.Value);
            }
            var flag = ExecuteCamstarXML(Sqlstr);
            return false;
        }

        /// <summary>
        /// 根据条件删除对象
        /// </summary>
        /// <param name="model">删除对象参数</param>
        /// <param name="Sqlstr">删除SQL语句模板</param>
        /// <returns>默认返回false，操作失败</returns>
        public virtual bool Delete(dynamic model, string Sqlstr = "")
        {
            Dictionary<string, string> dic = model.ToDictionary();
            foreach (var item in dic)
            {
                Sqlstr = Sqlstr.SetParmValue(item.Key, item.Value);
            }
            var flag = ExecuteCamstarXML(Sqlstr);
            return false;
        }

        /// </summary>
        /// 通过SQL获取datatable
        /// </summary>
        /// <param name="sqlstr">SQL参数</param>
        /// <returns>返回DATATABLE</returns>

        public virtual DataTable executeDt(string sqlstr)
        {
            DataTable dt = GetXMLForDataTable(sqlstr);
            return dt;
        }
        /// <summary>
        /// 通过参数列表与SQL模板执行返回结果DATATABLE
        /// </summary>
        /// <param name="dic"> 参数对象</param>
        /// <param name="Sqlstr">SQL模板</param>
        /// <returns>返回执行结果DATATABLE</returns>
        public virtual DataTable SearchSQL(Dictionary<string, string> dic, string Sqlstr = "")
        {
            foreach (var item in dic)
            {
                Sqlstr = Sqlstr.SetParmValue(item.Key, item.Value);
            }
            DataTable dt = GetXMLForDataTable(Sqlstr);
            return dt;
        }
        /// <summary>
        /// 通过SQL获取单个对象
        /// </summary>
        /// <param name="sqlstr">CamstarXML参数</param>
        /// <returns>返回执行结果字符串</returns>
        public virtual object executesql(string sqlstr)
        {

            var flag = ExecuteCamstarXML(sqlstr);
            return flag;
        }



        /// <summary>
        /// 查询单个对象信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回单个对象</returns>
        public virtual object Search(dynamic model, string Sqlstr = "")
        {
            Dictionary<string, string> dic = model.ToDictionary();
            foreach (var item in dic)
            {
                Sqlstr = Sqlstr.SetParmValue(item.Key, item.Value);
            }
            var flag = ExecuteCamstarXML(Sqlstr);
            return flag;
        }

        /// <summary>
        /// 查询数据库返回datatable
        /// </summary>
        /// <param name="model">查询参数</param>
        /// <returns></returns>
        public virtual DataTable SearchDt(dynamic model, string Sqlstr = "")
        {
            Dictionary<string, string> dic = model.ToDictionary();
            foreach (var item in dic)
            {
                Sqlstr = Sqlstr.SetParmValue(item.Key, item.Value);
            }
            DataTable dt = GetXMLForDataTable(Sqlstr);
            return dt;
        }

        /// <summary>
        /// 更新单个数据对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns>默认返回false，操作失败</returns>
        public virtual bool Update(dynamic model, string Sqlstr = "")
        {
            Dictionary<string, string> dic = model.ToDictionary();
            foreach (var item in dic)
            {
                Sqlstr = Sqlstr.SetParmValue(item.Key, item.Value);
            }
            var flag = ExecuteCamstarXML(Sqlstr);
            return flag != null;
        }
    }
}

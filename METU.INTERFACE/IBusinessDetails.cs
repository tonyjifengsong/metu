using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{

    public interface IBusinessDetails
    {
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="Sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，使用模板库中的ＳＱＬ时，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>
        bool Add(dynamic model, string Sqlstr = "", bool CamstarFlag = false);
        /// <summary>
        /// 删除数据库中数据（可以批量删除）
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="Sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>

        bool Delete(dynamic model, string Sqlstr = "", bool CamstarFlag = false);
        /// <summary>
        /// 更新数据库中数据，（可以批量更新）
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="Sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>

        bool Update(dynamic model, string Sqlstr = "", bool CamstarFlag = false);
        /// <summary>
        /// 查询并返回DATATABLE
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>

        DataTable ExecuteSQL(string sqlstr);

    }
}

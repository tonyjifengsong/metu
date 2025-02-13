using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.EXTENTIONS
{
    /// <summary>
    /// Created by tony
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class XMLExtentionAttribute : Attribute
    {


        public XMLExtentionAttribute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlattributename">xml扩展属性</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="regex">字段输入规则</param>
        /// <param name="validmessage">错误提示信息</param>
        /// <param name="dbfieldType">扩展方式</param>
        /// <param name="description">字段描述</param>
        /// <param name="fieldname">字段名称</param>
        public XMLExtentionAttribute(string xmlattributename = null, object defaultvalue = null, string regex = null, string validmessage = null, DBFieldType dbfieldType = DBFieldType.None, string description = null, string fieldname = null)
        {
            xmlAttributeName = xmlattributename;
            defaultValue = defaultvalue;
            RegEx = regex;
            ValidMessage = validmessage;
            DbFieldType = dbfieldType;
            Description = description;
            FieldName = fieldname;

        }
        /// <summary>
        /// xml扩展属性
        /// </summary>
        public string xmlAttributeName { get; set; }
        /// <summary>
        /// 扩展方式
        /// </summary>
        public DBFieldType DbFieldType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 字段输入规则
        /// </summary>
        public string RegEx { get; set; }
        /// <summary>
        /// 错误提示信息
        /// </summary>
        public string ValidMessage { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object defaultValue { get; set; }
        public string GetExtentionXML()
        {
            if (DbFieldType == DBFieldType.Others)
            {
                return xmlAttributeName;
            }
            return xmlAttributeName = "<" + FieldName + " " + xmlAttributeName + ">{VALUE}</" + FieldName + ">";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.ObjectModel;

namespace System
{
    public static class StringExtentions
    {
        /// <summary>
        /// 替换{}标签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetParmValue<T>(this T model, string name, string value)
        {
            string str = model.ToString();
            if (name != null) str = str.Replace("{" + name.ToUpper() + "}", value);

            return str;
        }
        /// <summary>
        /// 加密XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EnCode<T>(this T model, string value = "~")
        {
            string name = "<";
            string str = model.ToString();
            if (name != null) str = str.Replace(name, value);
            return str;
        }
        /// <summary>
        /// 解密XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DeCode<T>(this T model, string name = "~")
        {
            string value = "<";
            string str = model.ToString();
            if (name != null) str = str.Replace(name, value);
            return str;
        }
        #region 替换SQL文本中的会与XML冲突的符号，例如：>,<,&,',"等   
        /// <summary>
        /// 替换SQL文本中的会与XML冲突的符号，例如：>,<,&,',"等
        /// </summary>
        /// <param name="model">需要替换的文本</param>
        /// <returns></returns>
        public static string ReplaceSQlSymbol<T>(this T model)
        {
            string Text = model.ToString();
            //替换&符号
            Text = Text.Replace("&", "&amp;");
            //替换<符号
            Text = Text.Replace("<", "&lt;");
            //替换>符号
            Text = Text.Replace(">", "&gt;");
            //替换'符号
            Text = Text.Replace("'", "&apos;");
            //替换"符号
            Text = Text.Replace("\"", "&quot;");
            return Text;
        }
        #endregion

        /// <summary>
        /// 查找ＸＭＬ节点值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">ＸＭＬ字符串</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns></returns>
        public static string XMLSearchInnerNode<T>(this T model, string NodeName = "~")
        {
            string xmlstr = model.ToXML();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlstr.ToString());

            XmlNode xmlnode = xmldoc.SelectSingleNode(NodeName);

            string str = xmlnode.InnerText;

            return str;
        }
        /// <summary>
        /// 查找ＸＭＬ节点值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">ＸＭＬ字符串</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns>返回节点ＸＭＬ</returns>
        public static string XMLSearchInnerXmlNode<T>(this T model, string NodeName = "~")
        {
            try
            {
                string xmlstr = model.ToXML();

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlstr.ToString());

                XmlNode xmlnode = xmldoc.SelectSingleNode(NodeName);
                NodeName = "//" + NodeName;
                string str = xmlnode.InnerXml;
                if (str == null) return "";
                return str;
            }
            catch (Exception ex)
            {
                return model.ToString();
            }
        }
        /// <summary>
        /// 通过节点名称从ＸＭＬ字符串中获取节点字符串
        /// </summary>
        /// <param name="model">ＸＭＬ字符串</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns></returns>
        public static string XMLSearchInnerXmlNode(this string model, string NodeName = "")
        {
            try
            {
                string xmlstr = model;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlstr.ToString());
                NodeName = "//" + NodeName;
                XmlNode xmlnode = xmldoc.SelectSingleNode(NodeName);
                if (xmlnode == null) return "";

                string str = xmlnode.InnerXml;
                if (str == null) return "";
                return str;
            }
            catch (Exception ex)
            {
                return model.ToString();
            }
        }
        /// <summary>
        /// 通过查询节点名称，为节点赋值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="nodevalue">节点值</param>
        /// <returns></returns>
        public static string XMLSetValueBySearchInnerXmlNode(this string model, string NodeName = "", string nodevalue = "")
        {
            try
            {
                string xmlstr = model;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlstr.ToString());
                NodeName = "//" + NodeName;
                XmlNode xmlnode = xmldoc.SelectSingleNode(NodeName);

                if (xmlnode == null) return model;

                string str = xmlnode.InnerXml;
                if (str == null) return model;
                xmldoc.SelectSingleNode(NodeName).RemoveAll();
                xmldoc.SelectSingleNode(NodeName).InnerXml = nodevalue;
                model = xmldoc.InnerXml.ToString();
                return model;
            }
            catch (Exception ex)
            {

                return model.ToString();
            }
        }
        /// <summary>
        /// 通过节点名称从ＸＭＬ字符串中获取节点值
        /// </summary>
        /// <param name="model">ＸＭＬ字符串</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns></returns>
        public static string XMLSearchInnerTextNode(this string model, string NodeName = "")
        {
            try
            {
                string xmlstr = model;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlstr.ToString());
                NodeName = "//" + NodeName;
                XmlNode xmlnode = xmldoc.SelectSingleNode(NodeName);
                if (xmlnode == null) return "";

                string str = xmlnode.InnerText;
                if (str == null) return "";
                return str;
            }
            catch (Exception ex)
            {
                return model.ToString();
            }
        }
        /// <summary>
        /// 控件判断
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="datatype">类型</param>
        /// <param name="regStr">规则（正则表达式）</param>
        /// <returns></returns>
        public static string StringIsValidate(this string ctl, DataType datatype = DataType.String, string regStr = null)
        {
            string str = ctl;
            if (str == null) return "不可以为空！";
            if (str.Trim().ToString().Length < 1) return "不可以为空！";
            if (datatype == DataType.String)
            {
                try
                {
                    if (str == null) return "不可以为空！";
                    if (str.Trim().ToString().Length < 1) return "不可以为空！";
                    if (regStr != null)
                    {
                        if (!Regex.IsMatch(str, regStr))
                        {
                            return "数据不符合规则；规则表达式为：" + regStr + "！";
                        }
                    }
                }
                catch (Exception ex)
                {
                    return "信息出错！";
                }
            }
            if (datatype == DataType.BOOL || datatype == DataType.BOOLEAN)
            {
                try
                {
                    bool flag = bool.Parse(str);
                }
                catch (Exception ex)
                {
                    return "信息出错！";
                }
            }
            if (datatype == DataType.BOOLEAN)
            {
                try
                {
                    Boolean flag = Boolean.Parse(str);
                }
                catch (Exception ex)
                {
                    return "信息出错！";
                }
            }
            if (datatype == DataType.DATE)
            {
                try
                {
                    DateTime flag = DateTime.Parse(str);
                }
                catch (Exception ex)
                {
                    return "日期信息错误！";
                }
            }
            if (datatype == DataType.INT)
            {
                try
                {
                    int flag = int.Parse(str);
                }
                catch (Exception ex)
                {
                    return "数值出错！";
                }
            }
            if (datatype == DataType.INT32)
            {
                try
                {
                    Int32 flag = Int32.Parse(str);
                }
                catch (Exception ex)
                {
                    return "数值出错！";
                }
            }
            if (datatype == DataType.INT64)
            {
                try
                {
                    Int64 flag = Int64.Parse(str);
                }
                catch (Exception ex)
                {
                    return "数值出错！";
                }
            }
            if (datatype == DataType.LONG)
            {
                try
                {
                    long flag = long.Parse(str);
                }
                catch (Exception ex)
                {
                    return "数值出错";
                }
            }
            if (datatype == DataType.FLOAT)
            {
                try
                {
                    float flag = float.Parse(str);
                }
                catch (Exception ex)
                {
                    return "数值出错！";
                }
            }
            if (datatype == DataType.DOUBLE)
            {
                try
                {
                    double flag = double.Parse(str);
                }
                catch (Exception ex)
                {
                    return "数值出错！";
                }
            }
            if (datatype == DataType.DATETIME)
            {
                try
                {
                    DateTime flag = DateTime.Parse(str);
                }
                catch (Exception ex)
                {
                    return "日期出错！";
                }
            }

            return "True";

        }
        /// <summary>
        /// 控件判断
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="datatype">类型</param>
        /// <param name="regStr">规则（正则表达式）</param>
        /// <returns></returns>
        public static bool CheckStringValue(this string ctl, DataType datatype = DataType.String, string regStr = null)
        {
            string str = ctl;
            if (str == null) return false;
            if (str.Trim().ToString().Length < 1) return false;
            if (datatype == DataType.String)
            {
                try
                {
                    if (str == null) return false;
                    if (str.Trim().ToString().Length < 1) return false;
                    if (regStr != null)
                    {
                        if (!Regex.IsMatch(str, regStr))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.BOOLEAN)
            {
                try
                {
                    Boolean flag = Boolean.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.BOOL)
            {
                try
                {
                    bool flag = bool.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.DATE)
            {
                try
                {
                    DateTime flag = DateTime.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.INT)
            {
                try
                {
                    int flag = int.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.INT32)
            {
                try
                {
                    Int32 flag = Int32.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.INT64)
            {
                try
                {
                    Int64 flag = Int64.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.LONG)
            {
                try
                {
                    long flag = long.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.FLOAT)
            {
                try
                {
                    float flag = float.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.DOUBLE)
            {
                try
                {
                    double flag = double.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            if (datatype == DataType.DATETIME)
            {
                try
                {
                    DateTime flag = DateTime.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;

        }
        /// <summary>
        /// 字符串转COLLICTION
        /// </summary>
        /// <param name="model">字符串</param>
        /// <param name="splitStr">字符串间分隔符号</param>
        /// <returns></returns>
        public static ICollection<string> ToIcollection(this string model, string splitStr = "")
        {
            ICollection<string> rs = new Collection<string>();
            try
            {
                string xmlstr = model;

                if (xmlstr == null) return rs;
                if (xmlstr.Trim().Length <= 0) return rs;
                if (splitStr == null) splitStr = ";";
                if (splitStr.Trim().Length == 0) splitStr = ";";
                string[] arr = model.Split(splitStr);
                for (int i = 0; i < arr.Length; i++)
                {
                    rs.Add(arr[i]);
                }
                return rs;
            }
            catch (Exception ex)
            {
                return rs;
            }
        }
        /// <summary>
        /// 字符串转列表
        /// </summary>
        /// <param name="model">字符串</param>
        /// <param name="splitStr">分隔符</param>
        /// <returns></returns>
        public static List<string> ToList(this string model, string splitStr = "")
        {
            List<string> rs = new List<string>();
            try
            {
                string xmlstr = model;

                if (xmlstr == null) return rs;
                if (xmlstr.Trim().Length <= 0) return rs;
                if (splitStr == null) splitStr = ";";
                if (splitStr.Trim().Length == 0) splitStr = ";";
                string[] arr = model.Split(splitStr);
                for (int i = 0; i < arr.Length; i++)
                {
                    rs.Add(arr[i]);
                }
                return rs;
            }
            catch (Exception ex)
            {
                return rs;
            }
        }
        /// <summary>
        /// 字符串转字典
        /// </summary>
        /// <param name="model">字符串</param>
        /// <param name="splitStr">字典数据分隔符号</param>
        /// <param name="keyStr">KEY  Value之间分隔符号</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictinary(this string model, string splitStr = ";", string keyStr = ":")
        {
            Dictionary<string, string> rs = new Dictionary<string, string>();
            try
            {
                string xmlstr = model;

                if (xmlstr == null) return rs;
                if (xmlstr.Trim().Length <= 0) return rs;
                if (splitStr == null) splitStr = ";";
                if (splitStr.Trim().Length == 0) splitStr = ";";
                string[] arr = model.Split(splitStr);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (keyStr == null) keyStr = ":";
                    if (keyStr.Trim().Length == 0) keyStr = ":";
                    string dicstr = arr[i];
                    if (dicstr == null) continue;
                    if (dicstr.Trim().Length < 2) continue;
                    string[] dicarr = dicstr.Split(keyStr);
                    if (dicarr.Length == 2)
                    {
                        if (dicarr[0] == null) continue;
                        if (dicarr[0].Trim().Length < 2) continue;
                        rs.Add(dicarr[0], dicarr[1]);
                    }
                    if (dicarr.Length == 1)
                    {
                        if (dicarr[0] == null) continue;
                        if (dicarr[0].Trim().Length < 2) continue;
                        rs.Add(dicarr[0], "");
                    }
                }
                return rs;
            }
            catch (Exception ex)
            {
                return rs;
            }
        }
    }
}

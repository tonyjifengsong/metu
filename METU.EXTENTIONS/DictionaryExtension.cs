using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    public static class DictionaryExtension
    {/// <summary>
     /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
     /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key) == false) dict.Add(key, value);
            return dict;
        }
        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }
        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }
        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) == false || replaceExisted)
                    dict[item.Key] = item.Value;
            }
            return dict;
        }
        public static string KVPToCamstarXmlTemplate<T, V>(this KeyValuePair<T, V> instance, string strReg = null)
        {
            string result = "";
            if (strReg == null)
            {
                strReg = "<{KEY}>{{KEY}}</{KEY}>";

                if (instance.Key != null && instance.Value != null)
                {
                    result = strReg.Replace("{KEY}", instance.Key.ToString());
                }

            }
            else
            {
                if (strReg != null && strReg.Length > 10)
                {
                    if (instance.Key != null && instance.Value != null)
                    {
                        result = strReg.Replace("{KEY}", instance.Key.ToString());
                    }
                }
            }
            return result;
        }

        public static string KVPToCamstarXml<T, V>(this KeyValuePair<T, V> instance, string strReg = null)
        {
            string result = "";
            if (strReg == null)
            {
                strReg = "<{KEY}>{VALUE}</{KEY}>";

                if (instance.Key != null && instance.Value != null)
                {
                    result = strReg.Replace("{KEY}", instance.Key.ToString()).Replace("{VALUE}", instance.Value.ToString());
                }

            }
            else
            {
                if (strReg != null && strReg.Length > 10)
                {
                    if (instance.Key != null && instance.Value != null)
                    {
                        result = strReg.Replace("{" + instance.Key.ToString().ToUpper() + "}", instance.Value.ToString());
                    }
                }
            }
            return result;
        }
        public static string ToCamstarXml<T, V>(this Dictionary<T, V> instance, string strReg = null)
        {
            string result = "";

            foreach (var item in instance)
            {
                result = result + item.KVPToCamstarXml(strReg);

            }
            return result;
        }
        public static string ToCamstarXmlTemplate<T, V>(this Dictionary<T, V> instance, string strReg = null)
        {
            string result = "";

            foreach (var item in instance)
            {
                result = result + item.KVPToCamstarXmlTemplate(strReg);

            }
            return result;
        }

        /// <summary>
        /// 把对象的属性及属性值存储到Dictionary中
        /// </summary>
        /// <param name="obj">待扩展的对象</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToCamstarDictionary(this object obj)
        {
            Dictionary<string, string> Dics = new Dictionary<string, string>();
            //处理获取字段信息

            if (obj == null) { return null; }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(obj, null);
                if (value == null)
                {
                    Dics.Add(name, "");
                }
                else
                {
                    Dics.Add(name, value.ToString());
                }

            }

            return Dics;
        }

        public static string ListToCamstarXml<T>(this List<T> instance, string strReg = null)
        {
            string result = "";
            foreach (var item in instance)
            {
                var dic = item.ToDictionary();
                result += dic.ToCamstarXml();
            }

            return result;
        }

        public static List<T> CamstarKeyList<T, V>(this Dictionary<T, V> instance)
        {
            List<T> lists = new List<T>();

            foreach (var item in instance)
            {
                lists.Add(item.Key);

            }
            return lists;
        }
        public static List<V> CamstarValueList<T, V>(this Dictionary<T, V> instance)
        {
            List<V> lists = new List<V>();

            foreach (var item in instance)
            {
                lists.Add(item.Value);

            }
            return lists;
        }
        /// <summary>
        /// 获取方法执行结果
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>默认返回False</returns>
        public static string GetResult(this Dictionary<string, string> instance)
        {
            string result = "False";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "Result".ToUpper())
                {
                    result = item.Value.ToString();
                    break;
                }
            }
            return result;
        }
        public static Dictionary<string, string> ConDic(this Dictionary<string, string> instance, Dictionary<string, string> appeddic)
        {

            string result = "";
            if (instance == null) return appeddic;
            foreach (var item in appeddic)
            {
                instance.Add(item.Key, item.Value);
            }
            return instance;
        }
        public static Dictionary<string, string> UnionDic(this Dictionary<string, string> instance, Dictionary<string, string> appeddic)
        {


            if (instance == null) return appeddic;
            foreach (var item in appeddic)
            {
                if (!instance.Keys.Contains(item.Key))
                {
                    instance.Add(item.Key, item.Value);
                }
            }
            return instance;
        }
        /// <summary>
        /// 获取返回结果中的信息
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="key">KEY值</param>
        /// <returns>默认返回空</returns>
        public static string GetMessage(this Dictionary<string, string> instance, string key = null)
        {

            string result = "";
            if (key == null) return result;
            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == key.ToUpper())
                {
                    result = item.Value.ToString();
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 把Dictionary转化为任何对象（对象必需加入了DisplayNameAttribute属性，确保描述已经添加）
        /// </summary>
        /// <typeparam name="T">类型对象参数</typeparam>
        /// <param name="instance">待扩展的对象</param>
        /// <returns></returns>
        public static T MESObjClone2Obj<T>(this Dictionary<string, string> instance)
        {
            T obj = default(T);
            obj = System.Activator.CreateInstance<T>();
            var t = obj.GetType();
            var lists = obj.GetType().GetProperties();
            foreach (var items in instance)
            {
                foreach (var item in lists)
                {

                    var pname = t.GetProperty(item.Name);

                    var displaynamea = (DisplayNameAttribute)pname.GetCustomAttributes(true).FirstOrDefault();
                    string uname = displaynamea.DisplayName;
                    if (items.Key.ToString() == uname.ToString())
                    {

                        pname.SetValue(obj, items.Value, null);
                    }


                }


            }
            return obj;
        }
        /// <summary>
        /// 把对象的属性及属性值存储到Dictionary中
        /// </summary>
        /// <param name="obj">待扩展的对象</param>
        /// <returns></returns>
        public static Dictionary<string, string> ObjectToDictionary(this object obj)
        {
            if (obj is Dictionary<string, string>)
            {
                return (Dictionary<string, string>)obj;
            }
            Dictionary<string, string> Dics = new Dictionary<string, string>();
            //处理获取字段信息

            if (obj == null) { return null; }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(obj, null);
                if (value == null)
                {
                    Dics.Add(name, "");
                }
                else
                {
                    Dics.Add(name, value as string);
                }

            }

            return Dics;
        }

        /// <summary>
        /// 获取方法执行结果
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>默认返回False</returns>
        public static string Result(this Dictionary<string, string> instance)
        {
            string result = "False";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "Result".ToLower())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取方法名称
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ActionName(this Dictionary<string, string> instance)
        {
            string result = "ExecuteServic";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ActionName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取服务名
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ServiceName(this Dictionary<string, string> instance)
        {
            string result = "BLLCore";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ServiceName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取模块名称
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ModelName(this Dictionary<string, string> instance)
        {
            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ModelName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取模板名称
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Template(this Dictionary<string, string> instance)
        {
            string result = "BASETEMPLATE";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "TEMPLATENAME".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Versions(this Dictionary<string, string> instance)
        {
            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "Versions".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ErrorMessage(this Dictionary<string, string> instance)
        {
            string result = "False";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ErrorMessage".ToUpper())
                {
                    result = item.Value as string;
                    break;
                }
            }
            return result;
        }

        #region added by tony 2018-3-9

        /// <summary>
        /// 获取方法执行结果
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>默认返回False</returns>
        public static object Result(this Dictionary<string, object> instance)
        {
            object result = null;

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "Result".ToLower())
                {
                    result = item.Value;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取方法名称
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ActionName(this Dictionary<string, object> instance)
        {
            string result = "ExecuteCamstaroService";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ActionName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        public static Dictionary<string, object> AddActionName(this Dictionary<string, object> instance, string value)
        {
            string key = "ActionName";
            if (!value.CheckStringValue()) value = "ExecuteCamstaroService";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, string> AddActionName(this Dictionary<string, string> instance, string value)
        {
            string key = "ActionName";
            if (!value.CheckStringValue()) value = "DoWork";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        #region  Memo:2018-4-10


        public static string GetXMLTemplate(this Dictionary<string, string> instance)
        {
            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "XMLTemplate".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        public static Dictionary<string, string> ADDXMLTemplate(this Dictionary<string, string> instance, string value = "")
        {
            string key = "XMLTemplate";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value.ToString());
            }
            else
            {
                instance[key] = value.ToString();
            }

            return instance;
        }
        public static Dictionary<string, string> ADDReplacedXML(this Dictionary<string, string> instance, string value = "")
        {
            string key = "REPLACEDXML";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value.ToString());
            }
            else
            {
                instance[key] = value.ToString();
            }

            return instance;
        }
        public static string GetReplacedXML(this Dictionary<string, object> instance)
        {

            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "REPLACEDXML".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        public static string GetReplacedXML(this Dictionary<string, string> instance)
        {

            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "REPLACEDXML".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }

        public static string GetXMLTemplate(this Dictionary<string, object> instance)
        {

            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "XMLTemplate".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        public static Dictionary<string, object> ADDXMLTemplate(this Dictionary<string, object> instance, string value = null)
        {
            string key = "XMLTemplate";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;
        }

        public static Dictionary<string, object> ADDReplacedXML(this Dictionary<string, object> instance, string value = null)
        {
            string key = "REPLACEDXML";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;
        }




        public static bool IsTemplateEnabled(this Dictionary<string, string> instance)
        {
            object obj = 0;
            bool flag = false;
            try
            {
                obj = instance["XMLTemplateEnabled"];
                int i = (int)obj;
                flag = i > 0;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public static Dictionary<string, string> XMLTemplateDisEnabled(this Dictionary<string, string> instance)
        {
            int value = 0;
            string key = "XMLTemplateEnabled";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value.ToString());
            }
            else
            {
                instance[key] = value.ToString();
            }

            return instance;
        }
        public static Dictionary<string, string> XMLTemplateEnabled(this Dictionary<string, string> instance)
        {
            int value = 5;
            string key = "XMLTemplateEnabled";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value.ToString());
            }
            else
            {
                instance[key] = value.ToString();
            }

            return instance;
        }

        public static bool IsTemplateEnabled(this Dictionary<string, object> instance)
        {
            object obj = 0;

            bool flag = false;
            try
            {
                obj = instance["XMLTemplateEnabled"];
                int i = (int)obj;
                flag = i > 0;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public static Dictionary<string, object> XMLTemplateDisEnabled(this Dictionary<string, object> instance)
        {
            int value = 0;
            string key = "XMLTemplateEnabled";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;
        }
        public static Dictionary<string, object> XMLTemplateEnabled(this Dictionary<string, object> instance)
        {
            int value = 5;
            string key = "XMLTemplateEnabled";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;
        }

        public static Dictionary<string, object> AddBLLActionName(this Dictionary<string, object> instance, string value)
        {
            string key = "BLLActionName";
            if (!value.CheckStringValue()) value = "ExecuteServic";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, string> AddBLLActionName(this Dictionary<string, string> instance, string value)
        {
            string key = "BLLActionName";
            if (!value.CheckStringValue()) value = "ExecuteService";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static string BLLActionName(this Dictionary<string, string> instance)
        {
            string result = "ExecuteService";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "BLLActionName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        public static string BLLActionName(this Dictionary<string, object> instance)
        {
            string result = "ExecuteServic";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "BLLActionName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }

        public static Dictionary<string, object> AddBLLName(this Dictionary<string, object> instance, string value)
        {
            string key = "BLLName";
            if (!value.CheckStringValue()) value = "WebBLLCore";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value + "BLL");
            }
            else
            {
                instance[key] = value + "BLL";
            }

            return instance;

        }
        public static Dictionary<string, string> AddBLLName(this Dictionary<string, string> instance, string value)
        {
            string key = "BLLName";
            if (!value.CheckStringValue()) value = "WebBLLCore";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value + "BLL");
            }
            else
            {
                instance[key] = value + "BLL";
            }

            return instance;

        }
        /// <summary>
        /// 获取服务名
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string BLLName(this Dictionary<string, string> instance)
        {
            string result = "WebBLLCoreBLL";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "BLLName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        public static string BLLName(this Dictionary<string, object> instance)
        {
            string result = "WebBLLCoreBLL";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "BLLName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }

        #endregion
        public static Dictionary<string, object> AddServiceName(this Dictionary<string, object> instance, string value)
        {
            string key = "ServiceName";
            if (!value.CheckStringValue()) value = "ServiceBase";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value  );
            }
            else
            {
                instance[key] = value ;
            }

            return instance;

        }
        public static Dictionary<string, string> AddServiceName(this Dictionary<string, string> instance, string value)
        {
            string key = "ServiceName";
            if (!value.CheckStringValue()) value = "ServiceBase";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value  );
            }
            else
            {
                instance[key] = value  ;
            }

            return instance;

        }
        /// <summary>
        /// 获取服务名
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ServiceName(this Dictionary<string, object> instance)
        {
            string result = "BLLCore";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ServiceName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取模块名称
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ModelName(this Dictionary<string, object> instance)
        {
            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ModelName".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取模板名称
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Template(this Dictionary<string, object> instance)
        {
            string result = "BASETEMPLATE";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "TEMPLATENAME".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Versions(this Dictionary<string, object> instance)
        {
            string result = "";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "Versions".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ErrorMessage(this Dictionary<string, object> instance)
        {
            string result = "False";

            foreach (var item in instance)
            {
                if (item.Key.ToString().ToUpper() == "ErrorMessage".ToUpper())
                {
                    string rs = item.Value as string;
                    if (rs.CheckStringValue()) result = item.Value as string;

                    break;
                }
            }
            return result;
        }
        #endregion
        /// <summary>
        /// 错误信息添加
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="errormessage"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ADDErrorKey(this Dictionary<string, string> instance, string errormessage)
        {


            if (instance == null) return null;

            if (!instance.Keys.Contains("ErrorMessage".ToUpper()))
            {
                instance.Add("ErrorMessage".ToUpper(), errormessage);
            }
            else
            {
                instance["ErrorMessage".ToUpper()] = errormessage;
            }

            return instance;


        }
        
        public static Dictionary<string, object> ToObjectDictionary(this Dictionary<string, string> instance)
        {
            if (instance == null) return new Dictionary<string, object>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in instance)
            {
                if (!instance.Keys.Contains(item.Key))
                {
                    dic.Add(item.Key, item.Value);
                }
                else
                {
                    dic[item.Key] = item.Value;
                }
            }
            return dic;
        }
        public static Dictionary<string, string> ToDictionary(this Dictionary<string, object> instance)
        {
            if (instance == null) return new Dictionary<string, string>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in instance)
            {
                if (!instance.Keys.Contains(item.Key))
                {
                    dic.Add(item.Key, item.Value as string);
                }
                else
                {
                    dic[item.Key] = item.Value as string;
                }
            }
            return dic;
        }
        public static Dictionary<string, object> ConDic(this Dictionary<string, object> instance, Dictionary<string, object> appeddic)
        {
            if (instance == null) return appeddic;
            foreach (var item in appeddic)
            {
                if (!instance.Keys.Contains(item.Key))
                {
                    instance.Add(item.Key, item.Value);
                }
                else
                {
                    instance[item.Key] = item.Value;
                }
            }
            return instance;
        }
        public static Dictionary<string, object> ConDic(this Dictionary<string, object> instance, Dictionary<string, string> appeddic)
        {
            if (instance == null) return appeddic.ToObjectDictionary(); ;
            foreach (var item in appeddic)
            {
                if (!instance.Keys.Contains(item.Key))
                {
                    instance.Add(item.Key, item.Value);
                }
                else
                {
                    instance[item.Key] = item.Value;
                }
            }
            return instance;
        }
        public static Dictionary<string, string> ConDic(this Dictionary<string, string> instance, Dictionary<string, object> appeddic)
        {
            if (instance == null) return appeddic.ToDictionary(); ;
            foreach (var item in appeddic)
            {
                if (!instance.Keys.Contains(item.Key))
                {
                    instance.Add(item.Key, item.Value as string);
                }
                else
                {
                    instance[item.Key] = item.Value as string;
                }
            }
            return instance;
        }

        public static Dictionary<string, object> AddKey(this Dictionary<string, object> instance, string key, object value)
        {
            if (instance == null) return null;

            if (!instance.GetValUpperKey(key).CheckStringValue())
            {
                instance.Add(key, value);
            }
            else
            {
                foreach (var itm in instance.Keys)
                {
                    if (itm.ToUpper() == key.ToUpper())
                    {
                        instance[itm] = value;
                    }
                }

            }

            return instance;
        }
        public static Dictionary<string, object> AddKey(this Dictionary<string, object> instance, object key, object value)
        {
            if (instance == null) return null;
            string dicKey = "null";
            if (key as string != null) dicKey = key as string;
            if (!instance.GetValUpperKey(dicKey).CheckStringValue())
            {
                instance.Add(dicKey, value);
            }
            else
            {
                foreach (var itm in instance.Keys)
                {
                    if (itm.ToUpper() == dicKey.ToUpper())
                    {
                        instance[itm] = value;
                    }
                }

            }

            return instance;
        }
        public static Dictionary<string, string> AddKey(this Dictionary<string, string> instance, string key, string value)
        {
            if (instance == null) return null;


            if (!instance.GetValueUpperKey(key).CheckStringValue())
            {
                instance.Add(key, value);
            }
            else
            {
                List<string> list = instance.Keys.ToList<string>();
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].ToUpper() == key.ToUpper())
                    {
                        instance[list[i]] = value;
                    }
                }


            }

            return instance;
        }
        public static Dictionary<string, string> AddKey(this Dictionary<string, string> instance, string key, object value)
        {
            if (instance == null) return null;
            string dicvalue = "";
            if (value as string != null) dicvalue = value as string;
            if (!instance.GetValueUpperKey(key).CheckStringValue())
            {
                instance.Add(key, dicvalue);
            }
            else
            {
                foreach (var itm in instance.Keys)
                {
                    if (itm.ToUpper() == key.ToUpper())
                    {
                        instance[itm] = dicvalue;
                    }
                }

            }

            return instance;
        }
        public static Dictionary<string, string> AddKey(this Dictionary<string, string> instance, object key, object value)
        {
            if (instance == null) return null;
            string dicvalue = "";
            string dicKey = "null";
            if (key as string != null) dicKey = key as string;
            if (value as string != null) dicvalue = value as string;
            if (!instance.GetValueUpperKey(dicKey).CheckStringValue())
            {
                instance.Add(dicKey, dicvalue);
            }
            else
            {
                foreach (var itm in instance.Keys)
                {
                    if (itm.ToUpper() == dicKey.ToUpper())
                    {
                        instance[itm] = dicvalue;
                    }
                }

            }

            return instance;
        }
        
        /// <summary>
        /// 把Dictionary转化为任何对象（对象属性中必需包含有Dictionary中的KEY值属性）
        /// </summary>
        /// <typeparam name="T">类型对象参数</typeparam>
        /// <param name="instance">待扩展的对象</param>
        /// <returns></returns>
        public static T Dictionary2Object<T>(this Dictionary<string, string> instance)
        {

            Type t = typeof(T);
            object obj = Activator.CreateInstance(t, null);

            var lists = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in lists)
            {

                object oob = instance.GetValueUpperKey(item.Name);

                if (item.Name != null) try { item.SetValue(obj, oob, null); } catch { }
            }
            return (T)obj;
        }
        
        public static object GetValueUpperKey(this Dictionary<string, object> dict, string key)
        {
            object result = null;
            foreach (var itm in dict.Keys)
            {
                if (itm.ToUpper() == key.ToUpper())
                {
                    result = dict[itm];
                }
            }
            return result;
        }
        public static string GetValUpperKey(this Dictionary<string, object> dict, string key)
        {
            object result = "";
            foreach (var itm in dict.Keys)
            {
                if (itm.ToUpper() == key.ToUpper())
                {
                    result = dict[itm];
                }
            }
            return result as string;
        }
        public static string GetStringValueUpperKey(this Dictionary<string, object> dict, string key)
        {
            object result = new object();
            foreach (var itm in dict.Keys)
            {
                if (itm.ToUpper() == key.ToUpper())
                {
                    result = dict[itm];
                }
            }
            if (result as string != null)
            {
                return result as string;
            }
            else
            {
                return "";
            }
        }
        public static string GetValueUpperKey(this Dictionary<string, string> dict, string key)
        {
            string result = "";
            foreach (var itm in dict.Keys)
            {
                if (itm.ToUpper() == key.ToUpper())
                {
                    result = dict[itm];
                }
            }
            return result;
        }

        public static Dictionary<string, string> AddTemplate(this Dictionary<string, string> dict, string value)
        {

            dict.AddKey("templatename", value);

            return dict;
        }
        public static Dictionary<string, object> AddTemplate(this Dictionary<string, object> dict, string value)
        {

            dict.AddKey("templatename", value);

            return dict;
        }
        public static string GetTemplate(this Dictionary<string, string> dict)
        {
            return dict.GetValueUpperKey("templatename");
        }
        public static Dictionary<string, object> AddAppName(this Dictionary<string, object> instance, string value)
        {
            string key = "AppName";
            if (!value.CheckStringValue()) value = "AppName";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, string> AddAppName(this Dictionary<string, string> instance, string value)
        {
            string key = "AppName";
            if (!value.CheckStringValue()) value = "AppName";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, object> AddCName(this Dictionary<string, object> instance, string value)
        {
            string key = "CName";
            if (!value.CheckStringValue()) value = "CName";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, string> AddCName(this Dictionary<string, string> instance, string value)
        {
            string key = "CName";
            if (!value.CheckStringValue()) value = "CName";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, object> AddMethodName(this Dictionary<string, object> instance, string value)
        {
            string key = "MethodName";
            if (!value.CheckStringValue()) value = "call";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, string> AddMethodName(this Dictionary<string, string> instance, string value)
        {
            string key = "MethodName";
            if (!value.CheckStringValue()) value = "call";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }

        public static string AppName(this Dictionary<string, string> dict)
        {



            return dict.GetValueUpperKey("AppName");
        }
        public static string CName(this Dictionary<string, string> dict)
        {



            return dict.GetValueUpperKey("cname");
        }
        public static string MethodName(this Dictionary<string, string> dict)
        {



            return dict.GetValueUpperKey("MethodName");
        }

        public static object AppName(this Dictionary<string, object> dict)
        {



            return dict.GetValueUpperKey("AppName");
        }
        public static object CName(this Dictionary<string, object> dict)
        {



            return dict.GetValueUpperKey("cname");
        }
        public static object MethodName(this Dictionary<string, object> dict)
        {



            return dict.GetValueUpperKey("MethodName");
        }

        public static Dictionary<string, object> AddResult(this Dictionary<string, object> instance, string value)
        {
            string key = "result";
            if (!value.CheckStringValue()) value = "result";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
        public static Dictionary<string, string> AddResult(this Dictionary<string, string> instance, string value)
        {
            string key = "result";
            if (!value.CheckStringValue()) value = "result";
            if (instance == null) return null;

            if (!instance.Keys.Contains(key))
            {
                instance.Add(key, value);
            }
            else
            {
                instance[key] = value;
            }

            return instance;

        }
    }
}

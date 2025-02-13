using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections;

namespace System
{
    public static class SystemobjectExtentions
    {  /// <summary>
       /// 判断DataTable是否为有效表格
       /// </summary>
       /// <param name="dt"></param>
       /// <returns></returns>
        public static bool IsValidated(this DataTable dt)
        {
            try
            {
                if (dt as DataTable == null) return false;
            }
            catch (Exception ex)
            {
              
                return false;
            }
            return dt.GetStringValue().CheckStringValue();
        }
        #region 通用对象扩展方法
        /// <summary>
        /// 获取对象中字段信息
        /// </summary>
        /// <param name="obj">待扩展的对象</param>
        /// <returns></returns>
        public static List<string> GetFields(this object obj)
        {
            List<string> lists = new List<string>();
            //处理获取字段信息

            if (obj == null)
            {
                return null;
            }

            var tobj = obj.GetType();

            try
            {
                FieldInfo[] fields = tobj.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                foreach (var item in fields)
                {
                    lists.Add(item.Name);
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }


            return lists;
        }

        public static IEnumerator GetEnumerator(this object obj)
        { //处理获取字段信息
            if (obj == null)
            {
                yield return null;
            }

            var tobj = obj.GetType();


            PropertyInfo[] Proinfos = tobj.GetProperties();
            foreach (var item in Proinfos)
            {
                yield return item.Name;
            }

        }
        /// <summary>
        /// 获取类对象的属性信息
        /// </summary>
        /// <param name="obj">待扩展的对象</param>
        /// <returns></returns>
        public static List<string> GetProperties(this object obj)
        {
            List<string> lists = new List<string>();
            //处理获取字段信息
            if (obj == null)
            {
                return null;
            }

            var tobj = obj.GetType();

            try
            {
                PropertyInfo[] Proinfos = tobj.GetProperties();
                foreach (var item in Proinfos)
                {
                    lists.Add(item.Name);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return lists;
        }

        /// <summary>
        /// 把对象中的属性值及属性转换为SQL语句字符串
        /// </summary>
        /// <param name="obj">待扩展的对象</param>
        /// <param name="SQLRegstr">SQL模板</param>
        /// <returns></returns>
        public static string ToSQL(this object obj, string SQLRegstr = null)
        {
            //处理获取字段信息
            var lists = obj.ToDictionary();
            foreach (var item in lists)
            {
                SQLRegstr = SQLRegstr.SetParmValue(item.Key, item.Value);

            }
            return SQLRegstr;
        }
        /// <summary>
        /// 把对象中的属性值及属性转换为XML字符串
        /// </summary>
        /// <param name="obj">待扩展的对象</param>
        /// <param name="XMLRegstr">模板文件</param>
        /// <returns></returns>
        public static string ToXML(this object obj, string XMLRegstr = null)
        {
            string result = "";
            string xmlreg = "";
            var lists = obj.ToDictionary();
            if (XMLRegstr == null)
            {
                xmlreg = "<{KEY}>{VALUE}</{KEY}>";

            }
            foreach (var item in lists)
            {
                if (XMLRegstr == null)
                {
                    result = result + xmlreg.Replace("{KEY}", item.Key).Replace("{VALUE}", item.Value);
                }
                else
                {
                    result = result + XMLRegstr.SetParmValue(item.Key, item.Value);

                }
            }

            return result;
        }
        /// <summary>
        /// 字符串转换XML，返回字符串本身
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="XMLRegstr"></param>
        /// <returns></returns>
        public static string ToXML(this string obj)
        {
            return obj;
        }
        #endregion

        /// <summary>
        /// DataTable转换为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">待扩展DataTable</param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(this DataTable dt)
        {
            List<T> lists = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }

                lists.Add(s);
            }

            return lists;
        }
        public static List<string> DataTableToList(this DataTable dt, string columnname = "0")
        {
            if (dt as DataTable == null) return new List<string>();
            List<string> lists = new List<string>();
            if (columnname.CheckStringValue())
            {

                foreach (DataRow item in dt.Rows)
                {


                    if (!Convert.IsDBNull(item[columnname]))
                    {
                        lists.Add(item[columnname].ToString());
                    }


                }
            }
            else
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (!Convert.IsDBNull(item[0]))
                    {
                        lists.Add(item[0].ToString());
                    }


                }
            }
            return lists;
        }
        public static string ToSQL<T>(this T model, DataTable dt)
        {
            string str = model.ToString();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Rows[0][i] != null)
                {
                    if (dt.Rows[0][i] != null)
                    {
                        str = str.Replace("{" + dt.Columns[i].ColumnName.ToUpper() + "}", dt.Rows[0][i].ToString());
                    }
                    else
                    {
                        str = str.Replace("{" + dt.Columns[i].ColumnName.ToUpper() + "}", "");
                    }
                }


            }

            return str;
        }

        public static string ToCamstarXml(this string model, DataTable dt)
        {
            string str = model.ToString();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Rows[0][i] != null)
                {
                    if (dt.Rows[0][i] != null)
                    {
                        str = str.Replace("{" + dt.Columns[i].ColumnName.ToUpper() + "}", dt.Rows[0][i].ToString());
                    }
                    else
                    {
                        str = str.Replace("{" + dt.Columns[i].ColumnName.ToUpper() + "}", "");
                    }
                }


            }

            return str;
        }
        /// <summary>
        /// 循环替换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToCamstarXml<T>(this T model, Dictionary<string, string> dic) where T : class
        {
            string str = model.ToString();
            foreach (var item in dic)
            {
                if (item.Key != null)
                {
                    if (item.Value != null)
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", item.Value);
                    }
                    else
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", "");
                    }
                }
            }
            return str;
        }
        public static string ToCamstarXml<T>(this T model, Dictionary<string, object> dic) where T : class
        {
            string str = model.ToString();
            foreach (var item in dic)
            {
                if (item.Key != null)
                {
                    if (item.Value != null)
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", item.Value.ToString());
                    }
                    else
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", "");
                    }
                }
            }
            return str;
        }
        public static string ToCamstarXml<T>(this string model, object obj)
        {
            var dic = obj.ToDictionary();
            string str = model.ToString();
            foreach (var item in dic)
            {
                if (item.Key != null)
                {
                    if (item.Value != null)
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", item.Value);
                    }
                    else
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", "");
                    }
                }
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToSQL<T>(this T model, Dictionary<string, string> dic) where T : class
        {
            string str = model.ToString();
            foreach (var item in dic)
            {
                if (item.Key != null)
                {
                    if (item.Value != null)
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", item.Value);
                    }
                    else
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", "");
                    }
                }
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToSQL<T>(this T model, Dictionary<string, object> dic)where T:class
        {
            string str = model.ToString();
            foreach (var item in dic)
            {
                if (item.Key != null)
                {
                    if (item.Value != null)
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", item.Value.ToString());
                    }
                    else
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", "");
                    }
                }
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSQL(this string model, object obj)
        {
            var dic = obj.ToDictionary();
            string str = model.ToString();
            foreach (var item in dic)
            {
                if (item.Key != null)
                {
                    if (item.Value != null)
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", item.Value.ToString());
                    }
                    else
                    {
                        str = str.Replace("{" + item.Key.ToUpper() + "}", "");
                    }
                }
            }
            return str;
        }
        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<TResult> ToLists<TResult>(this DataTable dt) where TResult : class, new()
        {
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            Type t = typeof(TResult);
            PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<TResult> oblist = new List<TResult>();
            if (dt == null) return oblist;
            foreach (DataRow row in dt.Rows)
            {
                object obj = Activator.CreateInstance(t, null);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    foreach (PropertyInfo p in props)
                    {
                        if (dt.Columns[i].ToString().ToUpper() == p.Name.ToUpper())
                        {
                            if (row[dt.Columns[i]] != DBNull.Value)
                            {
                                string strtypename = p.PropertyType.Name;
                                if (strtypename.ToUpper() == "Int32".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, Int32.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex) { }
                                }
                                else if (strtypename.ToUpper() == "Decimal".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, decimal.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex) { }
                                }
                                else if (strtypename.ToUpper() == "Double".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, Double.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex) { }
                                }
                                else if (strtypename.ToUpper() == "DateTime".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, DateTime.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex) { }
                                }
                                else if (strtypename.ToUpper() == "float".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, float.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex) { }
                                }
                                else
                                {
                                    p.SetValue(obj, row[dt.Columns[i]].ToString(), null);
                                }
                            }
                        }
                    }
                }
                oblist.Add((TResult)obj);
            }
            return oblist;
        }

        public static Dictionary<string, string> DataTableToKeyValue(this DataTable dt, string templatestr = "")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string listTemplate = templatestr;
            string valuestr = "";
            string KEY = "LISTITEMS";// dt.TableName;
            if (dt == null)
            {
                KEY = "LISTITEMS";
                dic.Add(KEY, "");
                return dic;
            }
            KEY = dt.TableName;
            if (KEY.ToString().Trim().Length < 1) KEY = "LISTITEMS";
            if (dt.Rows.Count == 0)
            {

                dic.Add(KEY, "");
                return dic;
            }

            var flag = true;
            if (listTemplate.ToString().Trim().Length < 1) flag = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string rowresult = listTemplate;

                if (flag)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        rowresult.SetParmValue(dt.Columns[j].ToString(), dt.Rows[i][j].ToString());
                    }
                }
                else
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            rowresult = rowresult + "<" + dt.Columns[j].ToString() + ">" + dt.Rows[i][j].ToString() + "</" + dt.Columns[j].ToString() + ">";// (dt.Columns[j].ToString(), dt.Rows[i][j].ToString());
                        }
                        else
                        {
                            rowresult = rowresult + "<" + dt.Columns[j].ToString() + "></" + dt.Columns[j].ToString() + ">";// (dt.Columns[j].ToString(), dt.Rows[i][j].ToString());

                        }
                    }
                }
                valuestr = valuestr + rowresult;
            }
            dic.Add(KEY, valuestr);
            return dic;
        }
        /// <summary>
        /// 行转列
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ConvertToTable(this DataTable source)
        {
            DataTable dt = new DataTable();
            //前两列是固定的加上
            dt.Columns.Add("staff_id");
            dt.Columns.Add("staff_Name");
            //以staff_TiCheng 字段为筛选条件  列转为行  下面有图
            var columns = (from x in source.Rows.Cast<DataRow>() select x[2].ToString()).Distinct();
            //把 staff_TiCheng 字段 做为新字段添加进去
            foreach (var item in columns) dt.Columns.Add(item).DefaultValue = 0;
            //   x[1] 是字段 staff_Name 按  staff_Name分组 g 是分组后的信息   g.Key 就是名字  如果不懂就去查一个linq group子句进行分组
            var data = from x in source.Rows.Cast<DataRow>()
                       group x by x[1] into g
                       select new { Key = g.Key.ToString(), Items = g };
            data.ToList().ForEach(x =>
            {
                //这里用的是一个string 数组 也可以用DataRow根据个人需要用
                string[] array = new string[dt.Columns.Count];
                //array[1]就是存名字的
                array[1] = x.Key;
                //从第二列开始遍历
                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    // array[0]  就是 staff_id
                    if (array[0] == null)
                        array[0] = x.Items.ToList<DataRow>()[0]["staff_id"].ToString();
                    //array[0] = (from y in x.Items
                    //            where y[2].ToString() == dt.Columns[i].ToString()
                    //            select y[0].ToString()).SingleOrDefault();
                    //array[i]就是 各种提成
                    array[i] = (from y in x.Items
                                where y[2].ToString() == dt.Columns[i].ToString()//   y[2] 各种提成名字等于table中列的名字
                                select y[3].ToString()                            //  y[3] 就是我们要找的  staff_TiChengAmount 各种提成 的钱数
                               ).SingleOrDefault();
                }
                dt.Rows.Add(array);   //添加到table中
            });
            return dt;
        }
        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            return list.ToDataTable<T>();
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(this IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
            }
            else
            {
                propertyNameList = propertyName[0].GetProperties();

            }

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }
}

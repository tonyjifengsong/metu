using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class SysObjectExtentions
    {  /// <summary>
       /// 判断字符串是否不为Null、空
       /// </summary>
       /// <param name="s"></param>
       /// <returns></returns>
        public static bool NotNull(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 默认格式：4575c4b3-7997-4f11-acd9-f107258e9adc
        /// N格式：a53a7186b583483aa4580519034e8095
        /// D格式：5ae7f002-a989-4345-864b-3bcfbe09e1da
        /// B格式：{d9762660-8461-4c44-b714-8ffad6e1b79c}
        /// P格式：(694ce704-0a7d-41d5-a25a-4eaedf7db50d)
        ///X格式：{0x75198f26,0xac4e,0x42c8,{0x96,0x88,0xcc,0x91,0xe0,0xa6,0x9b,0x21}
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetNewGuId(this object obj, string format = "")
        {
            if (string.IsNullOrWhiteSpace(format))
                return Guid.NewGuid().ToString();
            else
                return Guid.NewGuid().ToString(format);
        }
        /// <summary>
        /// 将十进制转换为指定的进制
        /// </summary>
        /// <param name="Val">十进制值</param>
        /// <param name="TargetRadix">目标进制</param>
        /// <param name="BaseChar">基数列表(长度必须大于源字符串进制,无I，O)</param>
        /// <returns></returns>
        public static string JinZhiConvert(this string str, int TargetRadix = 0, string BaseChar = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ")
        {
            if (str == null) return "";
            ulong Val = 0;
            try
            {
                Val = ulong.Parse(str.ToString());
            }
            catch
            {
                return "";
            }
            if (TargetRadix == 0 && BaseChar.Length == 0) return "长度与基数不能同时为零";
            if (TargetRadix <= 0) TargetRadix = BaseChar.Length;
            List<string> r = new List<string>();
            do
            {
                ulong y = Val % (ulong)TargetRadix;
                r.Add(BaseChar[Convert.ToInt32(y)].ToString());
                Val = Convert.ToUInt64(Math.Floor(Val / (decimal)TargetRadix));
            } while (Val > 0);
            r.Reverse();
            return string.Join("", r.ToArray());
        }


        /// <summary>
        /// 将任意进制转化为十制
        /// </summary>
        /// <param name="Val">任意进制的字任串</param>
        /// <param name="SourceRadix">源字符串的进制</param>
        /// <param name="BaseChar">基数列表(长度必须大于源字符串进制,无I，O)</param>
        /// <returns></returns>
        public static ulong ConvertTen(this string Val, int SourceRadix = 0, string BaseChar = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ")
        {
            ulong r = 0;
            if (BaseChar == "") return 0;
            if (SourceRadix == 0) SourceRadix = BaseChar.Length;
            List<char> v = Val.ToCharArray().ToList();
            v.Reverse();
            for (int i = 0; i < v.Count; i++)
            {
                int index = BaseChar.IndexOf(v[i]);
                if (index > -1)
                    r += Convert.ToUInt64(index * Math.Pow(SourceRadix, i));
            }
            return r;
        }
        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
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
                                    catch (Exception ex)
                                    {
                                        //FileHelpers.Writelog(ex);
                                    }
                                }
                                else if (strtypename.ToUpper() == "Decimal".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, decimal.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex)
                                    {
                                        //FileHelpers.Writelog(ex);
                                    }
                                }
                                else if (strtypename.ToUpper() == "Double".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, Double.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex)
                                    {
                                        //FileHelpers.Writelog(ex);
                                    }
                                }
                                else if (strtypename.ToUpper() == "DateTime".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, DateTime.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex)
                                    {
                                        //FileHelpers.Writelog(ex);
                                    }
                                }
                                else if (strtypename.ToUpper() == "float".ToUpper())
                                {
                                    try
                                    {
                                        p.SetValue(obj, float.Parse(row[dt.Columns[i]].ToString()), null);
                                    }
                                    catch (Exception ex)
                                    {
                                        //FileHelpers.Writelog(ex);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        p.SetValue(obj, row[dt.Columns[i]], null);
                                    }
                                    catch (Exception ex)
                                    {
                                        //FileHelpers.Writelog(ex);
                                    }

                                }

                            }
                        }
                    }
                }
                oblist.Add((TResult)obj);
            }
            return oblist;
        }
        /// <summary>
        /// 转换为一个DataTable
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult : class
        {
            List<PropertyInfo> pList = new List<PropertyInfo>();
            Type type = typeof(TResult);
            DataTable dt = new DataTable();
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in value)
            {
                DataRow row = dt.NewRow();
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                dt.Rows.Add(row);
            }
            return dt;
        }
        /// <summary>
        /// DataTable通过反射获取单个像
        /// </summary>
        public static T ToSingleModel<T>(this DataTable data) where T : new()
        {
            T t = data.GetList<T>(null, true).Single();
            return t;
        }


        /// <summary>
        /// DataTable通过反射获取单个像
        /// <param name="prefix">前缀</param>
        /// <param name="ignoreCase">是否忽略大小写，默认不区分</param>
        /// </summary>
        public static T ToSingleModel<T>(this DataTable data, string prefix, bool ignoreCase = true) where T : new()
        {
            T t = data.GetList<T>(prefix, ignoreCase).Single();
            return t;
        }

        /// <summary>
        /// DataTable通过反射获取多个对像
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> ToListModel<T>(this DataTable data) where T : new()
        {
            List<T> t = data.GetList<T>(null, true);
            return t;
        }


        /// <summary>
        /// DataTable通过反射获取多个对像
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="ignoreCase">是否忽略大小写，默认不区分</param>
        /// <returns></returns>
        private static List<T> ToListModel<T>(this DataTable data, string prefix, bool ignoreCase = true) where T : new()
        {
            List<T> t = data.GetList<T>(prefix, ignoreCase);
            return t;
        }



        private static List<T> GetList<T>(this DataTable data, string prefix, bool ignoreCase = true) where T : new()
        {
            List<T> t = new List<T>();
            int columnscount = data.Columns.Count;
            if (ignoreCase)
            {
                for (int i = 0; i < columnscount; i++)
                    data.Columns[i].ColumnName = data.Columns[i].ColumnName.ToUpper();
            }
            try
            {
                var properties = new T().GetType().GetProperties();

                var rowscount = data.Rows.Count;
                for (int i = 0; i < rowscount; i++)
                {
                    var model = new T();
                    foreach (var p in properties)
                    {
                        var keyName = prefix + p.Name + "";
                        if (ignoreCase)
                            keyName = keyName.ToUpper();
                        for (int j = 0; j < columnscount; j++)
                        {
                            if (data.Columns[j].ColumnName == keyName && data.Rows[i][j] != null)
                            {
                                string pval = data.Rows[i][j].ToString();
                                if (!string.IsNullOrEmpty(pval))
                                {
                                    try
                                    {
                                        // We need to check whether the property is NULLABLE
                                        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                        {
                                            try
                                            {
                                                p.SetValue(model, Convert.ChangeType(data.Rows[i][j], p.PropertyType.GetGenericArguments()[0]), null);
                                            }
                                            catch (Exception ex)
                                            {
                                                //FileHelpers.Writelog(ex);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                p.SetValue(model, Convert.ChangeType(data.Rows[i][j], p.PropertyType), null);
                                            }
                                            catch (Exception ex)
                                            {
                                                //FileHelpers.Writelog(ex);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //FileHelpers.Writelog(ex);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    t.Add(model);
                }
            }
            catch (Exception ex)
            {
                //FileHelpers.Writelog(ex);
            }


            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DtToList<T>(this DataTable dt) where T : class, new()
        {
            // 定义集合  
            var list = new List<T>();

            if (0 == dt.Rows.Count)
            {
                return list;
            }

            // 获得此模型的可写公共属性  
            IEnumerable<PropertyInfo> propertys = new T().GetType().GetProperties().Where(u => u.CanWrite);
            list = dt.ToListEntity<T>(propertys);


            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T DtToEntityFirst<T>(this DataTable dt) where T : class, new()
        {
            DataTable dtTable = dt.Clone();
            dtTable.Rows.Add(dt.Rows[0].ItemArray);
            return dtTable.DtToList<T>()[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="propertys"></param>
        /// <returns></returns>
        public static List<T> ToListEntity<T>(this DataTable dt, IEnumerable<PropertyInfo> propertys) where T : class, new()
        {
            var list = new List<T>();
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new T();

                //遍历该对象的所有属性  
                foreach (PropertyInfo p in propertys)
                {
                    //将属性名称赋值给临时变量
                    string tmpName = p.Name;

                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (!dt.Columns.Contains(tmpName)) continue;
                    //取值  
                    object value = dr[tmpName];
                    //如果非空，则赋给对象的属性  
                    if (value != DBNull.Value)
                    {
                        if (p.GetType().FullName.ToLower().IndexOf("ool") > 0)
                        {
                            if (value.ToString().ToLower() == "true")
                            {
                                p.SetValue(entity, true, null);

                            }
                            else
                            {
                                p.SetValue(entity, false, null);
                            }
                        }
                        else
                        {
                            try
                            {
                                if (value is null) continue;

                                p.SetValue(entity, value, null);
                            }
                            catch (Exception ex)
                            {
                                //FileHelpers.Writelog(ex);
                            }
                        }
                    }
                }
                //对象添加到泛型集合中  
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this object list) where T : class, new()
        {
            List<T> lst = new List<T>();

            lst = (List<T>)list;

            return lst;
        }

        /// <summary>
        /// Json字符串反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string str) where T : class
        {
            if (str == null) return null;
            if (str.ToString().Trim().Length <7) return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception ex)
            {
                //FileHelpers.Writelog(ex);

                return default(T);
            }
        }
        /// <summary>
        /// 对象转换为ＪＳＯＮ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string toJson(this object obj)
        {
            if (obj == null) return "";

            try
            {
                if (obj is string) return obj.ToString();
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                //FileHelpers.Writelog(ex);

                return null;
            }
        }

        /// <summary>
        /// 对象转换为实体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T toEntity<T>(this object obj) where T : class, new()
        {
            T result = default(T);
            if (obj == null) return result;

            try
            {
                if (obj is string) return obj.ToString().JsonToObject<T>();
                return JsonConvert.SerializeObject(obj).JsonToObject<T>();
            }
            catch (Exception ex)
            {
                //FileHelpers.Writelog(ex);

                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="copyto"></param>
        /// <param name="iscopynull"></param>
        /// <returns></returns>
        public static T CopyTo<T>(this object obj, T copyto, bool iscopynull = false) where T : class, new()
        {
            if (obj == null) return new T();
            Dictionary<string, object> dic = obj.ToODictionary();

            dic.Dictionary2Object<T>(copyto, iscopynull);
            return copyto;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="copyto"></param>
        /// <param name="iscopynull"></param>
        /// <returns></returns>
        public static List<T> ListCopyTo<T>(this List<object> obj, List<T> copyto, bool iscopynull = false) where T : class, new()
        {
            if (obj == null) return new List<T>();
            foreach (var itm in obj)
            {
                T md = new T();
                itm.CopyTo(md);
                copyto.Add(md);
            }


            return copyto;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="appeddic"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ConDic(this Dictionary<string, string> instance, Dictionary<string, Object> appeddic)
        {
            if (instance == null) instance = new Dictionary<string, string>();
            if (appeddic != null)
            {
                foreach (var item in appeddic)
                {
                    if (!instance.Keys.Contains(item.Key))
                    {
                        instance.Add(item.Key, item.Value.ToString());
                    }
                    else
                    {
                        instance[item.Key] = item.Value.ToString();
                    }
                }
            }
            return instance;
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
        public static Dictionary<string, string> JObject2Dictionary(this JObject obj)
        {
            if (obj == null) return new Dictionary<string, string>();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(obj.toJson());

        }/// <summary>
         /// 
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="obj"></param>
         /// <returns></returns>
        public static List<T> List2Entity<T>(this List<object> obj) where T : class, new()
        {
            if (obj == null) return new List<T>();
            List<T> list = new List<T>();
            foreach (var itm in obj)
            {
                T md = new T();
                itm.CopyTo(md);
                list.Add(md);

            }

            return list;

        }
        /// <summary>
        /// /
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="parantid"></param>
        /// <returns></returns>
        public static List<T> ToTree<T>(this List<T> obj, string parantid = null) where T : class, ITree<T>, new()
        {
            if (obj == null) return new List<T>();
            var tree = obj.Where(e => e.parentid == parantid).ToList();
            tree.ForEach(menu =>
            {
                SetChildren(menu, obj);
            });
            return tree;

        }
        /// <summary>
        /// 子菜单列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="all"></param>
        static void SetChildren<T>(T parent, List<T> all) where T : class, ITree<T>, new()
        {

            parent.sublist = all.Where(e => e.parentid == parent.id).ToList();
            if (parent.sublist.Any())
            {
                parent.sublist.ForEach(menu =>
                {
                    SetChildren(menu, all);
                });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="iscopynull"></param>
        /// <returns></returns>
        public static T JObject2Entity<T>(this JObject obj, bool iscopynull = false) where T : class, new()
        {
            if (obj == null) return new T();
            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(obj.toJson());
            T rs = (T)dic.Dictionary2Object<T>();
            return rs;

        }/// <summary>
         /// 
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            if (obj == null) return new Dictionary<string, string>();
            if (obj is Dictionary<string, string>)
            {
                return (Dictionary<string, string>)obj;
            }
            Dictionary<string, string> Dics = new Dictionary<string, string>();
            if (obj is Dictionary<string, object>)
            {
                foreach (var itm in (Dictionary<string, object>)obj)
                {
                    try
                    {
                        Dics.Add(itm.Key, itm.Value.ToString());
                    }
                    catch
                    {
                        Dics.Add(itm.Key, null);
                    }
                }
                return Dics;
            }

            //处理获取字段信息

            if (obj == null) { return new Dictionary<string, string>(); }
            if (obj as string != null)
            {
                Dics.Add("JSON", obj as string);
                return Dics;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            if (properties.Length <= 0) { return new Dictionary<string, string>(); }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                try
                {
                    object value = item.GetValue(obj);
                    if (value == null) value = "";
                    Dics.Add(name, value.ToString());
                }
                catch
                {
                    Dics.Add(name, null);
                }

            }

            return Dics;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static Dictionary<string, object> ToODictionary(this object obj)
        {
            if (obj is Dictionary<string, object>)
            {
                return (Dictionary<string, object>)obj;
            }
            Dictionary<string, object> Dics = new Dictionary<string, object>();
            if (obj is Dictionary<string, string>)
            {
                foreach (var itm in (Dictionary<string, string>)obj)
                {
                    try
                    {
                        Dics.Add(itm.Key, itm.Value);
                    }
                    catch
                    {
                        Dics.Add(itm.Key, null);
                    }
                }
                return Dics;
            }

            //处理获取字段信息

            if (obj == null) { return new Dictionary<string, object>(); }
            if (obj as string != null)
            {
                Dics.Add("JSON", obj as string);
                return Dics;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            if (properties.Length <= 0) { return new Dictionary<string, object>(); }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(obj);
                if (value != null)
                    Dics.Add(name, value);
            }

            return Dics;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<Condition> ToQueryConditions(this object obj)
        {
            if (obj == null) return new List<Condition>();
            List<Condition> Dics = new List<Condition>();
            if (obj is Dictionary<string, object>)
            {
                foreach (var itm in obj as Dictionary<string, object>)
                {
                    string name = itm.Key.ToLower();
                    object value = itm.Value;
                    string[] arrnames = name.Split("_");
                    if (arrnames.Length > 1 && itm.Key.ToLower() != "domain_id")
                    {
                        Condition md = new Condition();

                        if (arrnames[0] == "pageindex" || arrnames[0] == "pagesize") continue;
                        md.Key = arrnames[0];
                        md.Value = value;
                        if (arrnames[1].ToUpper() == "EQ")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.Equal;
                        }
                        if (arrnames[1].ToUpper() == "NEQ")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.NotEqual;
                        }
                        if (arrnames[1].ToUpper() == "LT")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.Less;
                        }
                        if (arrnames[1].ToUpper() == "LET")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.LessEqual;
                        }
                        if (arrnames[1].ToUpper() == "GT")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.Greater;
                        }
                        if (arrnames[1].ToUpper() == "GTE")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.GreaterEqual;
                        }
                        if (arrnames[1].ToUpper() == "IN")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.In;
                        }
                        if (arrnames[1].ToUpper() == "BT")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.Between;
                        }
                        if (arrnames[1].ToUpper() == "CS")
                        {
                            md.QuerySymbol = ConditionSymbolEnum.Contains;
                        }
                        md.Order = OrderEnum.None;
                        Dics.Add(md);
                    }
                    else
                    {
                        Condition md = new Condition();
                        md.Key = name.ToLower();
                        md.Value = value;
                        md.QuerySymbol = ConditionSymbolEnum.Equal;
                        md.Order = OrderEnum.None;
                        Dics.Add(md);
                    }
                }
                return Dics;
            }

            //处理获取字段信息

            if (obj == null) { return new List<Condition>(); }

            PropertyInfo[] properties = obj.GetType().GetProperties();
            if (properties.Length <= 0) { return new List<Condition>(); }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name.ToLower();
                object value = item.GetValue(obj);
                string[] arrnames = name.Split("_");
                if (arrnames.Length > 1 && name.ToLower() != "domain_id")
                {
                    Condition md = new Condition();

                    if (arrnames[0] == "pageindex" || arrnames[0] == "pagesize") continue;
                    md.Key = arrnames[0];
                    md.Value = value;
                    if (arrnames[1].ToUpper() == "EQ")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.Equal;
                    }
                    if (arrnames[1].ToUpper() == "NEQ")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.NotEqual;
                    }
                    if (arrnames[1].ToUpper() == "LT")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.Less;
                    }
                    if (arrnames[1].ToUpper() == "LET")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.LessEqual;
                    }
                    if (arrnames[1].ToUpper() == "GT")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.Greater;
                    }
                    if (arrnames[1].ToUpper() == "GTE")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.GreaterEqual;
                    }
                    if (arrnames[1].ToUpper() == "IN")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.In;
                    }
                    if (arrnames[1].ToUpper() == "BT")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.Between;
                    }
                    if (arrnames[1].ToUpper() == "CS")
                    {
                        md.QuerySymbol = ConditionSymbolEnum.Contains;
                    }
                    md.Order = OrderEnum.None;
                    Dics.Add(md);
                }
                else
                {
                    if (name == "pageindex" || name == "pagesize") continue;

                    Condition md = new Condition();
                    md.Key = name;
                    md.Value = value;
                    md.QuerySymbol = ConditionSymbolEnum.Equal;
                    md.Order = OrderEnum.None;
                    Dics.Add(md);
                }

            }

            return Dics;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(this Dictionary<string, string> dict, string key)
        {
            if (dict == null) return "";
            string result = "";
            foreach (var itm in dict.Keys)
            {
                if (itm == key)
                {
                    result = dict[itm];
                }
            }
            return result;
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="dict"></param>
         /// <param name="key"></param>
         /// <returns></returns>
        static object GetValueByKey(this Dictionary<string, object> dict, string key)
        {
            object result = null;
            foreach (var itm in dict.Keys)
            {
                if (itm == key)
                {
                    result = dict[itm];
                }
            }
            return result;
        }
        /// <summary>
        /// SQL规则字符串转SQL语句
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Str2SQL(this string key)
        {
            if (key == null) return "";
            if (key.Trim().ToString().Length < 10) return "";
            if (key.IndexOf("[[") <= 0) return key;
            string[] arr = key.Split("[[");
            string sqlstr = "";
            int i = 0;
            foreach (var itm in arr)
            {
                if (i != 0)
                {
                    if (itm.IndexOf("}") > 0)
                    {
                        sqlstr += " " + itm.Split("]]")[1];
                    }
                    else
                    {

                        sqlstr += " " + itm.Replace("]]", "");

                    }
                }
                else
                {
                    sqlstr += " " + itm;
                }

                i++;
            }
            return sqlstr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="Tobj"></param>
        /// <param name="iscopynull"></param>
        /// <returns></returns>
        public static T Dictionary2Object<T>(this Dictionary<string, string> instance, T Tobj = null, bool iscopynull = false) where T : class, new()
        {
            object obj = new object();
            Type t = typeof(T);
            if (Tobj == null)
            {
                obj = Activator.CreateInstance(t, null);
            }
            else
            {
                obj = Tobj;
            }


            var lists = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in lists)
            {

                object oob = instance.GetValueByKey(item.Name);
                if (!iscopynull)
                {
                    if (item.Name != null) if (oob != null) try { item.SetValue(obj, oob, null); }
                            catch (Exception ex)
                            {
                                //FileHelpers.Writelog(ex);
                            }
                }
                else
                {
                    if (item.Name != null) if (oob != null) try { item.SetValue(obj, oob, null); }
                            catch (Exception ex)
                            {
                                //FileHelpers.Writelog(ex);
                            }
                }
            }
            return (T)obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="Tobj"></param>
        /// <param name="iscopynull"></param>
        /// <returns></returns>
        static T Dictionary2Object<T>(this Dictionary<string, object> instance, T Tobj = null, bool iscopynull = false) where T : class, new()
        {
            object obj = new object();
            Type t = typeof(T);
            if (Tobj == null)
            {
                obj = Activator.CreateInstance(t, null);
            }
            else
            {
                obj = Tobj;
            }


            var lists = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in lists)
            {

                object oob = instance.GetValueByKey(item.Name);
                if (!iscopynull)
                {
                    if (item.Name != null) if (oob != null) try { item.SetValue(obj, oob, null); }
                            catch (Exception ex)
                            {
                                //FileHelpers.Writelog(ex);
                            }
                }
                else
                {
                    if (item.Name != null) if (oob != null) try { item.SetValue(obj, oob, null); }
                            catch (Exception ex)
                            {
                                //FileHelpers.Writelog(ex);
                            }
                }
            }
            return (T)obj;
        }
        #region  Memo:验证

        // 验证电话号码的主要代码如下：
        /// <summary>
        /// 验证是否是电话号吗
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool validated_IsTelephone(this string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void validatedIsTelephone(this object str_telephone, string message = "电话号不正确", string code = "0")
        {
            if (str_telephone is string)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(str_telephone as string, @"^(\d{3,4}-)?\d{6,8}$"))
                {
                    throw
            new DebugException(code, message);
                }
            }
            else
            {
                throw
              new DebugException(code, message);
            }
        }
        // 验证手机号码的主要代码如下：
        /// <summary>
        /// 验证是否是手机号
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool validated_IsPoneNum(this string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,5,4,6,7,8]+\d{9}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void validatedIsPoneNum(this object str_telephone, string message = "手机号不正确", string code = "0")
        {

            if (str_telephone is string)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(str_telephone as string, @"^[1]+[3,5,4,6,7,8,9]+\d{9}"))
                {
                    throw
            new DebugException(code, message);
                }
            }
            else
            {
                throw
              new DebugException(code, message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        public static bool IsGuid(this string str)
        {
            Guid gv = new Guid();
            try
            {
                gv = new Guid(str);
            }
            catch (Exception ex)
            {
                //FileHelpers.Writelog(ex);

            }
            if (gv != Guid.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断是否为GUID
        /// </summary>
        /// <param name="str_date"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void validatedIsGUID(this object str_date, string message = "数据必需是GUID格式", string code = "0")
        {

            if ((str_date == null))
            {
                throw
            new DebugException(code, message);
            }
            if (!str_date.ToString().IsGuid())
            {
                throw
          new DebugException(code, message);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_date"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void validatedIsNull(this object str_date, string message = "不可以为空", string code = "0")
        {

            if ((str_date == null))
            {
                throw
            new DebugException(code, message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_date"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void validatedIsDate(this object str_date, string message = "日期格式不正确", string code = "0")
        {

            if (!(str_date is DateTime))
            {
                throw
            new DebugException(code, message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_date"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void validatedIsString(this object str_date, string message = "字符串不能为空！", string code = "0")
        {

            if ((str_date is string))
            {
                if (string.IsNullOrWhiteSpace(str_date as string))
                    throw
                new DebugException(code, message);
                if (str_date.ToString().Trim().Length == 0) throw
                new DebugException(code, message);
            }
            else
            {
                throw
                            new DebugException(code, message);
            }

        }
        /// <summary>
        /// 是否为有效字符串
        /// </summary>
        /// <param name="str_date"></param>
        /// <returns></returns>
        public static bool validated_IsString(this object str_date)
        {

            if ((str_date is string))
            {
                if (string.IsNullOrWhiteSpace(str_date as string)) return false;

                if (str_date.ToString().Trim().Length == 0) return false;
                return true;
            }
            else
            {
                return false;
            }

        }
        // 验证身份证号的主要代码如下：
        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool validated_IsIDcard(this string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }
        //验证输入为数字的主要代码如下：
        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="str_number"></param>
        /// <returns></returns>
        public static bool validated_IsNumber(this string str_number)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="str_number"></param>
         /// <param name="message"></param>
         /// <param name="code"></param>
        public static void validatedIsNumber(this object str_number, string message = "数值类型格式不正确", string code = "0")
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(str_number as string, @"^[1]+[3,5]+\d{9}"))
            {
                throw
        new DebugException(code, message);
            }
        }
        // 验证邮编的主要代码如下：
        /// <summary>
        /// 验证是否为邮编号码
        /// </summary>
        /// <param name="str_postalcode"></param>
        /// <returns></returns>
        public static bool validated_IsPostalcode(this string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }
        #endregion
        /// <summary>
        /// 通过指定字段名 返回实体列表；Addede by tony 2019-12-19 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr">JSON字符串</param>
        /// <param name="fieldname">节点名</param>
        /// <param name="iscopynull">是否复制空值（默认不复制）</param>
        /// <returns>返回对象列表</returns>
        public static List<T> JsonToEntity<T>(this string jsonStr, string fieldname = "root", bool iscopynull = false) where T : class, new()
        {
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonStr, "root");
            var list = doc.SelectNodes("//" + fieldname);
            List<T> result = new List<T>();
            object obj = new object();

            foreach (XmlElement element in list)
            {
                Type t = typeof(T);

                obj = Activator.CreateInstance(t, null);

                var lists = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var item in lists)
                {
                    if (item.Name == null) continue;

                    if (!item.PropertyType.IsPrimitive)
                    {
                        continue;
                    }
                    object oob;
                    try
                    {
                        oob = element.GetElementsByTagName(item.Name)[0].InnerText;
                    }
                    catch
                    {
                        oob = null;
                    }
                    if (!iscopynull)
                    {
                        if (item.Name != null) if (oob != null) try
                                {


                                    #region

                                    string strtypename = item.PropertyType.Name;

                                    if (strtypename.ToUpper() == "Int32".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, Int32.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else if (strtypename.ToUpper() == "Decimal".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, decimal.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else if (strtypename.ToUpper() == "Double".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, Double.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else if (strtypename.ToUpper() == "DateTime".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, DateTime.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    else if (strtypename.ToUpper() == "float".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, float.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    else
                                    if (("tony" + strtypename).ToUpper().IndexOf("INT") > 0)
                                    {
                                        try
                                        {
                                            item.SetValue(obj, oob, null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else
                                    if (strtypename.ToUpper() == "LONG")
                                    {
                                        try
                                        {
                                            item.SetValue(obj, long.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else
                                    if (("tony" + strtypename).ToUpper().IndexOf("DATE") > 0)
                                    {
                                        try
                                        {
                                            if (oob != null)
                                            {
                                                item.SetValue(obj, oob, null);
                                            }
                                            else
                                            {
                                                item.SetValue(obj, DateTime.Now, null);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, DateTime.Now, null);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            item.SetValue(obj, oob, null);
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    }

                                    #endregion
                                }
                                catch (Exception ex)
                                {

                                }
                    }
                    else
                    {
                        if (item.Name != null) if (oob != null) try
                                {


                                    #region

                                    string strtypename = item.PropertyType.Name;

                                    if (strtypename.ToUpper() == "Int32".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, Int32.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else if (strtypename.ToUpper() == "Decimal".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, decimal.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else if (strtypename.ToUpper() == "Double".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, Double.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else if (strtypename.ToUpper() == "DateTime".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, DateTime.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    else if (strtypename.ToUpper() == "float".ToUpper())
                                    {
                                        try
                                        {
                                            item.SetValue(obj, float.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    else
                                    if (("tony" + strtypename).ToUpper().IndexOf("INT") > 0)
                                    {
                                        try
                                        {
                                            item.SetValue(obj, oob, null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else
                                    if (strtypename.ToUpper() == "LONG")
                                    {
                                        try
                                        {
                                            item.SetValue(obj, long.Parse(oob.ToString()), null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, 0, null);
                                        }
                                    }
                                    else
                                    if (("tony" + strtypename).ToUpper().IndexOf("DATE") > 0)
                                    {
                                        try
                                        {
                                            if (oob != null)
                                            {
                                                item.SetValue(obj, oob, null);
                                            }
                                            else
                                            {
                                                item.SetValue(obj, DateTime.Now, null);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, DateTime.Now, null);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            item.SetValue(obj, oob, null);
                                        }
                                        catch (Exception ex)
                                        {
                                            item.SetValue(obj, null, null);

                                        }

                                    }

                                    #endregion
                                }
                                catch (Exception ex)
                                {

                                }
                    }
                }
                result.Add((T)obj);



            }

            return result;

        }
        /// <summary>
        /// datatabletodictionary
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
       public static Dictionary<string,Dictionary<string,object>> ToDictionary(this DataTable dt) 
        {
            var list = new Dictionary<string, Dictionary<string, object>>();
            int row = 0;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new Dictionary<string, object>();

                //遍历该对象的所有属性  
                foreach (DataColumn p in dt.Columns)
                {
                    entity.Add(p.ColumnName, dr[p.ColumnName]);
                }
                //对象添加到泛型集合中  
                list.Add("row"+row.ToString(),entity);
                row++;
            }
            return list;
        }
        /// <summary>
        /// datatabletodictionary
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, object>> ToDictionaries(this DataTable dt,string prefix="")
        {
            var list = new Dictionary<string, Dictionary<string, object>>();
            int row = 0;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new Dictionary<string, object>();

                //遍历该对象的所有属性  
                foreach (DataColumn p in dt.Columns)
                {
                    entity.Add(p.ColumnName, dr[p.ColumnName]);
                }
                if (prefix == null) prefix = "row";
                if (prefix.Trim().Length == 0) prefix = "";
                //对象添加到泛型集合中  
                list.Add(prefix+ row.ToString(), entity);
                row++;
            }
            return list;
        }


        /// <summary>
        /// datatabletodictionary
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List< Dictionary<string, object>> ToListDictionary(this DataTable dt)
        {
            var list = new List< Dictionary<string, object>>();
           
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new Dictionary<string, object>();

                //遍历该对象的所有属性  
                foreach (DataColumn p in dt.Columns)
                {
                    entity.Add(p.ColumnName, dr[p.ColumnName]);
                }
                //对象添加到泛型集合中  
                list.Add( entity);               
            }
            return list;
        }
        public static object GetValue(this DataTable dt)
        {
            if (dt as DataTable == null) return "";
            if (dt == null) return "";
            if (dt.Rows.Count < 1) return "";
            if (dt.Columns.Count < 1) return "";
            return dt.Rows[0][0];

        }
        public static string GetStringValue(this DataTable dt)
        {
            if (dt as DataTable == null) return "";
            string str = "TRUE";
            if (dt == null) return "";
            if (dt.Rows.Count < 1) return "";
            if (dt.Columns.Count < 1) return "";
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0] != null) str = dt.Rows[0][0] as string;
                if (!str.CheckStringValue()) str = "TRUE";
            }
            return str;
        }
        /// <summary>
        /// 通过表的名称　在List中获取DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <param name="TableName">表的名称</param>
        /// <returns></returns>
        public static DataTable GetValue(this IList<DataTable> list, string TableName = "")
        {
            if (list as IList<DataTable> == null) return new DataTable();
            DataTable dt = new DataTable();
            foreach (var itm in list)
            {
                if (itm.TableName.ToUpper() == TableName.ToUpper())
                {
                    if (itm as DataTable == null) continue;

                    dt = itm; break;
                }
            }

            dt.TableName = TableName;
            return dt;
        }
        /// <summary>
        /// 在List中获取默认DataTable
        /// </summary>
        /// <param name="list"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static DataTable GetDefaultValue(this IList<DataTable> list)
        {
            if (list as IList<DataTable> == null) return new DataTable();

            return list.LastOrDefault();


        }
        /// <summary>
        /// DataTable导出为EXCEL格式文件内容
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>返回CSV文件内容</returns>
        public static string ExportToCSVContent(this DataTable dt)
        {
            if (dt as DataTable == null) return "";

            string XLScontent = "";
            if (!dt.IsValidated()) return "";
            string str = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                str += dt.Columns[i] + ",";
            }
            XLScontent += str + "\r\n";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    str += dt.Rows[i][j] + ",";
                }
                XLScontent += str + "\r\n";
            }
            return XLScontent;
        }
    }



}
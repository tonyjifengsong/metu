using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{

    /// <summary>
    /// 通用操作方法
    /// </summary>
    public static class Common
    {

        /// <summary>
        /// 拷贝List数据（值和引用类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listIn"></param>
        /// <returns></returns>
        public static List<T> CopyList<T>(List<T> listIn)
        {
            if (listIn == null)
            {
                return null;
            }
            List<T> listTo = new List<T>();
            for (int i = 0; i < listIn.Count; i++)
            {
                listTo.Add(listIn[i]);
            }
            return listTo;
        }

        /// <summary>
        /// 数组转集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static List<T> ArrayToList<T>(params T[] datas)
        {
            List<T> listRet = new List<T>();
            for (int i = 0; i < datas.Length; i++)
            {
                listRet.Add(datas[i]);
            }
            return listRet;
        }

        /// <summary>
        ///集合转数组 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static T[] ListToArray<T>(List<T> datas)
        {
            T[] arrayRet = new T[datas.Count];
            for (int i = 0; i < datas.Count; i++)
            {
                arrayRet[i] = datas[i];
            }
            return arrayRet;
        }

        /// <summary>
        /// xml中未找到指定信息时的提示消息
        /// </summary>
        public static string notFoundMessage { get; } = null;

        /// <summary>
        /// 无效的日期时间
        /// </summary>
        public static DateTime invalidTime { get; } = new DateTime(1, 1, 1);

        /// <summary>
        /// 从字符串解析日期时间，当字符格式无效时，返回无效日期字段
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static DateTime ParseDateTime(string str)
        {
            DateTime time = invalidTime;
            if (IsStringNull(str))
            {
                return time;
            }
            try
            {
                time = DateTime.Parse(str);
            }
            catch
            {
                time = invalidTime;
            }
            return time;
        }

        /// <summary>
        /// 判断日期时间是否有效
        /// </summary>
        /// <param name="time">日期</param>
        /// <returns></returns>
        public static bool DateTimeValid(DateTime time)
        {
            if (time == invalidTime)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 初始化XMLResponse
        /// </summary>
        public static XMLResponse resCommon = new XMLResponse("", "", exceptType.normal, true, "", "");

        /// <summary>
        /// 获取异常字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetExceptionInfo(exceptType type)
        {
            switch (type)
            {
                case exceptType.cannotConnect:
                    return "连接服务器异常";
                case exceptType.timeout:
                    return "连接超时";
                case exceptType.formatFail:
                    return "返回XML格式异常";
                case exceptType.normal:
                    return "无异常";
                default:
                    return "其他异常";
            }
        }


        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <returns></returns>
        public static bool IsNull(this System.String str)
        {
            if (str == "" || str == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断字符串是否为null值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsStringNull(string str)
        {
            if (str == "" || str == null)
            {

                return true;
            }
            return false;
        }


        /// <summary>
        /// 反转bool值
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ReverseBool(bool b)
        {
            if (b)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 将List<T>添加到DataTable中
        /// </summary>
        /// <typeparam name="T">泛型值</typeparam>
        /// <param name="ColumnName">字段名</param>
        /// <param name="lstT">字段内容</param>
        /// <returns></returns>
        public static DataTable AddList<T>(this DataTable dt, string ColumnName, List<T> lstT)
        {
            try
            {
                if (ColumnName.IsNull())
                {
                    return dt;
                }

                if (lstT.Count == 0)
                {
                    return dt;
                }

                if (dt.Columns.IndexOf(ColumnName) <= 0)
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = ColumnName;
                    dc.DataType = typeof(T);
                    dt.Columns.Add(dc);
                }

                DataRowCollection drc = dt.Rows;

                for (int i = 0; i < lstT.Count; i++)
                {
                    if (dt.Rows.Count > i)
                    {
                        dt.Rows[i][ColumnName] = lstT[i];
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr[ColumnName] = lstT[i];
                        dt.Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                return dt;
            }

            return dt;
        }



    }

    /// <summary>
    /// 客户端请求的异常类型。无法连接；超时；XML格式异常；无异常
    /// </summary>
    public enum exceptType { cannotConnect, timeout, formatFail, normal, Others };


    /// <summary>
    /// 参数名和参数值的字符串对
    /// </summary>
    public struct prmValPair
    {
        public string name;
        public string value;
        public prmValPair(string nameIn, string valueIn)
        {
            this.name = nameIn;
            this.value = valueIn;
        }
        public string ToXmlString()
        {
            return XMLOps.GetNodeString(name, value, false);
        }
        //判断数据是否有效
        public bool Valid()
        {
            if (Common.IsStringNull(name) || Common.IsStringNull(value))
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 参数名和参数值集合对
    /// </summary>
    public struct prmListValPair
    {
        public string name;
        public List<string> values;
        public prmListValPair(string nameIn, List<string> valuesIn)
        {
            this.name = nameIn;
            this.values = new List<string>();
            this.values = Common.CopyList<string>(valuesIn);
        }
        public prmListValPair(string nameIn)
        {
            this.name = nameIn;
            this.values = new List<string>();
        }
        public string ToXmlString()
        {
            if (values == null)
            {
                return null;
            }
            string xml = XMLOps.GetNodeString(name, "", false);
            for (int i = 0; i < values.Count; i++)
            {
                string temp = XMLOps.GetNodeString("listItem", values[i], true);
                xml = XMLOps.InsertValue(xml, name, temp);
            }
            return xml;
        }
    }

    /// <summary>
    /// 字符串与索引号的结构
    /// </summary>
    internal struct StrIndexPair
    {
        public string name;
        public int index;
        public StrIndexPair(string nameIn, int indexIn)
        {
            this.name = nameIn;
            this.index = indexIn;
        }
    }



}

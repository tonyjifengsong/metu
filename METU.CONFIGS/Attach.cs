using METU.CACHES;
using System.Collections.Generic;

namespace System
{
    public static class Attach
    {
        #region List字符串互转
        /// <summary>
        /// List转字符串
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        public static string ListToString(this List<string> strList)
        {
            string result = "";
            if (strList != null && strList.Count > ConstNum.Zero)
            {
                foreach (var item in strList)
                {
                    result += item + ",";
                }
                result = result.TrimEnd(',');
            }
            return result;
        }


        /// <summary>
        /// 字符串转List
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> stringToList(this string str)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < str.Split(',').Length; i++)
            {
                list.Add(str[i].ToString());
            }
            return list;
        }

        #endregion

        #region 数组字符串处理
        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        public static string ArryToString(this string[] strList)
        {
            string result = "";
            if (strList != null && strList.Length > ConstNum.Zero)
            {
                foreach (var item in strList)
                {
                    result += item + ",";
                }
                result = result.TrimEnd(',');
            }
            else
            {
                return "";
            }
            return result;
        }


        /// <summary>
        /// 字符串转数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] stringToArry(this string str)
        {
            string[] result = new string[] { };
            if (!string.IsNullOrWhiteSpace(str))
            {
                result = str.Split(',');
            }
            return result;
        }
        #endregion


        #region  图片地址处理
       
        /// <summary>
        /// 图片地址拼接域名
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string[] PicProcessJoinDomain(this string[] pic,string domain=null)
        {
            if (pic != null && pic.Length > 0)
            {
                for (int i = 0; i < pic.Length; i++)
                {
                    if (domain == null )
                    {
                        pic[i] = CommonCache.PicRequest + pic[i];
                        continue;
                    }
                    if ( domain.Trim().Length == 0)
                    {
                        pic[i] = CommonCache.PicRequest + pic[i];
                        continue;
                    }
                    
                        pic[i] = domain + pic[i];
                    
                }
                return pic;
            }
            else
            {
                return null;
            }
        }

       


        /// <summary>
        /// /图片地址移除域名
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string[] PicProcessRemoveDomain(this string[] pic, string domain = null)
        {
            if (!string.IsNullOrEmpty(CommonCache.PicRequest))
            {
                if (pic != null && pic.Length > ConstNum.Zero)
                {
                    for (int i = 0; i < pic.Length; i++)
                    {
                        if (domain == null)
                        {
                            pic[i] = pic[i].Replace(CommonCache.PicRequest, "");
                            continue;
                        }
                        if (domain.Trim().Length == 0)
                        {
                            pic[i] = pic[i].Replace(CommonCache.PicRequest, "");
                            continue;
                        }
                        pic[i] = pic[i].Replace(domain, "");
                    }
                    return pic;
                }
                else
                {
                    return null;
                }
            }
            return pic;
        }
     
        /// <summary>
        ///  移除域名
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string PicProcessRemoveDomain(this string pic, string domain = null)
        {
            if (!string.IsNullOrEmpty(CommonCache.PicRequest))
            {
                if (!string.IsNullOrWhiteSpace(pic))
                {
                    if (domain == null)
                    {
                        pic = pic.Replace(CommonCache.PicRequest, "");
                        return pic;
                    }
                    if (domain.Trim().Length == 0)
                    {
                        pic = pic.Replace(CommonCache.PicRequest, "");
                        return pic;
                    }
                    pic = pic.Replace(CommonCache.PicRequest, "");
                    return pic;
                }
                else
                {
                    return null;
                }
            }
            return pic;
        }


        #endregion

    }
}

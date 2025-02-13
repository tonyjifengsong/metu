using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System
{
    public static class JsonHelper
    {
        /// <summary>
        /// 把对象序列化 JSON 字符串 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实体</param>
        /// <returns>JSON字符串</returns>
        public static string GetJson<T>(this T obj)
        {
            //记住 添加引用 System.ServiceModel.Web 
            /**
             * 如果不添加上面的引用,System.Runtime.Serialization.Json; Json是出不来的哦
             * */
           
            
                string szJson = JsonSerializer.Serialize(obj);
            return szJson;
            
        }

        
        /// <summary>
        /// 把JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="szJson">JSON字符串</param>
        /// <returns>对象实体</returns>
        public static T ParseFormJson<T>(this string szJson)
        {
          return  JsonSerializer.Deserialize<T>(szJson);
        }
        /// <summary>
        /// 把JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="szJson">JSON字符串</param>
        /// <returns>对象实体</returns>
        public static T ToObject<T>(this string szJson)
        {
            return JsonSerializer.Deserialize<T>(szJson);
        }
    }
}

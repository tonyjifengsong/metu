using METU.CACHES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES
{
  public static  class ESCALL
    {/// <summary>
     /// POST数据到WEBAPI 并返回字符串
     /// </summary>
     /// <param name="url">WEBAPI地址</param>
     /// <param name="body">传输的数据 字符串</param>
     /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
     /// <returns>返回字符串</returns>
        public static string HttpPost(string url, string body, string ContentType = "application/json")
        {
            if (url == null)
            {

                url = CommonCache.LogURL;
            }
            if (url.ToString().Trim().Length < 5)
            {

                url = CommonCache.LogURL;
            }

            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();

                    return sr;
                }
            }
            catch (Exception ex)
            {


                return ex.Message;
            }
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="url"></param>
         /// <param name="body"></param>
         /// <param name="ContentType"></param>
         /// <returns></returns>
        public static string HttpPut(string url, string body, string ContentType = "application/json")
        {
            if (url == null)
            {

                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {

                return "";
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();

                    return sr;
                }
            }
            catch (Exception ex)
            {


                return ex.Message;
            }
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="url"></param>
         /// <param name="body"></param>
         /// <param name="ContentType"></param>
         /// <returns></returns>
        public static string HttpDelete(string url, string body, string ContentType = "application/json")
        {
            if (url == null)
            {

                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {

                return "";
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();

                    return sr;
                }
            }
            catch (Exception ex)
            {


                return ex.Message;
            }
        }

        /// <summary>
        /// 通过Get方式获取数据 WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <returns>返回字符串</returns>
        public static string HttpGet(string url)
        {
            if (url == null)
            {

                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {

                return "";
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            try
            {

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();

                    return sr;
                }
            }
            catch (Exception ex)
            {


                return ex.Message;
            }
        }


        #region 异步请求
        /// <summary>
        ///   异步日志添加
        /// </summary>
        /// <param name="body"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static void PostLog(object obj, string tablename = "Debug", string dbname = "tonydbLog", string id = null, string ContentType = "application/json")
        {
            if (CommonCache.LogURL == null)
            {

                CommonCache.LogURL = "http://localhost:9200";
            }
            if (CommonCache.LogURL.ToString().Trim().Length < 5)
            {

                CommonCache.LogURL = "http://localhost:9200";
            }
            try
            {
                string body = obj.ToString();

                if (id == null)
                {

                    string url = CommonCache.LogURL + "/" + dbname + "/" + tablename;

                    HttpPostAsync(url, body, ContentType);

                }
                else
                {
                    string url = CommonCache.LogURL + "/" + dbname + "/" + tablename + "/" + id;

                    HttpPostAsync(url, body, ContentType);


                }
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="body">传输的数据 字符串</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        static async Task<string> HttpPostAsync(string url, string body, string ContentType = "application/json")
        {
            if (url == null)
            {

                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {

                return "";
            }
            url = url.ToString().ToLower();//ElasticSearch中不允许在ＵＲＬ中存在大写字符
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            if (url == null) return "";
            if (body == null) return "";
            if (url.Trim().Length < 10) return "";
            if (body.Trim().Length < 1) return "";
            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();

                    return sr;
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        #endregion
    }
}

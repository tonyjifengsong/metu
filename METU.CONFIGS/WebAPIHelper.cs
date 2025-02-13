using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace METU.CONFIGS
{

    public static class WebAPIHelper
    {
        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="body">传输的数据 字符串</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static string HttpPost(string url, string body, string ContentType = "application/json")
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="dics">传输的数据 键值对</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static string PostData2WebAPI(string url, Dictionary<string, string> dics, string ContentType = "application/json")
        {

            string serviceAddress = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

            request.Method = "POST";
            request.ContentType = ContentType;
            string strContent = "{ ";
            foreach (var item in dics)
            {
                if (strContent.Length > 3) strContent = strContent + ",";
                strContent = strContent + "\"" + item.Key.ToString() + "\":";
                strContent = strContent + "\"" + item.Value.ToString() + "\"";
            }
            strContent = strContent + " }";
            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(strContent);
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;

        }

        /// <summary>
        /// 通过Get方式获取数据 WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <returns>返回字符串</returns>
        public static string HttpGet(string url)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="body">传输的数据 字符串</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static string HttpPost(string url, object obj, string ContentType = "application/json")
        {
            string body = obj.toJson();
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="body">传输的数据 字符串</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static string CallAPI(string url, object obj, string ContentType = "application/json")
        {
            string body = obj.toJson();
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="dics">传输的数据 键值对</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static string PostData2WebAPI(string url, object obj, string ContentType = "application/json")
        {
            Dictionary<string, string> dics = obj.ToDictionary();
            string serviceAddress = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

            request.Method = "POST";
            request.ContentType = ContentType;
            string strContent = "{ ";
            foreach (var item in dics)
            {
                if (strContent.Length > 3) strContent = strContent + ",";
                strContent = strContent + "\"" + item.Key.ToString() + "\":";
                strContent = strContent + "\"" + item.Value.ToString() + "\"";
            }
            strContent = strContent + " }";
            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(strContent);
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;

        }

        
    }
}

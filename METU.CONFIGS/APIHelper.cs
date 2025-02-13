using METU.CACHES;
using METU.MODEL;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace System
{/// <summary>
/// 
/// </summary>
    public static class APIHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool WebclientPost(object param, string url = null)
        {
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return false;
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return false;
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (url == null) url = CommonCache.LogURL + "/SPDdbname/SPDtablename/add";
            if (url.Trim().ToString().Length == 0) url = CommonCache.LogURL + "/SPDdbname/SPDtablename/add";
            try
            {
                HttpContent hc = new StringContent(JsonConvert.SerializeObject(param).ToString(), Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, hc).Result;
                //如果调用失败，抛出异常
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();

                    //result.Result 是一个json字符串,解析后就可以拿到调用后的返回值
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //FileHelper.Writelog(ex, "apiex");

                return false;
            }

            return true;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dics"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static object PostData2WXAPI(string url, Dictionary<string, string> dics, string ContentType = "application/json")
        {
            string serviceAddress = url;
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return false;
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return false;
            }
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

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                //FileHelper.Writelog(ex, "apiex");
                response = null;
            }
            if (response != null)
            {
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码  
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                return retString;
            }
            else
            {
                return "出错；数据没有提交成功";
            }

        }

        /// <summary>
        ///   通过传输JSON字符串调用API
        /// </summary>
        /// <param name="body"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static string SSOPostJson(string body, string ContentType = "application/json")
        {
            if (CommonCache.SSOURL == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (CommonCache.SSOURL.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
            }

            string url = CommonCache.SSOURL + "Login";
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
                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
                return ex.Message;
            }
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static string SSORegister(object obj, string ContentType = "application/json")
        {
            if (CommonCache.SSOURL == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (CommonCache.SSOURL.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
            }
            string url = CommonCache.SSOURL + "Register";
            string body = obj.toJson();
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
                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
                return ex.Message;
            }
        }

        /// <summary>
        /// 接口调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="token"></param>
        /// <param name="isHeader"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static Result  CALLAPI(string url, object obj, string token = null, bool isHeader = false, string ContentType = "application/json")
        {
            FileHelper.Writelog("==================================接口调用开始======================================", "webapi");
            url.validatedIsString("接口调用地址不可以为空！");
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return Result.ERROR(0, "请求地址为空");
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return Result.ERROR(0, "请求地址错误！");
            }

            string body = obj.toJson();
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;
            if (isHeader)
            {
                token.validatedIsString("Header必需有输入！");
                request.Headers.Add("token", token);
            }

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {
                FileHelper.Writelog("接口地址：", "webapi");

                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog("接口传参：", "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog("接口返回值：", "webapi");

                    FileHelper.Writelog(sr, "webapi");
                    FileHelper.Writelog("==================================接口调用结束======================================", "webapi");

                    return new Result(sr);
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("==================================接口调用结束======================================", "webapi");

                return Result.ERROR(0, ex.Message);
            }
        }

        /// <summary>
        /// 接口调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="token"></param>
        /// <param name="isHeader"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static  Result CALLWEBSERVICE(string url, object obj, string token = null, bool isHeader = false, string ContentType = "application/json")
        {
            url.validatedIsString("接口调用地址不可以为空！");
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return Result.ERROR(0, "请求地址为空");
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return Result.ERROR(0, "请求地址错误");
            }

            string body = obj.toJson();
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;
            if (isHeader)
            {
                token.validatedIsString("Header必需有输入！");
                request.Headers.Add("token", token);
            }

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {
                FileHelper.Writelog(url, "webapi");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                   
                    return new Result(sr);
                }
            }
            catch (Exception ex)
            {

                return Result.ERROR(0, ex.Message);
            }
        }
        /// <summary>
        /// 传输对象到SSOAPI
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static string SSOPostObject(object obj, string ContentType = "application/json")
        {
            if (CommonCache.SSOURL == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (CommonCache.SSOURL.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
            }
            string url = CommonCache.SSOURL + "Login";
            string body = obj.toJson();
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = ContentType;

            if (url == null) FileHelper.Writelog("请求地址为空", "webapi");
            if (url.ToString().Trim().Length < 5) FileHelper.Writelog("请求地址错误！", "webapi");

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {
                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
                return ex.Message;
            }
        }
        /// <summary>
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
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
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
                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
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
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
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
                FileHelper.Writelog(url, "webapi");
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
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
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
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
                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
                return ex.Message;
            }
        }
        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="dics">传输的数据 键值对</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static void PostData2WebAPI(string url, Dictionary<string, string> dics, string ContentType = "application/json")
        {
            asyncPostData2WebAPI(url, dics, ContentType);


        }/// <summary>
         /// 
         /// </summary>
         /// <param name="url"></param>
         /// <param name="dics"></param>
         /// <param name="ContentType"></param>
         /// <returns></returns>
        static async Task<string> asyncPostData2WebAPI(string url, Dictionary<string, string> dics, string ContentType = "application/json")
        {
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
            }
            string r = await Task.Run(() =>
            {
                string serviceAddress = url;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
                FileHelper.Writelog(url, "webapi");
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

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception ex)
                {
                    FileHelper.Writelog(ex, "apiex");
                    response = null;
                }
                if (response != null)
                {
                    string encoding = response.ContentEncoding;
                    if (encoding == null || encoding.Length < 1)
                    {
                        encoding = "UTF-8"; //默认编码  
                    }
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                    string retString = reader.ReadToEnd();
                    return retString;
                }
                else
                {
                    return "出错；数据没有提交成功";
                }
            });

            return r;
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
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            try
            {
                FileHelper.Writelog(url, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
                return ex.Message;
            }
        }

        /// <summary>
        /// 模拟curl get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HttpGet(string url, string userName, string password)
        {
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
            }
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(url);
            wrGETURL.Method = "GET";
            wrGETURL.ContentType = "application/json";
            wrGETURL.Credentials = new NetworkCredential(userName, password);
            try
            {
                FileHelper.Writelog(url, "webapi");
                Stream objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                string responseFromServer = objReader.ReadToEnd();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
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
        public static void PostLog(object obj, string tablename = "Debug", string dbname = "SPDLog", string ContentType = "application/json")
        {
            if (CommonCache.LogURL == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return;
            }
            if (CommonCache.LogURL.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return;
            }
            try
            {
                string body = obj.toJson();
                string url = CommonCache.LogURL + "/" + dbname + "/" + tablename + "/add";
                FileHelper.Writelog(obj, "webapiDebug");
                //HttpPostAsync(url, body, ContentType);
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
            }
        }
        /// <summary>
        ///   异步日志添加
        /// </summary>
        /// <param name="body"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static void PostLog(string body, string tablename = "Debug", string dbname = "SPDAPILOG", string ContentType = "application/json")
        {
            //http://esx72.temiicore.cn:5555/esapi/tonydb/tonytable/add
            if (string.IsNullOrEmpty(CommonCache.LogURL)) CommonCache.LogURL = "http://esx72.temiicore.cn:5555/esapi";
            string url = CommonCache.LogURL + "/" + dbname + "/" + tablename + "/add";
             

             HttpPostAsync(url, body, ContentType);
        }
        /// <summary>
        ///   异步日志添加
        /// </summary>
        /// <param name="body"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static void PostESLog(object body, string ContentType = "application/json")
        {
            string tablename = "Debug"; string dbname = "SPDAPILOG";
            if (CommonCache.LogURL == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return;
            }
            if (CommonCache.LogURL.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return;
            }
            if (string.IsNullOrEmpty(CommonCache.LogURL)) CommonCache.LogURL = "http://esx72.temiicore.cn:5555/esapi";
            string url = CommonCache.LogURL + "/" + dbname + "/" + tablename + "/add";


            //  HttpPostAsync(url, body.toJson(), ContentType);
        }
        public static void PostAPILog(object body, string ContentType = "application/json")
        {
            string tablename = "APIDebug"; string dbname = "SPDAPILOG";
            if (CommonCache.LogURL == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return;
            }
            if (CommonCache.LogURL.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return;
            }
            if (string.IsNullOrEmpty(CommonCache.LogURL)) CommonCache.LogURL = "http://esx72.temiicore.cn:5555/esapi";
            string url = CommonCache.LogURL + "/" + dbname + "/" + tablename + "/add";


            //  HttpPostAsync(url, body.toJson(), ContentType);
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
                FileHelper.Writelog("请求地址为空", "webapi");
                return "";
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return "";
            }
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
                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    FileHelper.Writelog(sr, "webapi");
                    return sr;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "apiex");
                FileHelper.Writelog(ex, "webapi");
                return ex.Message;
            }
        }
        #endregion

        /// <summary>
        /// 通用WEBAPI调用接口 默认XML
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static  Result  CALLWEBAPI(string url, object obj, string ContentType = "application/xml")
        {
            FileHelper.Writelog("==================================接口调用开始======================================", "webapi");
            url.validatedIsString("接口调用地址不可以为空！");
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return Result.ERROR(0, "请求地址为空");
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return Result.ERROR(0, "请求地址错误！");
            }

            string body = obj.toJson();
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
                FileHelper.Writelog("接口地址：", "webapi");

                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog("接口传参：", "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();

                    return new Result(sr);
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("==================================接口调用结束======================================", "webapi");

                return Result.ERROR(0, ex.Message);
            }
        }
        /// <summary>
        /// 通用WEBAPI调用接口 默认Json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static  Result  CALLJsonAPI(string url, object obj, string ContentType = "application/json")
        {
            FileHelper.Writelog("==================================接口调用开始======================================", "webapi");
            url.validatedIsString("接口调用地址不可以为空！");
            if (url == null)
            {
                FileHelper.Writelog("请求地址为空", "webapi");
                return  Result.ERROR( 0,"请求地址为空");
            }
            if (url.ToString().Trim().Length < 5)
            {
                FileHelper.Writelog("请求地址错误！", "webapi");
                return Result.ERROR(0, "请求地址错误！");
            }

            string body = obj.toJson();
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
                FileHelper.Writelog("接口地址：", "webapi");

                FileHelper.Writelog(url, "webapi");
                FileHelper.Writelog("接口传参：", "webapi");
                FileHelper.Writelog(body, "webapi");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string sr = reader.ReadToEnd();
                    

                    return new Result(sr);
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("==================================接口调用结束======================================", "webapi");

                return Result.ERROR(0, ex.Message);
            }
        }


        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="body">传输的数据 字符串</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static string HttpJsonPost(string url, string body)
        {
            string ContentType = "application/json";
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
        }
        /// <summary>
        /// POST数据到WEBAPI 并返回字符串
        /// </summary>
        /// <param name="url">WEBAPI地址</param>
        /// <param name="body">传输的数据 字符串</param>
        /// <param name="ContentType">传输数据格式（默认为JSON格式数据）</param>
        /// <returns>返回字符串</returns>
        public static string HttpxmlPost(string url, string body)
        {
            string ContentType = "application/xml";
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
        }


    }

}

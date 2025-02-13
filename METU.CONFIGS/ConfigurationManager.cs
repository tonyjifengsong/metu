using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace METU.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <summary>
    /// Json文件读写
    /// 引用Newtonsoft.Json
    /// </summary>
    public class ConfHelper
    {
       
        private string _path;
        private IConfiguration Configuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonName"></param>
        public ConfHelper(string jsonName= "appsettings.json")
        { //在当前目录或者根目录中寻找appsettings.json文件
            FileHelper.Writelog("ConfHelper constructor param jsonName:");

            FileHelper.Writelog(jsonName);

            if (jsonName == null) jsonName = "appsettings.json";
            if (jsonName.Trim().Length < 4)
            {
                FileHelper.Writelog("ConfHelper constructor param: jsonName is null");

                jsonName = "appsettings.json";
            }
            if (!jsonName.EndsWith(".json"))
            {
                FileHelper.Writelog("fileName  not EndsWith json");

                jsonName = $"{jsonName}.json";
                FileHelper.Writelog(jsonName);

            }
            var fileName = jsonName;
           
            var directory = AppContext.BaseDirectory;
            FileHelper.Writelog("ConfHelper AppContext.BaseDirectory:");
            FileHelper.Writelog(directory);
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            
                int length = directory.IndexOf("/bin");
            if (length > 0)
            {
                FileHelper.Writelog("ConfHelper Contains /bin:");

                filePath = $"{directory.Substring(0, length)}/{fileName}";
                FileHelper.Writelog("ConfHelper Contains /bin  file path:");
                FileHelper.Writelog(filePath);
            }
            FileHelper.Writelog("ConfHelper  constructor:");
            filePath = filePath.Replace("\\", "/").Replace("//", "/");
            FileHelper.Writelog(filePath);
            FileHelper.Writelog("ConfHelper config file path:" + filePath);
            _path = filePath;
            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, true, true);
            Configuration = builder.Build();
        }

        /// <summary>
        /// 读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public T Read<T>() => Read<T>("");

        /// <summary>
        /// 根据节点读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public T Read<T>(string section)
        {
           
            try
            { FileHelper.Writelog("ConfHelper Read config file path:" );

                FileHelper.Writelog(_path);
                FileHelper.Writelog("ConfHelper Read config section:");

                FileHelper.Writelog(section);

                using (var file = new StreamReader(_path))
                using (var reader = new JsonTextReader(file))
                {
                    var jObj = (JObject)JToken.ReadFrom(reader);
                    if (!string.IsNullOrWhiteSpace(section))
                    {
                        var secJt = jObj[section];
                        if (secJt != null)
                        {
                            FileHelper.Writelog("ConfHelper Read config  "+ section);
                            FileHelper.Writelog(secJt.ToString());

                            return JsonConvert.DeserializeObject<T>(secJt.ToString());
                        }
                    }
                    else
                    {
                        FileHelper.Writelog("ConfHelper Read-config  " + section);
                        FileHelper.Writelog(jObj.ToString());

                        return JsonConvert.DeserializeObject<T>(jObj.ToString());
                    }
                }
            }
            catch 
            {
                return default(T);

            }
            return default(T);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public string ReadConfig(string section)
        {
            try
            {
                FileHelper.Writelog("ConfHelper ReadConfig config file path:");

                FileHelper.Writelog(_path);
                FileHelper.Writelog("ConfHelper ReadConfig config section:");

                FileHelper.Writelog(section);

                using (var file = new StreamReader(_path))
                using (var reader = new JsonTextReader(file))
                {
                    var jObj = (JObject)JToken.ReadFrom(reader);
                    if (!string.IsNullOrWhiteSpace(section))
                    {
                        var secJt = jObj[section];
                        if (secJt != null)
                        {
                            return secJt.ToString();
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch 
            {

                return "";
            }
            return "";
        }
        /// <summary>
        /// 读取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>() => ReadList<T>("");

        /// <summary>
        /// 根据节点读取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>(string section)
        {
            try
            {
                FileHelper.Writelog("ConfHelper ReadList config file path:");

                FileHelper.Writelog(_path);
                FileHelper.Writelog("ConfHelper ReadList config section:");

                FileHelper.Writelog(section);
                using (var file = new StreamReader(_path))
                {
                    if (file == null)
                    {
                        FileHelper.Writelog(" StreamReader Failed! ");
                        return new List<T>();
                    }
                    using (var reader = new JsonTextReader(file))
                    {
                        if (reader == null)
                        {
                            FileHelper.Writelog(" JsonTextReader Failed! ");
                            return new List<T>();
                        }
                        var jObj = (JObject)JToken.ReadFrom(reader);
                        if (!string.IsNullOrWhiteSpace(section))
                        {
                            var secJt = jObj[section];
                            if (secJt != null)
                            {
                                FileHelper.Writelog("ConfHelper Read-config  " + section);
                                FileHelper.Writelog(secJt.ToString());

                                return JsonConvert.DeserializeObject<List<T>>(secJt.ToString());
                            }
                        }
                        else
                        {
                            FileHelper.Writelog("ConfHelper Read-config  " + section);
                            FileHelper.Writelog(jObj.ToString());

                            return JsonConvert.DeserializeObject<List<T>>(jObj.ToString());
                        }
                    }
                }
            }
            catch (Exception  ex)
            {
                FileHelper.Writelog("ConfHelper Read-config Exception: "  );
                FileHelper.Writelog(ex.Message);

                return default(List<T>);
            }
            return default(List<T>);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public void Write<T>(T t) => Write("", t);

        /// <summary>
        /// 写入指定section文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public void Write<T>(string section, T t)
        {
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(_path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObj = (JObject)JToken.ReadFrom(reader);
                    var json = JsonConvert.SerializeObject(t);
                    if (string.IsNullOrWhiteSpace(section))
                        jObj = JObject.Parse(json);
                    else
                        jObj[section] = JObject.Parse(json);
                }

                using (var writer = new StreamWriter(_path))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jObj.WriteTo(jsonWriter);
                }
            }
            catch  
            {
               

               
            }
        }

        /// <summary>
        /// 删除指定section节点
        /// </summary>
        /// <param name="section"></param>
        public void Remove(string section)
        {
            try
            {
                JObject jObj;
                using (StreamReader file = new StreamReader(_path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObj = (JObject)JToken.ReadFrom(reader);
                    jObj.Remove(section);
                }

                using (var writer = new StreamWriter(_path))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jObj.WriteTo(jsonWriter);
                }
            }
            catch  
            {
               

                
            }
        }
    }
}

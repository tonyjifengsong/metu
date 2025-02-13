using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace METU.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigHelper
    {
       static ConfHelper conf;
        /// <summary>
        /// 
        /// </summary>
        private static IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        static ConfigHelper()
        {
            //在当前目录或者根目录中寻找appsettings.json文件
            var fileName = "appsettings.json";
             var directory = AppContext.BaseDirectory;
            FileHelper.Writelog("ConfigHelper-AppContext.BaseDirectory:");
            FileHelper.Writelog(directory);
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";

            int length = directory.IndexOf("/bin");
            if (length > 0)
            {
              
                filePath = $"{directory.Substring(0, length)}/{fileName}";
                FileHelper.Writelog("ConfigHelper-Contains /bin  file path:");
                FileHelper.Writelog(filePath);
            }

            FileHelper.Writelog("ConfigHelper-ConfigHelper json path:");
            filePath= filePath.Replace("\\", "/").Replace("//", "/"); 
            FileHelper.Writelog(filePath);
            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, true, true);

          conf = new ConfHelper(filePath);
             _configuration = builder.Build();
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSectionValue(string key)
        {
            if (key == null)
            {
                FileHelper.Writelog("GetConfigSettings  param- Key is null");
                return "";
            }
            if (key.Trim().ToString().Length < 1)
            {
                FileHelper.Writelog("GetConfigSettings  param- Key is Zero");
                return "";
            }
            try
            {
                FileHelper.Writelog("GetSectionValue config["+key+"]:");
              
                return conf.ReadConfig(key);
            }catch(Exception ex)
            {
                FileHelper.Writelog("GetSectionValue exceptions:");
                FileHelper.Writelog(ex.Message);

            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetConfigSettings(string key, string fileName = "appsettings.json")
        {
            if (key == null)
            {
                FileHelper.Writelog("GetConfigSettings  param- Key is null");
                return "";
            }
            if (key.Trim().ToString().Length<1)
            {
                FileHelper.Writelog("GetConfigSettings  param- Key is Zero");
                return "";
            }
            FileHelper.Writelog("GetConfigSettings  param- Key:");
            FileHelper.Writelog(key);

            FileHelper.Writelog("GetConfigSettings param-fileName:");
            FileHelper.Writelog( fileName );

            if (fileName == null) fileName = "appsettings.json";
            if (!fileName.EndsWith(".json"))
            {
                FileHelper.Writelog("param-fileName  not EndsWith json");

                fileName = $"{fileName}.json";
                FileHelper.Writelog(fileName);
            }
            // 获取bin目录路径
            var directory = AppContext.BaseDirectory;
            FileHelper.Writelog("GetConfigSettings-AppContext.BaseDirectory:");
            FileHelper.Writelog(directory);

            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";

            int length = directory.IndexOf("/bin");
            if (length > 0)
            {
                filePath = $"{directory.Substring(0, length)}/{fileName}";
                FileHelper.Writelog("Contains /bin  file path:");
                FileHelper.Writelog(filePath);
            }

            FileHelper.Writelog("ConfigHelper GetConfigSettings json path:");
            filePath = filePath.Replace("\\", "/").Replace("//", "/");
            FileHelper.Writelog(filePath);

            var builder = new ConfigurationBuilder();

            builder.AddJsonFile(filePath, false, true);

            var config = builder.Build();
            try
            {
                FileHelper.Writelog("GetConfigSettings config[key]:");
                FileHelper.Writelog(config[key]);
                return config[key]==null?"" : config[key];
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("GetConfigSettings exceptions:");
                FileHelper.Writelog(ex.Message);

            }
            return "";
        }

        /// <summary>
        /// 转换对象为JSON格式数据
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>字符格式的JSON数据</returns>
        public static string GetJSON<T>(object obj)
        {
            string result = String.Empty;
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
             return result;  
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("GetJSON exceptions:");
                FileHelper.Writelog(ex.Message);

            }
            return "";
        }
           
        
        /// <summary>
        /// 转换List<T>的数据为JSON格式
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="vals">列表值</param>
        /// <returns>JSON格式数据</returns>
        public static string JSON<T>(List<T> vals)
        {
            System.Text.StringBuilder st = new System.Text.StringBuilder();
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

                foreach (T city in vals)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        s.WriteObject(ms, city);
                        st.Append(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                return st.ToString(); 
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("JSON exceptions:");
                FileHelper.Writelog(ex.Message);

            }
            return "";           
        }

        /// <summary>
        /// JSON格式字符转换为T类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ParseFormByJson<T>(string jsonStr)
        {
            T obj = Activator.CreateInstance<T>();
            using (System.IO.MemoryStream ms =
            new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStr)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                try { 
                return (T)serializer.ReadObject(ms);
                }
                catch (Exception ex)
                {
                    FileHelper.Writelog("ParseFormByJson exceptions:");
                    FileHelper.Writelog(ex.Message);

                }
                return default(T);
            }
        }




        public static string JSON1<SendData>(List<SendData> vals)
        {
            System.Text.StringBuilder st = new System.Text.StringBuilder();
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(SendData));

                foreach (SendData city in vals)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        s.WriteObject(ms, city);
                        st.Append(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                   return st.ToString(); 
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("JSON1 exceptions:");
                FileHelper.Writelog(ex.Message);

            }
            return "";
        }


    }
}

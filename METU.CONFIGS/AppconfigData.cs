using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CONFIGS
{

    /// <summary>
    /// 读写APP配置信息
    /// </summary>
    public static class AppconfigData
    {
        public static IConfiguration Configuration { get; set; }
        static AppconfigData()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }
        /// <summary>
        /// 保存配置文件的内容
        /// </summary>
        /// <param name="configNodeName">配置节点名称</param>
        /// <param name="configValue">配置节点值</param>
        public static void SaveConfig(string key, string value)
        {
            try
            {
                AppSettingsHelper.Configuration[key] = value;
                
            }
            catch
            {
            }
        }
        /// <summary>
        /// 读取配置文件内容
        /// </summary>
        /// <param name="key">配置节名</param>
        /// <returns>获取配置节点值</returns>
        public static string Read(string key)
        {
            key = key.ToUpper();
            string strValue = null;
            try
            {
                strValue = AppSettingsHelper.Configuration[key];
            }
            catch { strValue = ""; }
            return strValue;
        }
        /// <summary>
        /// 写配置文件内容
        /// </summary>
        /// <param name="key">配置节名</param>
        /// <param name="value">获取配置节点值</param>
        public static void Write(string key, string value)
        {
            try
            {
                AppSettingsHelper.Configuration[key] = value;
            }
            catch { }
        }
    }
}

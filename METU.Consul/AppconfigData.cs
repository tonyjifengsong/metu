using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace METU.CONFIGS
{
    public class AppSettingsHelper
    {
        public static IConfiguration Configuration { get; set; }
        static AppSettingsHelper()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }

    }

    /// <summary>
    /// 读写APP配置信息
    /// </summary>
    public static class AppconfigData
    {
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
       
    }
}

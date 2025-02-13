using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;

namespace METU.CONFIGS
{
    public static class AppSettingsHelper
    {
        public static IConfiguration Configuration { get; set; }
        static AppSettingsHelper()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static String GetValue(string strValue)
        {
            var AppConfigValue = string.IsNullOrWhiteSpace(Configuration[strValue]) ? "0" : Configuration[strValue];
            return AppConfigValue;
        }

        #region DB操作连接参数
        /// <summary>
        /// IP地址
        /// </summary>
        /// <returns></returns>
        public static String GetIP
        {
            get
            {
                return GetValue("MESIP");
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        /// <returns></returns>
        public static String GetUserName
        {
            get
            {
                return GetValue("MESUserName");
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
        public static String GetPWD
        {
            get
            {
                return GetValue("MESPW");
            }
        }
        /// <summary>
        /// 端口号
        /// </summary>
        /// <returns></returns>
        public static int GetPort
        {
            get
            {
                return Convert.ToInt32(GetValue("MESPort"));
            }
        }
        #endregion
    }
}

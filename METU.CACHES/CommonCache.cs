using System.Collections.Generic;

namespace METU.CACHES
{
    public class CommonCache
    {
        /// <summary>
        /// 页面间公用变量pageinfo
        /// </summary>
        public static string PAGEINFOKEY { get; set; }
        /// <summary>
        /// SESSIONID
        /// </summary>
        public static string SESSIONID { get; set; }
        /// <summary>
        /// 用户KEY值
        /// </summary>
        public static string APPKEY { set; get; }
        /// <summary>
        /// MSSQL链接字符串
        /// </summary>
        public static string MSSQLConnectionString { get; set; }
        /// <summary>
        /// MYSQL字符串
        /// </summary>
        public static string MYSQLConnectionString { get; set; }
        /// <summary>
        /// 缓存字符串
        /// </summary>
        public static string REDISConnectionString { get; set; }
        /// <summary>
        /// ORACLE连接字符串
        /// </summary>
        public static string ORACLEConnectionString { get; set; }
        /// <summary>
        ///单点登录地址
        /// </summary>
        public static string SSOURL { get; set; }
        /// <summary>
        ///日志地址
        /// </summary>
        public static string LogURL { get; set; }
        /// <summary>
        /// 服务接口API地址
        /// </summary>
        public static string SERVICEURL { get; set; }
        /// <summary>
        /// API地址
        /// </summary>
        public static string APIURL { get; set; }
        /// <summary>
        /// 配置中心地址
        /// </summary>
        public static string ConfigURL { get; set; }
        /// <summary>
        /// 网关地址
        /// </summary>
        public static string GateWayURL { get; set; }
        /// <summary>
        /// 二维码生成地址
        /// </summary>
        public static string BarCodeURL { get; set; }
        /// <summary>
        /// 管理地址
        /// </summary>
        public static string ManagerURL { get; set; }
        /// <summary>
        /// 算法地址
        /// </summary>
        public static string ArtURL { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public static string VideoURL { get; set; }
        /// <summary>
        /// 文档地址
        /// </summary>
        public static string DocURL { get; set; }
        /// <summary>
        /// 资源地址
        /// </summary>

        public static string ResourceURL { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>

        public static string ImguRL { get; set; }

        /// <summary>
        /// 平台ID
        /// </summary>
        public static string PlatID { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string ServerIP { get; set; }
        /// <summary>
        /// 服务路径
        /// </summary>
        public static string ServicePath { get; set; }

        /// <summary>
        /// APPBASEPATH
        /// </summary>
        public static string APPBASEPATH { get; set; }
        /// <summary>
        /// ApplicationBasePath
        /// </summary>
        public static string ApplicationBasePath { get; set; }

        /// <summary>
        /// 日志数据库
        /// </summary>
        public static string LogDBCOnnectionstring { get; set; }

        /// <summary>
        /// 配置信息集合
        /// </summary>

        public static Dictionary<string, object> ConfigCaches = new Dictionary<string, object>();

        /// <summary>
        /// 图片服务器地址
        /// </summary>
        public static string PicRequest { get; set; }

        /// <summary>
        /// 调试日志输出是否启用
        /// </summary>
        public static bool EnabledLog { get; set; }
        /// <summary>
        /// 调试日志是否启用
        /// </summary>
        public static bool DebugEnabled { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public static bool Enabled { get; set; }
        /// <summary>
        /// 是否启用多库
        /// </summary>
        public static bool MutiDBEnabled { get; set; }
        /// <summary>
        /// 是否启用读取文件
        /// </summary>
        public static bool ReadEnabled { get; set; }
        /// <summary>
        /// 是否启用写
        /// </summary>
        public static bool WriteEnabled { get; set; }
        /// <summary>
        /// 配置开关参数 
        /// </summary>
        public static Dictionary<string, bool> SwitchEnableds = new Dictionary<string, bool>();
    }
}

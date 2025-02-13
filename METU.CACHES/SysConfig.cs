namespace METU.CACHES
{

    public static class SysConfig
    {
        /// <summary>
        /// IP
        /// </summary>
        public static string IP { get; set; }
        /// <summary>
        /// 链接字符串
        /// </summary>
        public static string DBConnectionString { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public static string Port { get; set; }
        /// <summary>
        /// 是否启用全局日志
        /// </summary>
        public static bool LOGENABLED { get; set; }
        /// <summary>
        /// 是否启用DataBase数据库记录全局日志
        /// </summary>
        public static bool DBENABLED { get; set; }
        /// <summary>
        /// 默认日志文件名称
        /// </summary>
        public static string LOGFILE = "logger";
        /// <summary>
        /// 用户名称
        /// </summary>
        public static string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public static string UserPasswords { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public static string ServiceURL { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public static string Versions { get; set; }


    }
}

namespace METU.CACHES
{
    /// <summary>
    /// Added by tony  2017-10-18 系统文件路径配置类
    /// </summary>
    public static class BaseConfig
    {
        /// <summary>
        /// 获取模块的完整路径，包括文件名
        /// </summary>
        public static string Config_ModuleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        /// <summary>
        /// 获取和设置当前目录(该进程从中启动的目录)的完全限定目录。,字符串后面无"/"
        /// </summary>
        public static string Config_CurrentDirectory = System.Environment.CurrentDirectory
;
        /// <summary>
        /// 程序最后一次操作过的目录
        /// </summary>
        public static string Config_LastDirectory = System.IO.Directory.GetCurrentDirectory();
        /// <summary>
        /// 获取程序的基目录
        /// </summary>
        public static string Config_BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 应用程序的目录的名称,字符串后面无"/"
        /// </summary>
        public static string Config_ApplicationBase = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        /// <summary>
        /// 环境变量取值 
        /// </summary>
        /// <param name="ConfigVariable">系统变量</param>
        /// <returns>返回环境变量值</returns>
        public static string GetEnvironmentVariable(string ConfigVariable = null)
        {
            if (ConfigVariable == null) ConfigVariable = "TEMP";
            return System.Environment.GetEnvironmentVariable(ConfigVariable);
        }
         


    }
}

using System.Collections.Generic;

namespace METU.CACHES
{
    /// <summary>
    /// Windows Caches
    /// </summary>
    public static class WinCaches
    {
        /// <summary>
        /// ＤＬＬ路径缓存
        /// </summary>
        public static Dictionary<string, string> DLLPathCaches = new Dictionary<string, string>();
        /// <summary>
        /// ＷＣＦ插件ＤＬＬ缓存
        /// </summary>
        public static Dictionary<string, object> WCFDLLCaches = new Dictionary<string, object>();
        /// <summary>
        /// WEBSERVCIE 插件缓存
        /// </summary>
        public static Dictionary<string, object> WebServiceDLLCaches = new Dictionary<string, object>();
        /// <summary>
        /// Windows Caches
        /// </summary>
        public static Dictionary<string, object> AppDLLCaches = new Dictionary<string, object>();

    }
}

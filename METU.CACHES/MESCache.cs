using System;
using System.Collections.Generic;

namespace METU.CACHES
{
    public static class MESCache
    {
        /// <summary>
        /// Windows Caches
        /// </summary>
        public static Dictionary<string, object> AppDLLCaches = new Dictionary<string, object>();
        /// <summary>
        /// 应用全局缓存
        /// </summary>
        public static Dictionary<string, object> AppCaches = new Dictionary<string, object>();
        /// <summary>
        /// 方法地图Mapping
        /// </summary>
        public static Dictionary<string, Predicate<object>> AppMapping = new Dictionary<string, Predicate<object>>();
    }
}

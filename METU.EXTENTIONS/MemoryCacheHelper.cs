using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// MemoryCacheHelper
    /// </summary>
    public static class MemoryCacheHelper
    {/// <summary>
     /// 缓存帮助类
     /// </summary>

        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        ///缓存Key
        /// 
        public static bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return Cache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        ///缓存Key
        ///缓存Value
        ///滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）
        ///绝对过期时长
        /// 
        public static bool Set(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Cache.Set(key, value,
                new MemoryCacheEntryOptions().SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte));
            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        ///缓存Key
        ///缓存Value
        ///缓存时长
        ///是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）
        /// 
        public static bool Set(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Cache.Set(key, value,
                isSliding
                    ? new MemoryCacheEntryOptions().SetSlidingExpiration(expiresIn)
                    : new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiresIn));

            return Exists(key);
        }

        #region 删除缓存

        /// <summary>
        /// 删除缓存
        /// </summary>
        ///缓存Key
        /// 
        public static void Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Cache.Remove(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// 
        public static void RemoveAll(IEnumerable keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            keys.ToList<object>().ForEach(item => Cache.Remove(item));
        }
        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取缓存
        /// </summary>
        ///缓存Key
        /// 
        public static T Get<T>(string key) where T : class
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return Cache.Get(key) as T;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        ///缓存Key
        /// 
        public static object Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        ///缓存Key集合
        /// 
        public static IDictionary<string, object> GetAll(IEnumerable keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();
            keys.ToList<object>().ForEach(item => dict.Add(item.ToString(), Cache.Get(item)));
            return dict;
        }
        #endregion

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public static void RemoveCacheAll()
        {
            var l = GetCacheKeys();
            foreach (var seee in l)
            {
                Remove(seee.ToString());
            }
        }

        /// <summary>
        /// 删除匹配到的缓存
        /// </summary>
        ///
        /// 
        public static void RemoveCacheRegex(string pattern)
        {
            IList l = SearchCacheRegex(pattern);
            foreach (var s in l)
            {
                Remove(s.ToString());
            }
        }

        /// <summary>
        /// 搜索 匹配到的缓存
        /// </summary>
        ///
        /// 
        public static IList SearchCacheRegex(string pattern)
        {
            var cacheKeys = GetCacheKeys();
            var l = cacheKeys.Where(k => Regex.IsMatch(k.ToString(), pattern)).ToList();
            return l.AsReadOnly();
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// 
        public static List<object> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = Cache.GetType().GetField("_entries", flags).GetValue(Cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<object>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}

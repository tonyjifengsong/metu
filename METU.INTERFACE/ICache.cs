using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        object Get(string key);
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, object value);
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="ExpirationTime">绝对过期时间(分钟)</param>
        void Add(string key, object value, int ExpirationTime = 20);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ExpirationTime"></param>
        void Replace(string key, object value, int ExpirationTime = 20);
        /// <summary>
        /// 清理所有缓存
        /// </summary>
        void Clear();
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddAsync(string key, object value);
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="ExpirationTime">绝对过期时间(分钟)</param>
        void AddAsync(string key, object value, int ExpirationTime = 20);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        void RemoveAsync(string key);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ExpirationTime"></param>
        void ReplaceAsync(string key, object value, int ExpirationTime = 20);
    }
}

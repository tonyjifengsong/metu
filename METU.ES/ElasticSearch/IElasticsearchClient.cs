using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.ElasticSearch
{
    /// <summary>
    /// ES客户端
    /// </summary>
    public interface IElasticsearchClient
    {
        /// <summary>
        /// 是否存在指定索引
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string indexName);

        /// <summary>
        /// 添加索引。不映射
        /// </summary>
        /// <param name="indexName">索引名</param>
        Task AddAsync(string indexName);

        /// <summary>
        /// 添加索引。自动映射实体属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        Task AddAsync<T>(string indexName) where T : class;

        /// <summary>
        /// 添加索引。自动映射实体属性并赋值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task AddAsync<T>(string indexName, T entity) where T : class;

        /// <summary>
        /// 更新索引。
        /// 由于是普通的简单更新，当ID已经存在时，则会更新文档，所以这里直接调用index方法（复杂方法待研究）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="indexName">索引名</param>
        /// <param name="entity">实体</param>
        Task UpdateAsync<T>(string indexName, T entity) where T : class;
    }
}

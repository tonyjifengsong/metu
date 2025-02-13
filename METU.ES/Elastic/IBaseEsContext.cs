using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.Elastic
{
    /// <summary>
    /// 接口限定
    /// </summary>
    public interface IBaseEsContext {

        /// <summary>
        /// 索引名称
        /// </summary>
         string IndexName { get;}
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
         bool Insert<T>(string IndexName, T obj) where T : class;

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
         bool InsertMany<T>(string IndexName, List<T> tList) where T : class;

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
         long GetTotalCount<T>(string IndexName) where T : class;
        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <returns></returns>
         bool DeleteById<T>(string IndexName, string id) where T : class;
        
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
         List<ESModel> Get(string IndexName, string id, int pageIndex, int pageSize);
        /// <summary>
        /// 获取所有地址
        /// </summary>
        /// <returns></returns>
         List<ESModel> GetAll(string IndexName);
        /// <summary>
        /// 删除指定城市的数据
        /// </summary>
        /// <param name="city">ID</param>
        /// <returns></returns>
          bool DeleteByQuery(string IndexName, string city);
    }
}

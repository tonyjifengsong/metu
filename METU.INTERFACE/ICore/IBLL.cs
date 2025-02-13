using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.ICore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBLL<T> where T : class, new()
    {/// <summary>
     /// 批量添加任意对象
     /// </summary>
     /// <typeparam name="V"></typeparam>
     /// <param name="list"></param>
     /// <returns></returns>
        bool AddTBat<V>(List<V> list) where V : class, IEntity, new();
        /// <summary>
        /// 通过主键值获取实体
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        T GetModeleByKey(string PID);
        /// <summary>
        /// 通过条件值获取实体
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        T GetModel(T Param);

        /// <summary>
        /// 通过主键值删除实体返回执行结果
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        bool DeleteeByKey<VT>(string PID) where VT : class, IEntity, new();

        /// <summary>
        /// 通过主键值列表删除实体返回执行结果
        /// </summary>
        /// <param name="pidlist"></param>
        /// <returns></returns>
        bool DeleteeByKeyList<V>(List<string> pidlist) where V : class, IEntity, new();
        /// <summary>
        /// 通过条件删除实体，返回执行结果
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        bool Delete(T Param);
        /// <summary>
        /// 返回列表数据
        /// </summary>
        /// <param name="model">实体参数</param>
        /// <param name="pageindex">页号</param>
        /// <param name="pagesize">页面大小</param>
        /// <returns></returns>
        List<T> GetList(T model, int pageindex = 1, int pagesize = 15);
        /// <summary>
        /// 更新数据，并返回更新后实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(T model);
        /// <summary>
        /// 反题更新数据，并返回更执行结果
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool UpdateBat(List<T> list);
        /// <summary>
        /// 添加实体，并返回执行结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Add(T model);
        /// <summary>
        /// 添加实体，并返回执行结果
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool Saves(List<T> param);
        /// <summary>
        /// 批量添加实体，并返回执行结果
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool AddBat(List<T> list);
        /// <summary>
        /// 执行SQL语句，并返回影响行数
        /// </summary>
        /// <param name="sqlstr">原始SQL语句</param>
        /// <returns></returns>
        int Execute(string sqlstr);


        /// <summary>
        /// 通过SQL返回表格
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string sqlQuery);
        /// <summary>
        /// 通过SQL返回多张表
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string sqlQuery);
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="SQLStringList"></param>
        void ExecuteSqlTrans(IList<string> SQLStringList = null);
        /// <summary>
        /// 执行带参数SQL语句
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        int ExecuteSql(string SQLString, DbParameter[] cmdParms = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        bool Remove(T Param);


        /// <summary>
        /// 通过主键值删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="PID"></param>
        /// <returns></returns>



        bool RemoveByKey(string PID);
        /// <summary>
        ///  批量删除软删除LIST
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool RemoveByKeyList(List<string> list);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pagination"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<T> FindList(Expression<Func<T, bool>> predicate, Pager pagination, out int count);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        List<T> FindList(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        List<V> FindList<V>(Expression<Func<V, bool>> whereLambda) where V : class, IEntity, new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        T FindListFirst(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 通过表名获取表中字段的详细信息
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        object GetPageControlInfoByTableName(string tablename);

        /// <summary>
        /// 添加任意实体
        /// </summary>
        /// <typeparam name="v"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Add<v>(v model) where v : class, IEntity, new();
        /// <summary>
        /// 批量更新任意实体
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        bool UpdateTBat<V>(List<V> list) where V : class, IEntity, new();

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T FindById(dynamic id);

        /// <summary>
        /// 获取默认一条数据，没有则为NULL
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> whereLambda = null);
        /// <summary>
        /// 通用保存方法(添加删除修改同时处理)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool SaveT<V>(List<V> param) where V : class, IEntity, new();

        /// <summary>
        /// 带条件查询获取数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetAllIQueryable(Expression<Func<T, bool>> whereLambda = null);
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="whereLambd"></param>
        /// <returns></returns>
        int GetCount(Expression<Func<T, bool>> whereLambd = null);

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="whereLambd"></param>
        /// <returns></returns>
        bool Any(Expression<Func<T, bool>> whereLambd);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rows">总条数</param>
        /// <param name="orderBy">排序条件（一定要有）</param>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <param name="isOrder">是否是Order排序</param>
        /// <returns></returns>
        List<T> Page<TKey>(int pageIndex, int pageSize, out int rows, Expression<Func<T, TKey>> orderBy, Expression<Func<T, bool>> whereLambda = null, bool isOrder = true);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rows">总条数</param>
        /// <param name="ordering">排序条件（一定要有）</param>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <returns></returns>
        List<T> Page(int pageIndex, int pageSize, out int rows, Expression<Func<T, T>> ordering, Expression<Func<T, bool>> whereLambda = null);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rows"></param>
        /// <param name="cons"></param>
        /// <returns></returns>
        List<T> GetPageByContains(int pageIndex, int pageSize, out int rows, List<Condition> cons);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cons"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        List<T> GetPageByDics(List<Condition> cons, out int rows);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rows"></param>
        /// <param name="cons"></param>
        /// <returns></returns>
        List<T> GetPageByParams(int pageIndex, int pageSize, out int rows, object cons);
       
      
    }
}

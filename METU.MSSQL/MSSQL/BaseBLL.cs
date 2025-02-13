using METU.CACHES;
using METU.Core;
using METU.INTERFACE;
using METU.INTERFACE.ICore;
using METU.MODEL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace METU.MSSQL.MSSQL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseBLL<T> : IBLL<T> where T : class, IEntity, new()
    {
       
        /// <summary>
        /// 
        /// </summary>
        private CoreBLL bll = null;
        /// <summary>
        /// 
        /// </summary>
        string namespaceconfig = null;
        /// <summary>
        /// 
        /// </summary>
        public BaseDbContext dbh = null;
        /// <summary>
        /// 
        /// </summary>
        public DBHelpers db = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonurl"></param>
        /// <param name="ConfigKey"></param>
        void AddAutoReadDBConfig(string jsonurl = null, string ConfigKey = null)
        {


            ConfHelper conf = null;

            conf = new ConfHelper(jsonurl);


            List<DBConfigString> list = new List<DBConfigString>();
            if (ConfigKey == null)
            {
                list = conf.ReadList<DBConfigString>("Connections");
            }
            else
            {
                if (ConfigKey.ToString().Trim().Length == ConstNum.Zero)
                {
                    list = conf.ReadList<DBConfigString>("Connections");
                }
                else
                {
                    list = conf.ReadList<DBConfigString>(ConfigKey);
                }
            }

            if (list != null) foreach (var itm in list)
                {
                    if (itm.Database != null) if (itm.Database.ToString().Trim().Length > ConstNum.Zero) if (itm.ConnString != null) if (itm.ConnString.ToString().Trim().Length > ConstNum.Zero)
                                {
                                    bool addflag = true;
                                    foreach (var citm in CommonCache.ConfigCaches)
                                    {
                                        if (citm.Key.ToUpper().Trim() == itm.Database.ToUpper().Trim())
                                        {
                                            addflag = false;
                                            break;
                                        }
                                    }
                                    if (addflag)
                                    {
                                        if (itm.Database.ToLower() == "connnectionstring" || itm.Database.ToLower() == "MSSQLConnectionString".ToLower()) CommonCache.MSSQLConnectionString = itm.ConnString;
                                        CommonCache.ConfigCaches.Add(itm.Database.ToUpper(), itm.ConnString);
                                    }
                                }

                }


        }
        /// <summary>
        ///  基类BLL
        /// </summary>
        public BaseBLL()
        {
            // AddAutoReadDBConfig();

            namespaceconfig = typeof(T).Namespace.Replace(".", "").ToUpper();
            if (CommonCache.ConfigCaches != null && CommonCache.ConfigCaches.Count() > ConstNum.Zero && CommonCache.ConfigCaches.ContainsKey(namespaceconfig))
            {
                if (CommonCache.ConfigCaches[namespaceconfig] != null)
                {
                    if (namespaceconfig.Trim().ToString().Length > ConstNum.Zero)
                    {
                        bll = new CoreBLL(namespaceconfig);
                        dbh = bll.dbh;
                        db = bll.db;
                    }
                    else
                    {
                        bll = new CoreBLL();
                        dbh = bll.dbh;
                        db = bll.db;
                    }

                }
                else
                {
                    bll = new CoreBLL();
                    dbh = bll.dbh;
                    db = bll.db;
                }
            }
            else
            {
                bll = new CoreBLL();
                dbh = bll.dbh;
                db = bll.db;
            }
        }
        /// <summary>
        /// 获取页面
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public object GetPageControlInfoByTableName(string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) return null;
            if (tablename.ToString().Trim().Length < 1) return null;
            string sqlstr = BaseSQL.SQL_GET_PAGE_LIST_COMMON;
            sqlstr = sqlstr.Replace("{0}", tablename);
            try
            {
                var corebll = new CoreBLL();
                var dt = corebll.db.ExecuteDataTable(sqlstr.ToLower());
                return dt;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return new object();
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Add(T model)
        {
            int result = 0;


            try
            {
                //model.id = Guid.NewGuid().ToString();
                
               bll.dbh.Add(model);
                try
                {
                    result = bll.dbh.SaveChanges();
                }
                catch (Exception ex)
                {
                    MSGBox.EXMessage(ex.Message);
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.InnerException.Message);
                return false;
            }

            return result > ConstNum.Zero;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Register(T model)
        {
            int result = 0;
            try
            {
               
                bll.dbh.Add(model);
                result = bll.dbh.SaveChanges();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.InnerException.Message);
                return false;
            }

            return result > ConstNum.Zero;
        }



        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool AddBat(List<T> list)
        {
            if (list == null) return false;

            var model = list.FirstOrDefault();
            int result = 0;

            try
            { 
                bll.dbh.Set<T>().AddRange(list);
                result = bll.dbh.SaveChanges();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);

                return false;
            }

            return result > ConstNum.Zero;
        }

        /// <summary>
        /// 通用批量添加任意实体
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool AddTBat<V>(List<V> list) where V : class, IEntity, new()
        {
            var model = list.FirstOrDefault();
            int result = 0;
            try
            { 
                bll.dbh.Set<V>().AddRange(list);
                result = bll.dbh.SaveChanges();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);

                return false;
            }

            return result > ConstNum.Zero;
        }
        /// <summary>
        ///  软删除
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public virtual bool Delete(T Param)
        {
            int result = 0;

            try
            {
                Param = GetModeleByKey(Param.id);
                if (Param.id != null)
                { 
                    bll.dbh.Update(Param);
                    result = bll.dbh.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);

                return false;
            }

            return result > ConstNum.Zero;
        }
        /// <summary>
        /// 删除类
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="PID"></param>
        /// <returns></returns>
        public virtual bool DeleteeByKey<V>(string PID) where V : class, IEntity, new()
        {
            int result = 0;


            try
            {

                T sm = new T();

                T md = FindList<T>(p => p.id == PID).FirstOrDefault();
                if (md != null)
                { 
                   
                    dbh.Update(md);
                }
                else
                {
                    dbh.Dispose();
                    MSGBox.EXMessage("待删除数据不存在！");
                }


                result = dbh.SaveChanges();
                return result > ConstNum.Zero;


            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);

                return false;
            }

            return result > ConstNum.Zero;
        }/// <summary>
         ///  执行sql语句
         /// </summary>
         /// <param name="sqlstr"></param>
         /// <returns></returns>

        public virtual int Execute(string sqlstr)
        {
            try
            {
                return bll.db.Execute(sqlstr);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// 返回DATASET 执行SQL
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(string sqlQuery)
        {
            try
            {
                return bll.db.ExecuteDataSet(sqlQuery);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public virtual DataTable ExecuteDataTable(string sqlQuery)
        {
            try
            {
                return bll.db.ExecuteDataTable(sqlQuery);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///  SQL语句
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string SQLString, DbParameter[] cmdParms = null)
        {
            try
            {
                return bll.db.ExecuteSql(SQLString, cmdParms);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQLStringList"></param>
        public virtual void ExecuteSqlTrans(IList<string> SQLStringList = null)
        {
            try
            {
                bll.db.ExecuteSqlTrans(SQLStringList);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);

            }
        }
        /// <summary>
        /// 获取列表 代分页
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public virtual List<T> GetList(T model, int pageindex = 1, int pagesize = 15)
        {
            try
            {
                model.validatedIsNull();
               
                    var rs = bll.dbh.Set<T>().AsNoTracking().ToList<T>().ToList<T>();
                    return rs;
                
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>

        public virtual List<T> AdminGetList(T model, int pageindex = 1, int pagesize = 15)
        {
            try
            {
                 
                    var rs = bll.dbh.Set<T>().AsNoTracking().ToList<T>().ToList<T>();
                    return rs;
                
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
        /// <summary>
        ///  获取实体，查询实体
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public virtual T GetModel(T Param)
        {
            try
            {

                return bll.dbh.Set<T>().AsNoTracking().ToList().FirstOrDefault<T>();
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        public virtual T GetModeleByKey(string PID)
        {
            try
            {

                return bll.dbh.Set<T>().AsNoTracking().ToList().Where(p => p.id == PID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(T model)
        {
            int result = 0;


            try
            { 
                bll.dbh.Update(model);
                result = bll.dbh.SaveChanges();


            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);

                return false;
            }

            return result > ConstNum.Zero;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool UpdateBat(List<T> list)
        {
            var model = list.FirstOrDefault();
            int result = 0;

            try
            { 
                foreach (var mdl in list)
                {
                    EntityEntry entry = dbh.Entry(mdl);
                    entry.State = EntityState.Modified;
                }
                bll.dbh.Set<T>().AttachRange(list);
                result = bll.dbh.SaveChanges();


            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);

                return false;
            }

            return result > ConstNum.Zero;
        }
        /// <summary>
        ///  批次更新 LIST
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool UpdateTBat<V>(List<V> list) where V : class, IEntity, new()
        {
            var model = list.FirstOrDefault();
            int result = 0;
            try
            {
                
                bll.dbh.Set<V>().UpdateRange(list);
                result = bll.dbh.SaveChanges();


            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                return false;
            }

            return result > ConstNum.Zero;
        }
 
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="Pager"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> FindList(Expression<Func<T, bool>> predicate, INTERFACE.Pager pager, out int count)
        {
            try
            {
                if (pager.pagesize < ConstNum.MINPAGE) pager.pagesize = ConstNum.PAGESIZE;
                if (pager.pagesize == ConstNum.Zero) pager.pagesize = ConstNum.PAGESIZE;
                if (pager.pageindex == ConstNum.Zero) pager.pageindex = ConstNum.One;
                IQueryable<T> tempData = dbh.Set<T>().AsNoTracking().Where(predicate) ;
                if (!string.IsNullOrEmpty(pager.sidx))
                {
                    bool isAsc = false;
                    if (!string.IsNullOrEmpty(pager.sord))
                    {
                        isAsc = pager.sord.ToLower() == "asc" ? true : false;
                    }
                    var _order = pager.sidx.Split(',');
                    MethodCallExpression resultExp = null;
                    count = dbh.Set<T>().AsNoTracking().Where(predicate).Count();
                    foreach (string item in _order)
                    {
                        string _orderPart = item;
                        _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                        string[] _orderArry = _orderPart.Split(' ');
                        string _orderField = _orderArry[0];
                        bool sort = isAsc;
                        if (_orderArry.Length == 2)
                        {
                            isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                        }
                        var parameter = Expression.Parameter(typeof(T), "t");
                        var property = typeof(T).GetProperty(_orderField);
                        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                        var orderByExp = Expression.Lambda(propertyAccess, parameter);
                        resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                    }

                    tempData = tempData.Provider.CreateQuery<T>(resultExp).AsQueryable();
                }
                count = tempData.Count();

                return tempData.Skip<T>(pager.pagesize * (pager.pageindex - 1)).Take<T>(pager.pagesize).AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                count = 0;
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                return new List<T>();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="Pager"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<V> FindList<V>(Expression<Func<V, bool>> predicate, INTERFACE.Pager pager, out int count) where V : class, IEntity, new()
        {
            try
            {
                if (pager.pagesize < ConstNum.Ten) pager.pagesize = ConstNum.PAGESIZE;
                if (pager.pagesize == ConstNum.Zero) pager.pagesize = ConstNum.PAGESIZE;
                if (pager.pageindex == ConstNum.Zero) pager.pageindex = ConstNum.One;
                IQueryable<V> tempData = dbh.Set<V>().AsNoTracking().Where(predicate);
                if (!string.IsNullOrEmpty(pager.sidx))
                {
                    bool isAsc = false;
                    if (!string.IsNullOrEmpty(pager.sord))
                    {
                        isAsc = pager.sord.ToLower() == "asc" ? true : false;
                    }
                    var _order = pager.sidx.Split(',');
                    MethodCallExpression resultExp = null;
                    count = dbh.Set<V>().AsNoTracking().Where(predicate).Count();
                    foreach (string item in _order)
                    {
                        string _orderPart = item;
                        _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                        string[] _orderArry = _orderPart.Split(' ');
                        string _orderField = _orderArry[0];
                        bool sort = isAsc;
                        if (_orderArry.Length == 2)
                        {
                            isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                        }
                        var parameter = Expression.Parameter(typeof(V), "t");
                        var property = typeof(V).GetProperty(_orderField);
                        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                        var orderByExp = Expression.Lambda(propertyAccess, parameter);
                        resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(V), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                    }

                    tempData = tempData.Provider.CreateQuery<V>(resultExp).AsQueryable();
                }
                count = tempData.Count();
                return tempData.Skip<V>(pager.pagesize * (pager.pageindex - 1)).Take<V>(pager.pagesize).AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                count
                      = 0;
            }
            return new List<V>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderbyLambda"></param>
        /// <param name="Pager"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<T> LoadPageItems<Tkey>(Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, INTERFACE.Pager pager, out int total)
        {
            try
            {
                if (pager.pagesize < ConstNum.Ten) pager.pagesize = ConstNum.PAGESIZE;
                if (pager.pagesize == ConstNum.Zero)
                {
                    if (pager.pagesize < ConstNum.MINPAGE) pager.pagesize = ConstNum.PAGESIZE;
                }
                if (pager.pageindex == ConstNum.Zero)
                {
                    pager.pageindex = ConstNum.One;
                }
                bool isAsc = true;
                if (!string.IsNullOrWhiteSpace(pager.sord))

                {
                    isAsc = pager.sord.ToLower() == "asc" ? true : false;
                }
                total = dbh.Set<T>().AsNoTracking().Where(whereLambda).Count();
                if (isAsc)
                {
                    var temp = dbh.Set<T>().AsNoTracking().Where(whereLambda)
                                 .OrderBy<T, Tkey>(orderbyLambda)
                                 .Skip(pager.pagesize * (pager.pageindex - 1))
                                 .Take(pager.pagesize);
                    return temp.AsQueryable().ToList();
                }
                else
                {
                    var temp = dbh.Set<T>().AsNoTracking().Where(whereLambda)
                               .OrderByDescending<T, Tkey>(orderbyLambda)
                               .Skip(pager.pagesize * (pager.pageindex - 1))
                               .Take(pager.pagesize);
                    return temp.AsQueryable().ToList();
                }
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                total = 0;
                return new List<T>();
            }
        }


        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> FindList(Expression<Func<T, bool>> whereLambda)
        {
            try
            {
                return bll.dbh.Set<T>().AsNoTracking().Where(whereLambda).ToList();
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T FindListFirst(Expression<Func<T, bool>> whereLambda)
        {
            try
            {
                return bll.dbh.Set<T>().AsNoTracking().Where(whereLambda).FirstOrDefault();
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
        /// <summary>
        ///  
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<V> FindList<V>(Expression<Func<V, bool>> whereLambda) where V : class, IEntity, new()
        {
            try
            {
                return bll.dbh.Set<V>().AsNoTracking().Where(whereLambda).ToList();
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
 

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="v"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add<v>(v model) where v : class, IEntity, new()
        {
            try
            { 
                bll.dbh.Add(model);
                var result = bll.dbh.SaveChanges();

               


                return result > ConstNum.Zero;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update<V>(V model) where V : class, IEntity, new()
        {
            try
            {
                int result = 0;

                try
                { 
                    bll.dbh.Update(model);
                    result = bll.dbh.SaveChanges();


                }
                catch (Exception ex)
                {
                     //FileHelper.Writelog(ex);

                    return false;
                }

                return result > ConstNum.Zero;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 根据ID删除实体软删除
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="pidlist"></param>
        /// <returns></returns>
        public bool DeleteeByKeyList<V>(List<string> pidlist) where V : class, IEntity, new()
        {
            try
            {
                bool result = true;
                foreach (var itm in pidlist)
                {
                    V md = FindList<V>(p => p.id == itm).FirstOrDefault();
                    if (md != null)
                    { 
                       
                        dbh.Update(md);
                    }
                    else
                    {
                        dbh.Dispose();
                        MSGBox.EXMessage("待删除数据不存在！");
                    }

                }
                result = dbh.SaveChanges() > ConstNum.Zero;
                return result;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }



        #region MyRegion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public bool Remove(T Param)
        {
            try
            {
                Param.validatedIsNull();
                Param.id.validatedIsGUID();
                // dbh.Removeentity(Param);
                
                dbh.Update(Param);
                int rs = dbh.SaveChanges();
                return rs > ConstNum.Zero;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        public bool RemoveByKey(string PID)
        {
            try
            {
                var itm = GetModeleByKey(PID);
                // dbh.Removeentity(itm);
               
                dbh.Update(itm);
                int rs = dbh.SaveChanges();
                return rs > ConstNum.Zero;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool RemoveByKeyList(List<string> list)
        {
            try
            {
                foreach (var itm in list)
                {
                    var md = GetModeleByKey(itm);
                   
                    dbh.Update(md);
                    // dbh.Removeentity(md);
                }
                int rs = dbh.SaveChanges();
                return rs > ConstNum.Zero;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }
        public List<string> DropDownList(string config = null)
        {
            try
            {
                if (config == null) config = "TDB";
                if (config.Trim().ToString().Length == ConstNum.Zero) config = "TDB";
                List<string> list = new List<string>();
                var sbll = new CoreBLL(config);
                var dt = sbll.db.ExecuteDataTable("SELECT  [TABLE_NAME] FROM  [INFORMATION_SCHEMA].[TABLES]");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i][0].ToString());

                }
                return list;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
        public List<string> CurrentDropDownList()
        {

            List<string> list = new List<string>();
            try
            {
                var dt = bll.db.ExecuteDataTable("SELECT  [TABLE_NAME] FROM  [INFORMATION_SCHEMA].[TABLES]");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i][0].ToString());

                }
                return list;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }
        /// <summary>
        ///  保存的约定，如果不存在ID就是新增，如果存在ID就是修改，如果idelete=1 就是删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Saves(List<T> param)
        {
            try
            {

                foreach (var itm in param)
                {
                    if (string.IsNullOrEmpty(itm.id))
                    {
                            dbh.Add(itm);
                    }
                    else
                    {
                        
                            dbh.Update(itm);
                        
                    }
                }
                var rs = dbh.SaveChanges();
                return rs > ConstNum.Zero;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 通用保存方法(添加删除修改同时处理)保存的约定，如果不存在ID就是新增，如果存在ID就是修改，如果idelete=1 就是删除
        /// </summary>
        /// <typeparam name="V">参数类型参数</typeparam>
        /// <param name="param">实体参数</param>
        /// <returns></returns>
        public bool SaveT<V>(List<V> param) where V : class, IEntity, new()
        {
            try
            {
                foreach (var itm in param)
                {
                    if (string.IsNullOrEmpty(itm.id))
                    {
                        
                        dbh.Add(itm);
                    }
                    else
                    { 
                            dbh.Update(itm);
                        
                    }
                }
                var rs = dbh.SaveChanges();
                return rs > ConstNum.Zero;
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }
        #endregion



        #region 查询及分页相关方法
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindById(dynamic id)
        {
            try
            {
                return dbh.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取默认一条数据，没有则为NULL
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> whereLambda = null)
        {
            try
            {
                if (whereLambda == null)
                {
                    return dbh.Set<T>().AsNoTracking().FirstOrDefault();
                }
                return dbh.Set<T>().AsNoTracking().FirstOrDefault(whereLambda);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }





        /// <summary>
        /// 带条件查询获取数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetAllIQueryable(Expression<Func<T, bool>> whereLambda = null)
        {
            try
            {
                return whereLambda == null ? dbh.Set<T>().AsNoTracking() : dbh.Set<T>().AsNoTracking().Where(whereLambda);
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="whereLambd"></param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> whereLambd = null)
        {
            try
            {
                return whereLambd == null ? dbh.Set<T>().AsNoTracking().Count() : dbh.Set<T>().AsNoTracking().Where(whereLambd).Count();
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="whereLambd"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> whereLambd)
        {
            try
            {
                return dbh.Set<T>().AsNoTracking().Where(whereLambd).Any();
            }
            catch (Exception ex)
            {
                MSGBox.EXMessage(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageindex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rows">总条数</param>
        /// <param name="orderBy">排序条件（一定要有）</param>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <param name="isOrder">是否是Order排序</param>
        /// <returns></returns>
        public List<T> Page<TKey>(int pageindex, int pageSize, out int rows, Expression<Func<T, TKey>> orderBy, Expression<Func<T, bool>> whereLambda = null, bool isOrder = true)
        {
            try
            {
                if (pageSize < ConstNum.MINPAGE) pageSize = ConstNum.PAGESIZE;
                IQueryable<T> data = isOrder ?
                    dbh.Set<T>().AsNoTracking().OrderBy(orderBy) :
                    dbh.Set<T>().AsNoTracking().OrderByDescending(orderBy);

                if (whereLambda != null)
                {
                    data = data.Where(whereLambda);
                }
                rows = data.Count();
                return data.Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                rows = 0;
                return new List<T>();
            }
        }
        //public List<T> Pages<TKey>(int pageindex, int pageSize, out int rows, List<Condition>cons)
        //{
        //    IQueryable<T> data = 
        //        dbh.Set<T>().AsNoTracking().QueryConditions(cons);            
        //    rows = data.Count();
        //    return data.Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
        //}
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageindex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="rows">总条数</param>
        /// <param name="ordering">排序条件（一定要有）</param>
        /// <param name="whereLambda">查询添加（可有，可无）</param>
        /// <returns></returns>
        public List<T> Page(int pageindex, int pagesize, out int rows, Expression<Func<T, T>> ordering, Expression<Func<T, bool>> whereLambda = null)
        {
            try
            {
                if (pagesize < ConstNum.MINPAGE) pagesize = ConstNum.PAGESIZE;

                // 分页 一定注意： Skip 之前一定要 OrderBy
                IQueryable<T> data = dbh.Set<T>().AsNoTracking().OrderBy(ordering);
                if (whereLambda != null)
                {
                    data = data.Where(whereLambda);
                }
                rows = data.Count();
                return data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList<T>();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                rows = 0;
                return new List<T>();
            }
        }
        /// <summary>
        /// 
        /// </summary>

        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="rows"></param>
        /// <param name="cons"></param>
        /// <returns></returns>
        public List<T> GetPageByContains(int pageindex, int pagesize, out int rows, List<Condition> cons)
        {
            try
            {
                if (pagesize < ConstNum.MINPAGE) pagesize = ConstNum.PAGESIZE;

                IQueryable<T> data =
                dbh.Set<T>().AsNoTracking().QueryConditions(cons);
                rows = data.Count();
                return data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                rows = 0;
                return new List<T>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cons"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<T> GetPageByDics(List<Condition> cons, out int rows)
        {
            try
            {
                T model = new T();
                List<Condition> qparam = new List<Condition>();
                int pageindex = 1;
                int pagesize = 500;
                foreach (Condition itm in cons)
                {
                    if (itm.Key.ToLower() == "pagesize")
                    {
                        try
                        {
                            pagesize = (int)itm.Value;
                            cons.Remove(itm);
                        }
                        catch (Exception ex)
                        {
                             //FileHelper.Writelog(ex);

                        }
                        if (pagesize < 10) pagesize = 500;
                    }
                    if (itm.Key.ToLower() == "pageindex")
                    {
                        try
                        {
                            pageindex = (int)itm.Value;
                            cons.Remove(itm);
                        }
                        catch (Exception ex)
                        {
                             //FileHelper.Writelog(ex);

                        }
                        if (pageindex < 1) pageindex = 1;
                    }

                    if (model.GetType().GetProperties().Where(p => p.Name.ToLower() == itm.Key.ToLower()).Count() > ConstNum.Zero)
                        qparam.Add(itm);

                }
                if (pagesize < ConstNum.MINPAGE) pagesize = ConstNum.PAGESIZE;

                IQueryable<T> data =
                dbh.Set<T>().AsNoTracking().QueryConditions(qparam);
                rows = data.Count();
                return data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                rows = 0;
                return new List<T>();
            }
        }
        /// <summary>
        /// 
        /// </summary>

        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="rows"></param>
        /// <param name="cons"></param>
        /// <returns></returns>
        public List<T> GetPageByParams(int pageindex, int pagesize, out int rows, object cons)
        {
            try
            {
                IQueryable<T> data =
                    dbh.Set<T>().AsNoTracking().QueryConditions(cons.ToQueryConditions());
                rows = data.Count();
                return data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex);
                MSGBox.EXMessage(ex.Message);
                rows = 0;
                return new List<T>();
            }
        }




        #endregion

        #region extention
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public dynamic DynCore(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }

        /// <summary>
        ///  执行业通用务逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public dynamic Core(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();



            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }

        /// <summary>
        ///  执行业通用务逻辑,返回整型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>



        public int Coreint(Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return -1;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return 0;
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return 0;
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return rsdt.Rows.Count;
                                }
                                return -1;
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt;
                            }

                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return rsdt.Rows.Count;
                            }



                        }

            return -1;
        }
        /// <summary>
        /// 执行业通用务逻辑,返回Boolean
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public bool Corebool(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return false;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return false;
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return false;
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return rsdt.Rows.Count > 0;
                                }
                                return false;
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return rsdt.Rows.Count > 0;
                            }



                        }

            return false;
        }
        /// <summary>
        ///  执行业通用务逻辑,返回对象数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object Coreobject(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        ///  执行业通用务逻辑,返回统一格式result
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result CoreBasic(Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="servicename"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result CoreService(string servicename, Dictionary<string, object> dic)
        {

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.ToString().Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.ToString().Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 执行业通用务逻辑,返回统一格式result
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result CoreList(int pageindex, int pagesize, Dictionary<string, string> dic = null)
        {

            string servicename = dic.ServiceName();

            Result rs = new Result();
            rs.Data.Add("pageindex", pageindex);
            rs.Data.Add("pagesize", pagesize);

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result DoCore(Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }

                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }

                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 根据数据库中的表名获取页面控件的基本信息（获取实体字段及描述）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tname">数据库中表名</param>
        /// <returns></returns>
        public dynamic Reports(string tname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGE_LIST_COMMON;
            sqlstr = sqlstr.Replace("{0}", tname);


            return dbh.ExecuteDataTable(sqlstr.ToLower());
        }

        /// <summary>
        /// 根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中表名}/getlist
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中表名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>

        public object configServices(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "getconfig";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        /// <returns></returns>
        public object PageInfo(string methodname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGECONTROL_LIST;
            sqlstr = sqlstr.Replace("{0}", methodname);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return dbh.ExecuteDataTable(sqlstr.Str2SQL());
        }
        /// <summary>
        /// [页面控件信息内容保存到后端] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；后缀：save
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicessave(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "save";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// [根据页面控件信息获取对应的数据]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesgets(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "get";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// [列表]根据数据库中的表名;获取表中的数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServices(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "getlist";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }


        /// <summary>
        /// [添加] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesadd(Dictionary<string, string> dic)
        {

            string methodname = dic.MethodName();
            methodname = methodname + "add";
            dic.AddMethodName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        ///  [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesedit(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "edit";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesupdate(Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "update";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除（逻辑删除）指定数据；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public object sysconfigServicesdelete(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "delete";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除(物理删除)指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
        public object sysconfigServicesremovebykey(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "removebykey";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesget(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "gets";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public object sysconfigServiceslist(Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "list";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// 根据数据库配置，服务配置 执行对应的业务逻辑；地址组合：{数据库配置名}APP/{数据库中表SysPageService中字段servicename服务名}Service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object Services(Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "service";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// 多ＳＱＬ语句执行，非事务；服务名：{ctrl}bntservice
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result BatServiceNotTransRecords(string ctrl, Dictionary<string, object> model)
        {
            var dic = model.ToDictionary();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}bntservice'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = dbh.ExecuteDataTable(sqlstr.ToLower());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero)
                {
                    List<string> sqltrans = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][0] != null) if (dt.Rows[i][0].ToString().Trim().Length > 10)
                            {
                                sqlstr = dt.Rows[i][0].ToString();

                                if (dic != null) if (dic.Count > ConstNum.Zero)
                                    {
                                        foreach (var itm in dic)
                                        {
                                            if (itm.Value != null)
                                            {
                                                sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }

                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSQL(sqlstr.Str2SQL());


                                if (rsdt < 0)
                                {
                                    result = "业务执行失败！";
                                }


                            }
                    }



                }


            return new Result(result);

        }


        /// <summary>
        /// SQL事务执行；服务名：{ctrl}sqlexecute
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result ExecuteSQLServiceNotTransRecords(string ctrl, Dictionary<string, object> model)
        {
            var dic = model.ToDictionary();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = dbh.ExecuteDataTable(sqlstr.ToLower());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero)
                {

                    if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                        else
                                        {

                                        }
                                    }
                                }

                            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                            var rsdt = dbh.ExecuteSQL(sqlstr.Str2SQL());


                            if (rsdt < 0)
                            {
                                result = "业务执行失败！";
                            }


                        }



                }

            return new Result(result);

        }

        /// <summary>
        ///  通用导出功能
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object ReplaceTemplate(string ctrl, Dictionary<string, string> dic)
        {
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";

            sqlstr = sqlstr.Replace("{0}", ctrl);
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = dbh.ExecuteDataTable(sqlstr.ToLower());
            return new Result(dt);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sqlstr"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object CoreExecuteSQL(string sqlstr, Dictionary<string, string> dic)
        {
            sqlstr = dic.ToSQL(sqlstr);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return dbh.ExecuteDataTable(sqlstr.Str2SQL());
        }
 

        #endregion


    }
}

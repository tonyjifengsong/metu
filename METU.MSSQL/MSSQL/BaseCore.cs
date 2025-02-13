using METU.CACHES;
using METU.INTERFACE.ICore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace METU.MSSQL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseCore  
    {/// <summary>
     /// 
     /// </summary>
        private CoreBLL bll = null;
        /// <summary>
        /// 
        /// </summary>
        public string namespaceconfig = "MSSQLConnectionString".ToUpper();
        /// <summary>
        /// 
        /// </summary>
        public BaseDbContext dbh = null;
        /// <summary>
        /// 
        /// </summary>
        public DBHelpers db = null;
 
        /// <summary>
        ///  基类BLL
        /// </summary>
        public BaseCore(string config = null)
        {
            if (config == null)
            {
                FileHelper.Writelog("BaseCore构造函参数为空！");

            }
            if (config.Trim().Length<1)
            {
                FileHelper.Writelog("BaseCore构造函参数长度不可为0！");

            }
            else
            {
                namespaceconfig = config;
            }
            FileHelper.Writelog("BaseCore构造函数执行！");
            FileHelper.Writelog("config：");

            FileHelper.Writelog( namespaceconfig);

            if (CommonCache.ConfigCaches != null && CommonCache.ConfigCaches.Count() > 0 && CommonCache.ConfigCaches.ContainsKey(namespaceconfig))
            {
                FileHelper.Writelog("ConfigCaches is not null");
                if (namespaceconfig.Trim().ToString().Length > 0)
                {
                    FileHelper.Writelog("Config is  not  null");
                    if (CommonCache.ConfigCaches[namespaceconfig.ToUpper()] != null)
                    {
                        FileHelper.Writelog("ConfigCaches["+namespaceconfig+"]：");
                        FileHelper.Writelog(CommonCache.ConfigCaches[namespaceconfig]);

                        bll = new CoreBLL(namespaceconfig.ToUpper());
                        dbh = bll.dbh;
                        db = bll.db;
                    }
                    else
                    {
                        FileHelper.Writelog("ConfigCaches[" + namespaceconfig + "]： null");

                        bll = new CoreBLL();
                        dbh = bll.dbh;
                        db = bll.db;
                    }

                }
                else
                {
                    FileHelper.Writelog("  namespaceconfig is null");

                    bll = new CoreBLL();
                    dbh = bll.dbh;
                    db = bll.db;
                }
            }
            else
            {
                FileHelper.Writelog("ConfigCaches is null");

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
               var dt= corebll.db.ExecuteDataTable(sqlstr.ToLower());
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
        public virtual bool Add<T>(T model) where T : class, IEntity, new()
        {
            int result = 0;


            try
            { 
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
                 //FileHelper.Writelog(ex, "ex");
                MSGBox.EXMessage(ex.InnerException.Message);
                return false;
            }

            return result > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Register<T>(T model) where T : class, IEntity, new()
        {
            int result = 0;
            try
            { 
                bll.dbh.Add(model);
                result = bll.dbh.SaveChanges();
            }
            catch (Exception ex)
            {
                 //FileHelper.Writelog(ex, "ex");
                MSGBox.EXMessage(ex.InnerException.Message);
                return false;
            }

            return result > 0;
        }



        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool AddBat<T>(List<T> list) where T : class, IEntity, new()
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
                 //FileHelper.Writelog(ex, "ex");

                return false;
            }

            return result > 0;
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
                return false;
            }

            return result > 0;
        }
       
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
        
    }
}

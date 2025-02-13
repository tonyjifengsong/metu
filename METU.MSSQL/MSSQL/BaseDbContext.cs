
using METU.CACHES;
using METU.CONFIGS;
using METU.MODEL;
using METU.MSSQL;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{/// <summary>
/// 
/// </summary>
    public class BaseDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {

        }/// <summary>
         /// 
         /// </summary>
        public BaseDbContext()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="baseflag"></param>
        /// <returns></returns>
        public EntityEntry Add(object entity, bool baseflag)
        {
          
            if (baseflag)
            {
                return base.Add(entity);
            }
            else
            {
                return Add(entity);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override EntityEntry Add(object entity)
        {

            try
            {

                return base.Add(entity);
            }
            catch (Exception ex)
            {

                return null;
            }
        }/// <summary>
         /// 
         /// </summary>
         /// <typeparam name="TEntity"></typeparam>
         /// <param name="entity"></param>
         /// <returns></returns>
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {

            try
            {
                return base.Add(entity);
            }
            catch
            {
                return null;
            }
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="entity"></param>
         /// <returns></returns>
        public override EntityEntry Update(object entity)
        {

            return base.Update(entity);

        }/// <summary>
         /// 
         /// </summary>
         /// <param name="entity"></param>
         /// <param name="baseflag"></param>
         /// <returns></returns>
        public EntityEntry Update(object entity, bool baseflag)
        {

            if (baseflag)
            {
                return base.Update(entity);
            }
            else
            {
                return Update(entity);
            }





        }/// <summary>
         /// 
         /// </summary>
         /// <typeparam name="TEntity"></typeparam>
         /// <param name="entity"></param>
         /// <returns></returns>
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {


            return base.Update(entity);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public override void UpdateRange(IEnumerable<object> entities)
        {

            base.UpdateRange(entities);
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="entities"></param>
        public override void UpdateRange(params object[] entities)
        {

            base.UpdateRange(entities);
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="baseflag"></param>
         /// <param name="entity"></param>
        public void UpdateRange(bool baseflag, params object[] entity)
        {

            if (baseflag)
            {
                base.UpdateRange(entity);
            }
            else
            {
                UpdateRange(entity);
            }

        }/// <summary>
         /// 
         /// </summary>
         /// <param name="entities"></param>
        public override void AddRange(IEnumerable<object> entities)
        {

            base.AddRange(entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public override void AddRange(params object[] entities)
        {

            base.AddRange(entities);
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="baseflag"></param>
         /// <param name="entity"></param>
        public void Add(bool baseflag, params object[] entity)
        {

            if (baseflag)
            {
                base.AddRange(entity);
            }
            else
            {
                AddRange(entity);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
        {

            return base.AddRangeAsync(entities, cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public override Task AddRangeAsync(params object[] entities)
        {

            return base.AddRangeAsync(entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override EntityEntry Remove(object entity)
        {

            return base.Remove(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="baseflag"></param>
        /// <returns></returns>
        public EntityEntry Removeentity(object entity)
        {

            //return base.Remove(entity);
            return base.Update(entity);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="baseflag"></param>
        /// <returns></returns>
        public EntityEntry Remove(object entity, bool baseflag)
        {

            if (baseflag)
            {
                return base.Update(entity);
                //  return base.Remove(entity);
            }
            else
            {
                return base.Update(entity);
                // return Remove(entity);
            }
        }/// <summary>
         /// 
         /// </summary>
         /// <typeparam name="TEntity"></typeparam>
         /// <param name="entity"></param>
         /// <returns></returns>
        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {

            return base.Update<TEntity>(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public override void RemoveRange(IEnumerable<object> entities)
        {

            base.UpdateRange(entities);
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="baseflag"></param>
         /// <param name="entity"></param>
        public void RemoveRange(bool baseflag, IEnumerable<object> entity)
        {

            if (baseflag)
            {
                base.Update(entity);
                // base.Remove(entity);
            }
            else
            {
                Remove(entity);
            }
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Assembly> lists = new List<Assembly>();
            bool isservice = false;
            try
            {
                isservice = bool.Parse( CommonCache.ConfigCaches.GetValueOrDefault("dllfindenabled").ToString());
            }
            catch
            {
                isservice = false;
            }
            if (isservice)
            {
                lists = DLLASMHelper.GetAllAssemblies().ToList();
            }
            //  List<Assembly> 
            else
            {
                lists = DLLASMHelper.GetAllAssemblies().ToList();
            }
            foreach (Assembly assembly in lists)
            {

                List<Type> lst = new List<Type>();

                try
                {

                    lst = assembly.ExportedTypes.ToList().Where(p => p.IsClass && p.FullName.ToLower().IndexOf("model") > 0 && p.GetInterface("IEntityBase") != null).ToList();
                }
                catch
                {
                    continue;
                }
                foreach (Type type in lst)
                {
                    if (type.IsClass)
                    { 
                            try
                            {
                                modelBuilder.Model.AddEntityType(type);

                            }
                            catch { }
                        
                    }
                }
            }


            base.OnModelCreating(modelBuilder);
        }


        #region extention
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   dynamic DynCore(  Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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

        public   dynamic Core(  Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();



            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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



        public   int Coreint(  Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return rsdt;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return rsdt;
                            }

                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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

        public   bool Corebool(  Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
        public   object Coreobject(  Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
        public   Result CoreBasic(  Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
        public   Result CoreService(  string servicename, Dictionary<string, object> dic)
        {

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
        public   Result CoreList(  int pageindex, int pagesize, Dictionary<string, string> dic = null)
        {

            string servicename = dic.ServiceName();

            Result rs = new Result();
            rs.Data.Add("pageindex", pageindex);
            rs.Data.Add("pagesize", pagesize);

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
        public   Result DoCore(  Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);

            var dt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());

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
                                var rsdt =  this.ExecuteSql( sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }

                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = this. ExecuteDataTable( sqlstr.Str2SQL());
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
        public   dynamic Reports(  string tname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGE_LIST_COMMON;
            sqlstr = sqlstr.Replace("{0}", tname);


            return this. ExecuteDataTable( sqlstr.ToLower());
        }

        /// <summary>
        /// 根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中表名}/getlist
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中表名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>

        public   object configServices(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "getconfig";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }

        /// <summary>
        /// [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        /// <returns></returns>
        public   object PageInfo(  string methodname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGECONTROL_LIST;
            sqlstr = sqlstr.Replace("{0}", methodname);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return this. ExecuteDataTable( sqlstr.Str2SQL());
        }
        /// <summary>
        /// [页面控件信息内容保存到后端] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；后缀：save
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object sysconfigServicessave(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "save";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }

        /// <summary>
        /// [根据页面控件信息获取对应的数据]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object sysconfigServicesgets(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "get";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }

        /// <summary>
        /// [列表]根据数据库中的表名;获取表中的数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object sysconfigServices(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "getlist";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }


        /// <summary>
        /// [添加] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object sysconfigServicesadd(  Dictionary<string, string> dic)
        {

            string methodname = dic.MethodName();
            methodname = methodname + "add";
            dic.AddMethodName(methodname);
            return DoCore( dic);
        }

        /// <summary>
        ///  [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object sysconfigServicesedit(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "edit";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }
        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object sysconfigServicesupdate(  Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "update";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除（逻辑删除）指定数据；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public   object sysconfigServicesdelete(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "delete";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除(物理删除)指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
        public   object sysconfigServicesremovebykey(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "removebykey";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }
        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object sysconfigServicesget(  Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "gets";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }

        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public   object sysconfigServiceslist(  Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "list";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }
        /// <summary>
        /// 根据数据库配置，服务配置 执行对应的业务逻辑；地址组合：{数据库配置名}APP/{数据库中表SysPageService中字段servicename服务名}Service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object Services(  Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "service";
            dic.AddServiceName(methodname);
            return DoCore( dic);
        }

        /// <summary>
        /// 多ＳＱＬ语句执行，非事务；服务名：{ctrl}bntservice
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public   Result BatServiceNotTransRecords(  string ctrl, Dictionary<string, object> model)
        {
            var dic = model.ToDictionary();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}bntservice'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = this.ExecuteDataTable( sqlstr.ToLower());
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
                                var rsdt = this.ExecuteSQL(  sqlstr.Str2SQL());


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
        public   Result ExecuteSQLServiceNotTransRecords(  string ctrl, Dictionary<string, object> model)
        {
            var dic = model.ToDictionary();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = this.ExecuteDataTable(  sqlstr.ToLower());
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
                            var rsdt = this.ExecuteSQL( sqlstr.Str2SQL());


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
        public   object ReplaceTemplate( string ctrl, Dictionary<string, string> dic)
        {
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";

            sqlstr = sqlstr.Replace("{0}", ctrl);
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = this.ExecuteDataTable(sqlstr.ToLower());
            return new Result(dt);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sqlstr"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public   object CoreExecuteSQL(  string sqlstr, Dictionary<string, string> dic)
        {
            sqlstr = dic.ToSQL(sqlstr);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return this.ExecuteDataTable( sqlstr.Str2SQL());
        }

        #endregion 

    }
}

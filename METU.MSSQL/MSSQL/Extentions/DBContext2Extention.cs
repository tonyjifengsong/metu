using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static class DBContext2Extention
    {
        #region DELETE

        /// <summary>
        /// 根据模型删除
        /// </summary>
        /// <param name="model">包含要删除id的对象</param>
        /// <returns></returns>
        public static int Delete<T>(this DbContext MasterDb, T model) where T : class, new()
        {
            MasterDb.Set<T>().Attach(model);
            MasterDb.Set<T>().Remove(model);
          return  MasterDb.SaveChanges();
        }



        #endregion DELETE
        /// <summary>
        /// 新增 实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Insert<T>(this DbContext MasterDb, T model) where T : class, new()
        {
            MasterDb.Set<T>().Add(model);
            return MasterDb.SaveChanges();
        }
        /// <summary>
        /// 普通批量插入
        /// </summary>
        /// <param name="datas"></param>
        public static int InsertRange<T>(this DbContext MasterDb, List<T> datas) where T : class, new()
        {
            MasterDb.Set<T>().AddRange(datas);
          return  MasterDb.SaveChanges();
        }

        /// <summary>
        /// 单个对象修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Update<T>(this DbContext context, T model) where T : class, new()
        {
            EntityEntry entry = context.Entry<T>(model);
            context.Set<T>().Attach(model);
            entry.State = EntityState.Modified;
           return context.SaveChanges();
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static int UpdateAll<T>(this DbContext context, List<T> models) where T : class, new()
        {
            foreach (var model in models)
            {
                EntityEntry entry = context.Entry(model);
                entry.State = EntityState.Modified;
            }
            return context.SaveChanges();
        }
        /// <summary>
        /// 批量统一修改
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="modifiedProNames"></param>
        /// <returns></returns>
        public static int Update<T>(this DbContext context, T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames) where T : class, new()
        {
            try { 
            //查询要修改的数据
            List<T> listModifing = context.Set<T>().Where(whereLambda).ToList();
            Type t = typeof(T);
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
            proInfos.ForEach(p =>
            {
                if (modifiedProNames.Contains(p.Name))
                {
                    dictPros.Add(p.Name, p);
                }
            });
            if (dictPros.Count <= 0)
            {
                throw new Exception("指定修改的字段名称有误或为空");
            }
            foreach (var item in dictPros)
            {
                PropertyInfo proInfo = item.Value;

                //取出 要修改的值
                object newValue = proInfo.GetValue(model, null);

                //批量设置 要修改 对象的 属性
                foreach (T oModel in listModifing)
                {
                    //为 要修改的对象 的 要修改的属性 设置新的值
                    proInfo.SetValue(oModel, newValue, null);
                }
                }
            }
            catch (Exception ex)
            {
                
                return  -1;
            }
            return context.SaveChanges();
        }

    }
}

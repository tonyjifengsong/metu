using METU.INTERFACE.ICore;
using METU.MODEL;
using METU.MSSQL.MSSQL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace METU.Main.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    [EnableCors("cors")]
    [Route("api/[controller]/[action]")]//路由
     [ApiResult]//统一返回结果
               //  [Auth]//权限验证
    [ApiController]//接口API类
 
    public partial class METUTVBase<T, V> : ControllerBase where T : class, IEntity, new() where V : class, new()

    { 

        /// <summary>
        /// 获取页面控件信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetPageInfo()
        {
            string tablename = "";
            T md = new T();
            tablename = md.GetType().Name;
            return bll.GetPageControlInfoByTableName(tablename);
        }

        /// <summary>
        /// 通过表名获取页面控件信息
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPageMap(string tablename)
        {
            tablename.validatedIsString("传入参数不可以为空！");

            return bll.GetPageControlInfoByTableName(tablename);
        }


        /// <summary>
        /// 此方法仅限基类使用
        /// </summary>
        private IBLL<T> bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="content"></param>
        public METUTVBase(BaseBLL<T> content)
        {

            bll = content;
            

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual bool Add(V model)
        {
            model.validatedIsNull("传入参数不可以为空！");
            T md = new T();
            model.CopyTo(md);


            return bll.Add(md);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddBat(List<V> list)
        {
            list.validatedIsNull("传入参数不可以为空！");
            if (list.Count == ConstNum.Zero) MSGBox.EXMessage("传入参数个数不可以为0");
            List<T> mdlist = new List<T>();
            foreach (var itm in list)
            {
                T md = new T();
                itm.CopyTo(md);
                mdlist.Add(md);

            }

            return bll.AddBat(mdlist);
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Delete(T Param)
        {
            Param.validatedIsString("传入参数不可以为空！");

            Param.id.validatedIsGUID();

            return bll.Delete(Param);
        }


        /// <summary>
        /// 通过主键值删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="PID"></param>
        /// <returns></returns>

        [HttpPost]

        public bool DeleteByKey(string PID)
        {
            PID.validatedIsString("传入参数不可以为空！");

            PID.validatedIsGUID();
            return bll.DeleteeByKey<T>(PID);
        }


        /// <summary>
        /// 通过 实体参数 返回列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>

        [HttpPost]

        public virtual List<T> GetList(V model)
        {



            T md = new T();
            model.CopyTo(md);
            int pageindex = 1;
            int pagesize = ConstNum.Ten;


            return bll.GetList(md, pageindex, pagesize);


        }

        [HttpPost]

        public virtual List<T> GetPageList(QueryParam model)
        {

            T md = new T();
            model.CopyTo(md);
            int pageindex = 1;
            int pagesize = ConstNum.Ten;


            return bll.GetList(md, pageindex, pagesize);


        }
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>

        [HttpPost]
        public T GetModel(V Param)
        {
            Param.validatedIsString("传入参数不可以为空！");

            T md = new T();
            Param.CopyTo(md);

            return bll.GetModel(md);
        }
        /// <summary>
        /// 通过键值获取数据
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>

        [HttpPost]
        public T GetModeleByKey(string PID)
        {
            PID.validatedIsString("传入参数不可以为空！");

            PID.validatedIsGUID();


            return bll.GetModeleByKey(PID);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public bool Update(T model)
        {

            model.id.validatedIsString("传入参数不可以为空！");

            model.id.validatedIsGUID();

            return bll.Update(model);
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>

        [HttpPost]
        public bool UpdateBat(List<V> list)
        {
            List<T> tlist = new List<T>();
            foreach (var itm in list)
            {
                T md = new T();
                itm.CopyTo(md);
                tlist.Add(md);
            }
            return bll.UpdateBat(tlist);
        }
        /// <summary>
        /// 主从表获取
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]

        public dynamic GetMasterSlave(T entity)
        {
            entity.validatedIsString("传入参数不可以为空！");

            entity.id.validatedIsGUID();

            return null;// bll.GetMasterSlaveInfo(entity);
        }
        /// <summary>
        /// 批量操作数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]

        public bool SPDSave(List<T> entity)
        {
            entity.validatedIsString("传入参数不可以为空！");

            if (entity.Count == 0) MSGBox.EXMessage("传入参数的数量不可以为0！");

            List<T> lists = new List<T>();
            foreach (var itm in entity)
            {
                if (itm.id == null)
                {
                    itm.id = Guid.NewGuid().ToString();
                   
                    lists.Add(itm);
                    continue;
                }

                if (string.IsNullOrEmpty(itm.id))
                {
                    itm.id = Guid.NewGuid().ToString();
                    
                    lists.Add(itm);
                    continue;
                }

                if (itm.id.ToString().Trim().Length == ConstNum.Zero)
                {
                    itm.id = Guid.NewGuid().ToString();
                   
                    lists.Add(itm);
                    continue;
                }

                itm.id.validatedIsGUID();
                lists.Add(itm);
            }
            return bll.Saves(lists);
        }
        /// <summary>
        /// 通过待删除的主键列表，删除数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]

        public bool DeleteByKeyList(List<string> list)
        {
            list.validatedIsNull("传入参数不可以为空！");
            foreach (var itm in list)
            {
                itm.validatedIsGUID();
            }

            if (list.Count == ConstNum.Zero) MSGBox.EXMessage("传入参数不可以为空！");
            return bll.DeleteeByKeyList<T>(list);
        }

        /// <summary>
        /// 通用分页调用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual QueryResult CommonPageList(QueryParam param)
        {
            QueryResult rs = new QueryResult();
            int pagecount = 0;
            rs.Result = bll.Page<QueryParam>(param.pageindex, param.pagesize, out pagecount, null, p => p.id == param.searchtxt, true);
            rs.PageCount = pagecount;
            return rs;
        }

        /// <summary>
        /// 通用分页调用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual QueryResult GetPageByContains(QueryParam param)
        {
            QueryResult rs = new QueryResult();
            int pagecount = 0;
            rs.Result = bll.GetPageByContains(param.pageindex, param.pagesize, out pagecount, param.ToQueryConditions());
            rs.PageCount = pagecount;
            return rs;
        }
        /// <summary>
        /// 通用分页调用，通过参数（字段名_对应操作:字段值）对象 获取列表
        ///JSON:{Username:"tony",PWD:"123456"}等同于：{Username_EQ:"tony",PWD:"123456"}等同于：{Username_EQ:"tony",PWD_EQ:"123456"}
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual QueryResult GetPageByDics(Dictionary<string, object> param)
        {
            QueryResult rs = new QueryResult();
            int pagecount = 0;
            rs.Result = bll.GetPageByDics(param.ToQueryConditions(), out pagecount);
            rs.PageCount = pagecount;
            return rs;
        }

        /// <summary>
        /// 通过参数（字段值、字段名、对应操作、是否排序）列表通用分页调用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual QueryResult GetPageByLists(List<Condition> param)
        {
            QueryResult rs = new QueryResult();
            int pagecount = 0;
            rs.Result = bll.GetPageByDics(param, out pagecount);
            rs.PageCount = pagecount;
            return rs;
        }
        /// <summary>
        /// 通用分页调用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual QueryResult GetPageByParams(QueryParam param)
        {
            QueryResult rs = new QueryResult();
            int pagecount = 0;
            rs.Result = bll.GetPageByParams(param.pageindex, param.pagesize, out pagecount, param);
            rs.PageCount = pagecount;
            return rs;
        }
        /// <summary>
        /// 通过实体物理删除数据
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Remove(T Param)
        {
            Param.validatedIsNull();
            Param.id.validatedIsGUID();
            return bll.RemoveByKey(Param.id);
        }
        /// <summary>
        /// 通过ID物理删除数据
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        [HttpPost]
        public bool RemoveByKey(string PID)
        {
            PID.validatedIsString("传入参数不可以为空！");

            PID.validatedIsGUID();


            return bll.RemoveByKey(PID);
        }
        /// <summary>
        /// 通过ID列表，物理删除数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public bool RemoveByKeyList(List<string> list)
        {
            list.validatedIsNull();
            if (list.Count == ConstNum.Zero) MSGBox.EXMessage("传入数据列表中个数不可以为0！");

            foreach (var itm in list)
            {
                itm.validatedIsGUID();
            }
            return bll.RemoveByKeyList(list);
        }
       
    }
}

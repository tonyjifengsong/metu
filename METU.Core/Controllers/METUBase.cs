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
    //  [AuthFilter]
    [EnableCors("cors")]
     [Route("api/[controller]/[action]")]//路由
   // [ApiResult]//统一返回结果
               // [Auth]//权限验证
    [ApiController]//接口API类

    public partial class METUBase<T> : ControllerBase where T : class, IEntity, new()

    { 
       
      

        /// <summary>
        /// 此方法仅限基类使用
        /// </summary>
        private IBLL<T> bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="content"></param>
        public METUBase(BaseBLL<T> content)
        {

            bll = content;
            
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Add(T model)
        {
            return bll.Add(model);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddBat(List<T> list)
        {

            return bll.AddBat(list);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Delete(T Param)
        {
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

        public List<T> GetList(T model)
        {
            int pageindex = 1;
            int pagesize = ConstNum.Ten;

            return bll.GetList(model, pageindex, pagesize);
        }
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>

        [HttpPost]
        public T GetModel(T Param)
        {

            return bll.GetModel(Param);
        }
        /// <summary>
        /// 通过键值获取数据
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>

        [HttpPost]
        public T GetModeleByKey(string PID)
        {

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

            return bll.Update(model);
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>

        [HttpPost]
        public bool UpdateBat(List<T> list)
        {

            return bll.UpdateBat(list);
        }
        /// <summary>
        /// 主从表获取
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]

        public dynamic GetMasterSlave(T entity)
        {
            return null;// bll.GetMasterSlaveInfo(entity);
        }
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
            return bll.GetPageControlInfoByTableName(tablename);
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

            return bll.Remove(Param);
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
        /// <summary>
        /// 批量操作数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]

        public bool SPDSave(List<T> entity)
        {
            entity.validatedIsString("传入参数不可以为空！");

            if (entity.Count == ConstNum.Zero) MSGBox.EXMessage("传入参数的数量不可以为0！");

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
        public virtual QueryResult GetPageByDics(Dictionary<string, object> param)
        {
            QueryResult rs = new QueryResult();
            int pagecount = 0;
            rs.Result = bll.GetPageByDics(param.ToQueryConditions(), out pagecount);
            rs.PageCount = pagecount;
            return rs;
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
    }
}

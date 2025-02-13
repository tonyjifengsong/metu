using METU.CACHES;
using METU.ES;
using METU.ES.Elastic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace METU.API.Controllers
{
    [EnableCors("cors")]
    [Route("{indexname}Page")]
    [ApiController]
    public class ESCOREController : ControllerBase
    {
        private IBaseEsContext _esContext;
         
    public ESCOREController(IBaseEsContext eSContext)
        {

            _esContext = eSContext;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="tname">数据库中表名称</param>
        /// <param name="indexname">数据库名，地索引名称</param>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/{tname}")]
        public bool Add([FromRoute] string tname, [FromRoute] string indexname, dynamic obj)
        {
          
            ESCALL.PostLog(obj, tname, indexname, null);
            return true;
        }

        /// <summary>
        /// 添加数据，需要指定操作信息，数据库及表名称还有数据的唯一值
        /// </summary>
        /// <param name="tname">数据库中表名称</param>
        /// <param name="indexname">数据库名，地索引名称</param>
        /// <param name="id">添加数据的ID值</param>
        /// <param name="operate">操作符号默认Add</param>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/{tname}/{id}")]
        public bool InsertESAPI([FromRoute] string tname, [FromRoute] string indexname, [FromRoute] string id, dynamic obj)
        {
            ESCALL.PostLog(obj, tname, indexname, id, null);
            return true;
        }

        [HttpPost]
        [Route("ES/ADD")]
        public bool IESAPI([FromRoute] string indexname, dynamic obj)
        {
           
          var rs=  ESHelper.Insert(CommonCache.LogURL, indexname, obj);            
            return rs;
        }

        [HttpPost]
        [Route("Search/{Fname}/{Fvalue}")]
        public object IESSearchAPI([FromRoute] string indexname, [FromRoute] string Fname, [FromRoute] string Fvalue)
        {
           
            var rs = ESHelper.Search(CommonCache.LogURL, indexname, Fname, Fvalue,0,100);
            return rs.toJson();
        }
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="address"></param>
        [HttpPost]
        [Route("esmodel/ADD")]
        public void AddESMODEL([FromRoute] string indexname, List<ESModel> addressList)
        {
            if (addressList == null || addressList.Count < 1)
            {
                return;
            }
            _esContext.InsertMany(indexname,addressList);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id">查询ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pagesize">页面大小</param>
        /// <returns></returns>
        [HttpGet]
        [Route("search/{id}/{pagesize}/{pageIndex}")]
        public dynamic  Search([FromRoute] string indexname, [FromRoute] string id, [FromRoute] int pageIndex, [FromRoute] int pagesize)
        {
            if (pageIndex<1 || pagesize < 1)
            {
                return   MODEL.Result.ERROR(0,"页面大小与页码必需大于0！");
            }
          var rs=  _esContext.Get(indexname,id, pageIndex,pagesize);
            return new METU.MODEL.Result(rs.toJson());
        }
    }
}

using METU.MODEL;
using METU.REDIS;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace METU.Main.Controllers
{
    /// <summary>
    /// 单点登陆统一接口
    /// </summary>
    [EnableCors("cors")]
    [Route("SSOapi")]
    [ApiController]
    
    public partial  class METUSSOController : ControllerBase
    {
         /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableCors("cors")]
        [HttpPost]
        [Route("{id}Login")]
        public Result Login([FromRoute] string id, object model)
        {
           Result bll = (Result)BLLRoute.ExecuteSSO(id, model, "Login");
           
            Result ubll = (Result)BLLRoute.ExecuteSSO(id, bll.Data, "GetUser");
            string token = Guid.NewGuid().ToString();
            bll.Data.Add("Token", token);
            RedisHelper.Default.StringSet(id.ToString().Trim()+token, ubll.toJson(), TimeSpan.FromSeconds(60 * 30 * 20));
      
            return bll;
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableCors("cors")]
        [HttpPost]
        [Route("{id}LoginOut")]
        public Result LoginOut([FromRoute] string id,string  model)
        {
           
            var a = RedisHelper.Default.KeyDelete(model);
           
            return new Result (a);
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableCors("cors")]
        [HttpPost]
        [Route("{id}signOut")]
        public Result SignOut([FromRoute] string id, string model)
        { 
            var a = RedisHelper.Default.KeyDelete(model );
           
            return new  Result (a);
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableCors("cors")]
        [HttpPost]
        [Route("{id}Register")]
        public Result Register([FromRoute] string id, object model)
        {
             Result bll = (Result)BLLRoute.ExecuteSSO(id, model, "Register");
           
            return bll;
        }
      
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableCors("cors")]
        [HttpPost]
        [Route("{id}GetUserInfo")]
        public Result GetUser([FromRoute] string id, string model)
        {
            if (RedisHelper.Default.KeyExists(model))
            {
                return RedisHelper.Default.StringGet(model).JsonToObject<Result>();
            }
            Result bll = (Result)BLLRoute.ExecuteSSO(id, model, "GetUser");
            
                return bll;
            
        }
       
        /// <summary>
        /// 判断用户是否已经登录系统
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableCors("cors")]
        [HttpPost]
        [Route("{id}Check")]
        public Result CheckUser([FromRoute] string id, string model)
        {
               bool rs = false;
            rs = RedisHelper.Default.KeyExists(model );


            if (rs)
            {
                var result = RedisHelper.Default.StringGet(model);
                RedisHelper.Default.StringSet(id.ToString().Trim() + model, result, TimeSpan.FromSeconds(60 * 30 * 20));
 
                return new Result (result);
            }
            else
            {
               
                return Result.Failed();
            }

        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableCors("cors")]
        [HttpPost]
        [Route("{id}signin")]
        public Result signin([FromRoute] string id, string model)
        {
            Result rs = new Result();

            rs.IsSuccess = false;
            if (model == null) return rs;
            if (id == null) return rs;
            if (id.ToString().Trim().Length < 1) return rs;
               try
            {
                RedisHelper.Default.StringSet(id.ToString().Trim() + model, model.toJson(), TimeSpan.FromSeconds(60 * 30 * 20));
                rs.IsSuccess = true;
            }
            catch
            {
                rs.IsSuccess = false;
            }
            return rs;
        }


    }
}

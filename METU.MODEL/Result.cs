using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace METU.MODEL
{
    /// <summary>
    /// 统一返回值
    /// </summary>
    public class Result : ArgumentBase<Dictionary<string, object>>
    {
        public Result() {
            Data = new Dictionary<string, object>();
            this.Code = 1;
            Msg = "";
            Data.Add("result", "");
            IsSuccess = true;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get;
            set;
        }
        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="obj"></param>
        public Result(object obj)
        {
            Data = new Dictionary<string, object>();
            try
            {
                Data.Add("result", (obj));
            }catch(Exception ex)
            {
                Data.Add("result", "");
            }
            IsSuccess = true;
        }
        /// <summary>
        /// 初始化返回值
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="obj">结果</param>
        public Result(int code,object obj)
        {
            Data = new Dictionary<string, object>();
            this.Code = code;
            try
            {
                Data.Add("result",  (obj));
            }
            catch (Exception ex)
            {
                Data.Add("result", "");
            }
            IsSuccess = true;
        }
        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="msg">提示信息</param>
        public Result(int code, string msg)
        {
            Data = new Dictionary<string, object>();
            this.Code = code;
            Msg=msg;
            IsSuccess = true;
        }
        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="msg">提示信息</param>
        /// <param name="obj">返回结果</param>
        public Result(int code, string msg,object  obj)
        {
            Data = new Dictionary<string, object>();
            this.Code = code;
            Msg = msg;
            try
            {
                Data.Add("result",  (obj));
            }
            catch (Exception ex)
            {
                Data.Add("result", "");
            }
            IsSuccess = true;
        }
        /// <summary>
        /// 返回错误结果
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="msg">错误信息</param>
        /// <param name="obj">异常</param>
        /// <returns>返回结果</returns>
        public static Result ERROR(int code, string msg, object obj)
        {
            Result rs = new Result();
            rs.Code = code;
            rs.Msg = msg;
            try
            {
                rs.Data.Add("result", JsonSerializer.Serialize(obj));
            }catch(Exception ex)
            {
                rs.Data.Add("result", "");
            }
            rs.IsSuccess = false;
            return rs;
        }
        /// <summary>
        /// 返回错误结果
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public static Result ERROR(int code, string msg)
        {
            Result rs = new Result();
            rs.Code = code;
            rs.Msg = msg;
            rs.Data.Add("result", "");
            rs.IsSuccess = false;
            return rs;
        }

   
        /// <summary>
        /// 返回错误结果
        /// </summary>
        /// <param name="code">错误码</param>
        /// <returns></returns>
        public static Result ERROR(int code)
        {
            Result rs = new Result();
            rs.Code = code;
            rs.Msg = "";
            rs.Data.Add("result", "");
            rs.IsSuccess = false;
            return rs;
        }
        /// <summary>
        /// 执行失败
        /// </summary>
        /// <returns></returns>
        public static Result Failed()
        {
            Result rs = new Result();
            rs.Code = 0;
            rs.Msg = "";
            rs.Data.Add("result", "");
            rs.IsSuccess = false;
            return rs;
        }
    }
}

using METU.INTERFACE;
using METU.LOG;
using METU.MODEL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    public partial class WebBLLCoreBLL : DoCore, ICoreBusiness
    {
        private Result BaseResult = new Result();

        public override void DoWork(Dictionary<string, object> dic, object obj, string actionname = null)
        {

            //定义一个"类型信息"的对象.
            Type t = this.GetType();

            //定义一个成员信息类对象数组,并从程序集中获取.
            MemberInfo[] info = t.GetMembers();

            //逐个返回成员的名字.
            foreach (MemberInfo inf in info)
            {
                if (inf.Name.ToUpper() == actionname.ToUpper())
                {
                    //定义一个成员方法对象,这里是指定方法名称来获取的.
                    MethodInfo method = t.GetMethod(inf.Name);
                    object objs = Activator.CreateInstance(t);
 
                        LogCache.WriteLog("传入参数：", t.Name);
                        LogCache.WriteLog(dic, t.Name);
 
                    //参数
                    object[] nums = { dic, obj };
                    try
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        method.Invoke(objs, nums);
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;

                        LogCache.WriteLog("方法" + t.Name + "." + actionname + "执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");

                    }
                    catch (Exception ex)
                    {
                        LogCache.WriteLog(ex.Message);
                        LogCache.WriteLog(t.Name + actionname + "执行方法异常：" + ex.Message, t.Name);
                    }
                   
                        LogCache.WriteLog("返回结果：", t.Name);
                        LogCache.WriteLog(obj, t.Name);

                    
                }
            }
        }
        public override void DoWork(Dictionary<string, string> dic, object obj, string actionname = null)
        {


            //定义一个"类型信息"的对象.
            Type t = this.GetType();

            //定义一个成员信息类对象数组,并从程序集中获取.
            MemberInfo[] info = t.GetMembers();

            //逐个返回成员的名字.
            foreach (MemberInfo inf in info)
            {
                if (inf.Name.ToUpper() == actionname.ToUpper())
                {
                    //定义一个成员方法对象,这里是指定方法名称来获取的.
                    MethodInfo method = t.GetMethod(inf.Name);
                    object objs = Activator.CreateInstance(t);
 
                        LogCache.WriteLog("传入参数：", t.Name);
                        LogCache.WriteLog(dic, t.Name);

                     
                    //参数
                    object[] nums = { dic, obj };
                    try
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        method.Invoke(objs, nums);
                        obj = nums[1];
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;

                        LogCache.WriteLog("方法" + t.Name + "." + actionname + "执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                    }
                    catch (Exception ex)
                    {
                        LogCache.WriteLog(ex.Message);
                        LogCache.WriteLog(t.Name + "." + actionname + "执行方法异常：" + ex.Message, t.Name);
                    }
                   
                        LogCache.WriteLog("返回结果：", t.Name);
                        LogCache.WriteLog(obj, t.Name);
 
                }
            }
        }


        public string Message
        {
            get;

            set;
        }


        public WebBLLCoreBLL()
        {
           
        }
        public override dynamic DoWork(params object[] paramlist)
        {
            string actionname = "DoWork";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var itm in paramlist)
            {
                dic.ConDic(itm.ToDictionary());
            }
            if (dic.Count() < 1) return new Result();
            actionname = dic.BLLActionName();
            object result = new object();
            if (actionname.CheckStringValue()) result = DoWork(actionname, paramlist);
            return result;

        }

        public override dynamic DoWork(string actionname, params object[] paramlist)
        {
            object dyrs = new object();
            //定义一个"类型信息"的对象.
            Type t = this.GetType();

            //定义一个成员信息类对象数组,并从程序集中获取.
            MemberInfo[] info = t.GetMembers();

            //逐个返回成员的名字.
            foreach (MemberInfo inf in info)
            {
                if (inf.Name.ToUpper() == actionname.ToUpper())
                {
                    //定义一个成员方法对象,这里是指定方法名称来获取的.
                    MethodInfo method = t.GetMethod(inf.Name);
                    object objs = Activator.CreateInstance(t);

                  

                        LogCache.WriteLog("传入参数：", t.Name);
                        LogCache.WriteLog(paramlist, t.Name);


                    
                    //参数

                    try
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        dyrs = method.Invoke(objs, paramlist);
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;

                        LogCache.WriteLog("方法" + t.Name + "." + actionname + "执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                    }
                    catch (Exception ex)
                    {
                        LogCache.WriteLog(ex.Message);
                        LogCache.WriteLog(t.Name + "." + actionname + "执行方法异常：" + ex.Message, t.Name);
                    }
                     
                        LogCache.WriteLog("返回结果：", t.Name);
                        LogCache.WriteLog(BaseResult, t.Name);

                    
                }
            }
            if (paramlist.Length > 0)
            {
                if (dyrs != null)
                {
                    return dyrs;
                }
                return paramlist[paramlist.Length - 1];
            }
            else
            {
                return null;
            }
        }
        public void ExecuteServic(Dictionary<string, object> dic, object obj)
        {

          

        }

        /// <summary>
        /// 通过SQL模板执行SQL并返回结果
        /// </summary>
        /// <param name="dic">模板参数列表</param>
        /// <param name="ServiceName">模板名称</param>
        /// <returns></returns>
        public Result ExecuteService(Dictionary<string, string> dic, string ServiceName = null)
        {

             Result result = new Result();
           
            return result;
        }

    }
}

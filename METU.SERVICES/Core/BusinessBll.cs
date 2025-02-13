using METU.CACHES;
using METU.INTERFACE;
using METU.LOG;
using METU.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace METU.SERVICES.Core
{
    /// <summary>
    ///  created by  tony  2017-12-26
    /// </summary>
    public class BusinessBll : DoCore, IEventInit,IBusiness, ICoreBusiness
    {

        public event Action<Dictionary<string, string>, object> BusinessBus;
        public event Action<Dictionary<string, object>, object> BsBus;
       public Result BaseResult
        {
            get; set;
        }

        public ICBLL BaseHelper
        {
            get;

            set;
        }
        public string MSG { get ; set ; }
        public bool isSuccess { get ; set ; }

        public BusinessBll(ICBLL basehelp = null)
        {
           
            if (basehelp != null)
            {
                BaseHelper = basehelp;
            }
            else
            {
              //  BaseHelper = new bllbase();
            }
            BaseResult = new Result();
        }
        /// <summary>
        /// 执行业务逻辑方法
        /// </summary>
        /// <param name="dic">参数模板</param>
        /// <param name="obj">参数2</param>
       public   virtual void DoWork(Dictionary<string, string> dic, object obj, string actionname = null)
        {
            Type t = this.GetType();
            if (actionname == null)
            {

               
                if (BusinessBus != null)
                {
                    Delegate[] dlist = BusinessBus.GetInvocationList();
                    foreach (Action<Dictionary<string, string>, object> itm in dlist)
                    { 

                            LogCache.WriteLog(dic, t.Name);
                            LogCache.WriteLog("方法名称：dowork-" + itm.Method.Name, t.Name);

                       
                        BaseResult = (Result)obj;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        itm(dic, BaseResult);
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
                        LogCache.WriteLog("方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                        BaseResult = (Result)obj;
                        if (!BaseResult.IsSuccess)
                        {
                            break;
                        }
                       
                            LogCache.WriteLog(BaseResult, t.Name);
                        
                    }
                }
                return;
            }
            if (actionname.ToUpper() == "DOWORK")
            {
               
                if (BusinessBus != null)
                {
                    Delegate[] dlist = BusinessBus.GetInvocationList();
                    foreach (Action<Dictionary<string, string>, object> itm in dlist)
                    {
 
                            LogCache.WriteLog(dic, t.Name);
                            LogCache.WriteLog("方法名称：" + itm.Method.GetType().Name, "ActionElapsed");
                         
                        BaseResult = (Result)obj;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        itm(dic, BaseResult);
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
 
                        LogCache.WriteLog("DOWork方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                        if (!BaseResult.IsSuccess)
 
                        {
                            break;
                        }
                        
                            LogCache.WriteLog(BaseResult, t.Name);
                        
                    }
                }
            }
            else
            {
                DoAction(dic, obj, actionname);
            }

        }
        private void DoAction(Dictionary<string, string> dic, object obj, string actionname = null)
        {
            BaseResult = (Result)obj;


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
                        obj = method.Invoke(objs, nums);
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
      
        #region added by tony 2018-3-9 Memo:执行方法
        public virtual dynamic DoWork(params object[] paramlist)
        {
            string actionname = "DoWork";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var itm in paramlist)
            {
               // dic.ConDic(itm.ToDictionary());
            }
            if (dic.Count() < 1) return new Result();
            actionname = dic.ActionName();
            if (actionname.CheckStringValue()) BaseResult = DoWork(actionname, paramlist);
            return BaseResult;
        }

        public virtual dynamic DoWork(string actionname, params object[] paramlist)
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
                            LogCache.WriteLog(paramlist, t.Name);
                        
                   
                    //参数

                    try
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        BaseResult = (Result)method.Invoke(objs, paramlist);
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
            return BaseResult;
        }
        #endregion
       
        

        /// <summary>
        /// 通过SQL模板执行SQL并返回结果
        /// </summary>
        /// <param name="dic">模板参数列表</param>
        /// <param name="ServiceName">模板名称</param>
        /// <returns></returns>
        public virtual DataTable ExecuteService(Dictionary<string, string> dic, string ServiceName)
        {

            DataTable dt = new DataTable("Result");

            LogCache.WriteLog("传入参数:", ServiceName);
            LogCache.WriteLog(dic, ServiceName);
             Stopwatch sw = new Stopwatch();
            sw.Start();
            dt = BaseHelper.ExecuteDtByTemplate(dic, ServiceName);
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            LogCache.WriteLog(ServiceName + "方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
            if (dt == null) { dt = new DataTable(); LogCache.WriteLog("查询结果为空;", ServiceName); }
             if (dt != null) dt.TableName = ServiceName;
            if (dt == null) return new DataTable(ServiceName);
            return dt;
        }
        
        /// <summary>
        /// 执行SQL返回object
        /// </summary>
        /// <param name="Dic"></param>
        /// <param name="functionname"></param>
        /// <returns></returns>
        public virtual object ExecuteSQL(Dictionary<string, string> Dic, string functionname = null)
        {
            return BaseHelper.ExecuteSQL(Dic, functionname);
        }

        public virtual void DoWork(Dictionary<string, object> dic, object obj, string actionname = null)
        {
            Type t = this.GetType();
            if (actionname == null)
            {

               
                if (BsBus != null)
                {
                    Delegate[] dlist = BsBus.GetInvocationList();
                    foreach (Action<Dictionary<string, object>, object> itm in dlist)
                    {

                          LogCache.WriteLog(dic, t.Name);
                            LogCache.WriteLog("方法名称：" + itm.Method.GetType().Name, t.Name);

                       
                        BaseResult = (Result)obj;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        itm(dic, BaseResult);
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
                        LogCache.WriteLog(t.Name + "DoWork方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                        BaseResult = (Result)obj;
                        if (!BaseResult.IsSuccess)
                        {
                            break;
                        }
                       
                            LogCache.WriteLog(BaseResult, t.Name);
                        
                    }
                }
            }
            if (actionname.ToUpper() == "DOWORK")
            {
                
                if (BsBus != null)
                {
                    Delegate[] dlist = BsBus.GetInvocationList();
                    foreach (Action<Dictionary<string, object>, object> itm in dlist)
                    {
 
                            LogCache.WriteLog(dic, t.Name);
                            LogCache.WriteLog("方法名称：" + itm.Method.GetType().Name, t.Name);

                      
                        BaseResult = (Result)obj;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        itm(dic, BaseResult);
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
                        LogCache.WriteLog("方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                        BaseResult = (Result)obj;
                        if (!BaseResult.IsSuccess)
                        {
                            break;
                        }
                        
                            LogCache.WriteLog(BaseResult, t.Name);
                       
                    }
                }
            }
            else
            {
                DoAction(dic, obj, actionname);
            }
        }
        private void DoAction(Dictionary<string, object> dic, object obj, string actionname = null)
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

                    Result rs = (Result)obj;
 
                        LogCache.WriteLog("返回结果：", t.Name);
                        LogCache.WriteLog(obj,t.Name); }
 
                
            }
        }
        
        public virtual void InitialCaches()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataTable dt = ExecuteService(dic, "TEMPLATENAMESQL");
            if (dt!=null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MESCache.AppCaches.AddKey(dt.Rows[i]["templatename"].ToString(), dt.Rows[i]["templatecontent"]);
                }
            }
        }
       

        public void ExecuteService(Dictionary<string, string> dic, object obj)
        {
            
        }

        public void ExecuteoService(Dictionary<string, object> dic, object obj)
        {
            
        }

        public virtual void DoWork(object paramlist, object obj)
        {
            if (paramlist is Dictionary<string, object>)
            {
                BsBus((Dictionary<string, object>)paramlist, obj);
            }
            if(paramlist is Dictionary<string, string>)
            {
                BusinessBus((Dictionary<string, string>)paramlist, obj);
            }
        }

        public void InitialBus()
        {
           
        }

        public void InitialQueue()
        {
             
        }

        public void ClearEventBus()
        {
            
        }

        public void ClearEventQueue()
        {
             
        }

        public void DoQueueWork(Dictionary<string, string> dic, object obj)
        {
             
        }
    }
}

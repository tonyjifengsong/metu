using METU.LOG;
using METU.MODEL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.Business
{
    public class EventBase : IEvent, ICoreBusiness
    {
        public event Func<Dictionary<string, string>, object> BusinessBus;
        public event Func<Dictionary<string, object>, object> BsBus;
        public event Action<Dictionary<string, string>, object> Business;
        public event Action<Dictionary<string, object>, object> Bs;
        #region initial

        public Result BaseResult
        {
            get; set;
        }
        public bool isSuccess { get ; set; }
        public string MSG { get; set ; }

        public virtual void InitialBus()
        {
            BusinessBus = null;
            Business = null;
            BsBus = null;
            Bs = null;
        }
        #endregion
        #region   dowork
        /// <summary>
        /// 执行业务逻辑方法
        /// </summary>
        /// <param name="dic">参数模板</param>
        /// <param name="obj">参数2</param>
        public virtual void DoWork(Dictionary<string, string> dic, object obj, string actionname = null)
        {
            Type t = this.GetType();
            InitialBus();
            if (Business != null && actionname == null)
            {
                Delegate[] dlist = Business.GetInvocationList();
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
                return;
            }
            if (Business != null && actionname.ToUpper() == "DOWORK")
            {
                Delegate[] dlist = Business.GetInvocationList();
                foreach (Action<Dictionary<string, string>, object> itm in dlist)
                {
                    LogCache.WriteLog(dic, t.Name);
                    LogCache.WriteLog("方法名称：" + itm.Method.GetType().Name, "ActionElapsed");
                    BaseResult = (Result)obj;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    try
                    {
                        itm(dic, BaseResult);
                    }catch(Exception ex)
                    {
                        LogCache.WriteLog(ex, t.Name);
                    }
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
        #endregion
        #region dowork

        public virtual void DoWork(Dictionary<string, object> dic, object obj, string actionname = null)
        {
            Type t = this.GetType();
            if (actionname == null)
            {

                InitialBus();
                if (Bs != null)
                {
                    Delegate[] dlist = Bs.GetInvocationList();
                    foreach (Action<Dictionary<string, object>, object> itm in dlist)
                    {

                        LogCache.WriteLog(dic, t.Name);
                        LogCache.WriteLog("方法名称：" + itm.Method.GetType().Name, t.Name);


                        BaseResult = (Result)obj;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        try
                        {
                            itm(dic, BaseResult);
                        }
                        catch (Exception ex)
                        {
                            LogCache.WriteLog(ex, t.Name);
                        }
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
                InitialBus();
                if (Bs != null)
                {
                    Delegate[] dlist = Bs.GetInvocationList();
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
                    LogCache.WriteLog(obj, t.Name);
                }


            }
        }

        #endregion
        public dynamic DoWork(params object[] paramlist)
        {
            Result rs = new Result();            
            Type t = this.GetType();
            InitialBus();
            if (BusinessBus != null)
            {
                Delegate[] dlist = BusinessBus.GetInvocationList();
               
               
                    foreach (Func<Dictionary<string, string>, object> itm in dlist)
                    {
                        LogCache.WriteLog(paramlist, t.Name);
                        LogCache.WriteLog("方法名称：dowork-" + itm.Method.Name, t.Name);
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        try
                        {
                            var acrs = itm(paramlist[0].ToDictionary());
                            rs.Data.Add(t.Name.ToUpper(), acrs);
                        }
                        catch (Exception ex)
                        {
                            rs.Data.Add(t.Name.ToUpper(), ex);
                        }
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
                        LogCache.WriteLog("方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                        LogCache.WriteLog(rs.Data[t.Name.ToUpper()], t.Name);
                        
                    }
                
            }
            if (BsBus != null)
            {
                Delegate[] dlist = BsBus.GetInvocationList();
                
                    foreach (Func<Dictionary<string, string>, object> itm in dlist)
                    {
                        LogCache.WriteLog(paramlist, t.Name);
                        LogCache.WriteLog("方法名称：dowork-" + itm.Method.Name, t.Name);
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        try
                        {
                            var acrs = itm((Dictionary<string, string>)paramlist[0]);
                            rs.Data.Add(t.Name.ToUpper(), acrs);
                        }
                        catch (Exception ex)
                        {
                            rs.Data.Add(t.Name.ToUpper(), ex);
                        }
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;
                        LogCache.WriteLog("方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                        LogCache.WriteLog(rs.Data[t.Name.ToUpper()], t.Name);
                    }
                
            }
            return rs;
        }

        public dynamic DoWork(string actionname = null, params object[] paramlist)
        {
            Result rs = new Result();

            Type t = this.GetType();
            InitialBus();
            if (BsBus != null)
            {
                Delegate[] dlist = BsBus.GetInvocationList();
                foreach (Func<Dictionary<string, object>, object> itm in dlist)
                {
                    LogCache.WriteLog(paramlist, t.Name);
                    LogCache.WriteLog("方法名称：dowork-" + itm.Method.Name, t.Name);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    try
                    {
                        var acrs = itm((Dictionary<string, object>)paramlist[0]);
                        rs.Data.Add(t.Name.ToUpper(), acrs);
                    }
                    catch (Exception ex)
                    {
                        rs.Data.Add(t.Name.ToUpper(), ex);
                    }
                    sw.Stop();
                    TimeSpan ts2 = sw.Elapsed;
                    LogCache.WriteLog("方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                    LogCache.WriteLog(rs.Data[t.Name.ToUpper()], t.Name);
                }

            }
            if (BsBus != null)
            {
                Delegate[] dlist = BsBus.GetInvocationList();
                foreach (Func<Dictionary<string, object>, object> itm in dlist)
                {
                    LogCache.WriteLog(paramlist, t.Name);
                    LogCache.WriteLog("方法名称：dowork-" + itm.Method.Name, t.Name);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    try
                    {
                        var acrs = itm((Dictionary<string, object>)paramlist[0]);
                        rs.Data.Add(t.Name.ToUpper(), acrs);
                    }
                    catch (Exception ex)
                    {
                        rs.Data.Add(t.Name.ToUpper(), ex);
                    }
                    sw.Stop();
                    TimeSpan ts2 = sw.Elapsed;
                    LogCache.WriteLog("方法执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                    LogCache.WriteLog(rs.Data[t.Name.ToUpper()], t.Name);
                }

            }
            return rs;
        }

        public  virtual  void DoAction(object paramlist, object obj)
        {
            isSuccess = true;
            if (paramlist is Dictionary<string, string>)
            {
                if (Business != null)
                {
                    try
                    {
                        Business((Dictionary<string, string>)paramlist, obj);
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        MSG = ex.Message;
                    }
                }
            }
            if (paramlist is Dictionary<string, object>)
            {
                if (Bs != null)
                {
                    try
                    {
                        Bs((Dictionary<string, object>)paramlist, obj);
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        MSG = ex.Message;
                    }
                }
            }
        }

        public virtual dynamic DoWork(object paramlist, EventResultType resultType = EventResultType.LAST)
        {
            isSuccess = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            object rs = null;
            if (paramlist is Dictionary<string, string>)
            {
                if (resultType == EventResultType.LAST)
                {
                    if (BusinessBus != null)
                    {
                        try
                        {
                            rs = BusinessBus((Dictionary<string, string>)paramlist);
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            MSG = ex.Message;
                        }
                    }
                    dic.Add("result", rs);
                    return dic;
                }
                if (resultType == EventResultType.NONE)
                {
                    if (BusinessBus != null)
                    {
                        try
                        {
                            rs = BusinessBus((Dictionary<string, string>)paramlist);
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            MSG = ex.Message;
                        }
                    }
                    dic.Add("result", null);
                    return dic;
                }
                else
                {
                    if (BusinessBus != null)
                    {
                        Delegate[] dlist = BusinessBus.GetInvocationList();
                        foreach (Func<Dictionary<string, string>, object> itm in dlist)
                        {
                            try
                            {
                                var rsobj = itm((Dictionary<string, string>)paramlist);
                                dic.Add(itm.Method.Name.ToUpper(), rsobj);
                            }
                            catch (Exception ex)
                            {
                                isSuccess = false;
                                MSG = ex.Message;

                                dic.Add(itm.Method.Name.ToUpper(), ex);
                            }

                        }
                        return dic;
                    }
                }
                return null;
            }
            if (paramlist is Dictionary<string, object>)
            {
                if (resultType == EventResultType.LAST)
                {
                    if (BsBus != null)
                    {
                        try
                        {
                            rs = BsBus((Dictionary<string, object>)paramlist);
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            MSG = ex.Message;
                        }
                    }
                    dic.Add("result", rs);
                    return dic;
                }
                if (resultType == EventResultType.NONE)
                {
                    if (BsBus != null)
                    {
                        try
                        {
                            rs = BsBus((Dictionary<string, object>)paramlist);
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            MSG = ex.Message;
                        }
                    }
                    dic.Add("result", null);
                    return dic;
                }
                else
                {
                    if (BsBus != null)
                    {
                        Delegate[] dlist = BsBus.GetInvocationList();
                        foreach (Func<Dictionary<string, object>, object> itm in dlist)
                        {
                            try
                            {
                                var rsobj = itm((Dictionary<string, object>)paramlist);
                                dic.Add(itm.Method.Name.ToUpper(), rsobj);
                            }
                            catch (Exception ex)
                            {
                                isSuccess = false;
                                MSG = ex.Message;

                                dic.Add(itm.Method.Name.ToUpper(), ex);
                            }

                        }
                        return dic;
                    }
                }
                return null;
            }
            return null;
        }

    }

}

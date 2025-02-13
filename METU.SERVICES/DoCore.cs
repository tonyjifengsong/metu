using METU.INTERFACE;
using METU.LOG;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace System
{

    public partial class DoCore : ICoreBusiness
    {
        public virtual void DoWork(Dictionary<string, object> dic, object obj, string actionname = null)
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
        public virtual void DoWork(Dictionary<string, string> dic, object obj, string actionname = null)
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
        public virtual dynamic DoWork(params object[] paramlist)
        {
            string actionname = "DoWork";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var itm in paramlist)
            {
                dic.ConDic(itm.ToDictionary());
            }
            if (dic.Count() < 1) return new object();
            actionname = dic.ActionName();
            object result = new object();
            if (actionname.CheckStringValue()) result = DoWork(actionname, paramlist);
            return result;

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
                        method.Invoke(objs, paramlist);
                        sw.Stop();
                        TimeSpan ts2 = sw.Elapsed;

                        LogCache.WriteLog("方法" + t.Name + "." + actionname + "执行用时：" + ts2.TotalMilliseconds, "ActionElapsed");
                    }
                    catch (Exception ex)
                    {
                        LogCache.WriteLog(ex.Message);
                        LogCache.WriteLog(t.Name + "." + actionname + "执行方法异常：" + ex.Message, t.Name);
                    }
                   
                }
            }
            if (paramlist.Length > 0)
            {
                return paramlist[paramlist.Length - 1];
            }
            else
            {
                return null;
            }
        }

    }
}

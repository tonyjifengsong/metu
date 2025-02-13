using METU.CONFIGS;
using METU.INTERFACE;
using METU.LOG;
using METU.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class WebServiceBase : IService
    {
        public string id { get; set; }

        private ICoreBusiness GetService(string servicename)
        {
            ICoreBusiness Isvc = null;
            if (DLLASMHelper.GetDllFullPathWithName().Count > 0)
            {
                foreach (var item in DLLASMHelper.GetDllFullPathWithName())
                {

                    Assembly assembly = Assembly.LoadFrom(item.Value);
                    Type[] list = null;
                    try
                    {
                        list = assembly.GetTypes();
                    }
                    catch (Exception ex)
                    {
                        LogCache.WriteLog(item.Value as string + "无法正常加载！可能会导致系统运行不稳定！");
                        continue;
                    }
                    foreach (Type t in list)
                    {

                        if (t.Name.ToUpper() == servicename.ToUpper())
                        {

                            if (t.GetInterface("ICoreBusiness") != null)
                            {
                                string pathname = t.FullName;
                                Type type = assembly.GetType(pathname);
                                Isvc = (ICoreBusiness)System.Activator.CreateInstance(type);
                                break;
                            }
                            else
                            {
                                LogCache.WriteLog(t.FullName + "类没有继承接口ICoreBusiness");
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in DLLASMHelper.GetAllAssemblies())
                {
                    Type[] list = null;
                    try
                    {
                        list = item.GetTypes();
                    }
                    catch (Exception ex)
                    {
                        LogCache.WriteLog(item.FullName as string + "无法正常加载！可能会导致系统运行不稳定！");
                        continue;
                    }
                    foreach (Type t in list)
                    {

                        if (t.Name.ToUpper() == servicename.ToUpper())
                        {

                            if (t.GetInterface("ICoreBusiness") != null)
                            {

                                Isvc = (ICoreBusiness)System.Activator.CreateInstance(t);
                                break;
                            }
                        }
                    }
                }
            }
            return Isvc;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="servicename">服务层类名称</param>
        /// <param name="dic">传入参数</param>
        /// <returns>返回Results</returns>
        public virtual dynamic ExecuteService(Dictionary<string, string> dic, string servicename = null)
        {
            ICoreBusiness Isvc;
            object rs = new object();
            if (!servicename.CheckStringValue()) servicename = dic.BLLName();
            if (servicename.CheckStringValue())
            {
                Isvc = GetService(servicename);
                if (Isvc != null) rs = Isvc.DoWork(dic.BLLActionName(), dic, null);
            }
            return rs;
        }
        public virtual dynamic ExecuteService(Dictionary<string, object> dic, string servicename = null)
        {
            ICoreBusiness Isvc;
            object rs = new object();
            Result rss = new Result();
            if (!servicename.CheckStringValue()) servicename = dic.BLLName();
            if (servicename.CheckStringValue())
            {
                Isvc = GetService(servicename);
                if (Isvc != null) rs = Isvc.DoWork(dic.BLLActionName(), dic, rss);
            }
            return rs;
        }


        /// <summary>
        /// 执行Service层服务
        /// </summary>
        /// <param name="servicename">服务层类名称</param>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        public virtual dynamic ExecuteService(string servicename = null, params object[] paramlist)
        {
            ICoreBusiness Isvc;
            object rs = new object();
            Isvc = GetService(servicename);
            if (Isvc != null) rs = Isvc.DoWork(paramlist);
            return rs;

        }
        public dynamic ExecuteService(string servicename = null, string actionname = null, params object[] paramlist)
        {
            ICoreBusiness Isvc;
            object rs = new object();
            Isvc = GetService(servicename);
            if (Isvc != null)
                rs = Isvc.DoWork(actionname, paramlist);
            return rs;

        }

        public bool CheckValidate(Dictionary<string, object> Dic)
        {
            return false;
        }

        public DataTable ExecuteDt(Dictionary<string, object> Dic)
        {
            return new DataTable ();
        }

        public object Execute(Dictionary<string, object> Dic)
        {
            throw new NotImplementedException();
        }

        public object DoBefore(Dictionary<string, object> Dic)
        {
            return "";
        }

        public object DoAfter(Dictionary<string, object> Dic)
        {
            return "";
        }

        public bool CheckReturnValidate(Dictionary<string, object> Dic)
        {
            return false;
        }

        public string ExecuteDtByTemplate(Dictionary<string, object> Dic)
        {
            return "";
        }

        public string GetID()
        {
           return Guid.NewGuid().ToString();
        }
    }
}

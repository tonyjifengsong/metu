using METU.CONFIGS;
using METU.INTERFACE;
using METU.LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    /// <summary>
    ///  created by  tony  2017-12-26
    /// </summary>
    public partial class BLLCallHelper
    {
        /// <summary>
        /// 获取所有服务列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> GetServieList()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var item in DLLASMHelper.GetDllFullPathWithName())
            {

                Assembly assembly = Assembly.LoadFrom(item.Value);

                Type[] lists;
                try
                {
                    lists = assembly.GetTypes();
                }
                catch (Exception ex)
                {
                    LogCache.WriteLog("DLL位置：" + item.Value);

                    LogCache.WriteLog(ex.Message);

                    continue;
                }
                foreach (Type t in lists)
                {
                    IBusiness Isvc;
                    if (t.GetInterface("IBusiness") != null)
                    {
                        try
                        {
                            Isvc = (IBusiness)System.Activator.CreateInstance(t);
                        }
                        catch (Exception ex)
                        {
                            LogCache.WriteLog("DLL位置：" + item.Value);

                            LogCache.WriteLog(ex.Message);

                            continue;
                        }
                        string key = item.Key + "." + t.Name;
                        if (!result.Keys.Contains(key))
                        {
                            result.Add(key, Isvc);
                        }
                    }
                }
            }
            return result;
        }


        public static IBusiness GetMethod(string servicename)
        {
            IBusiness Isvc = null;
            foreach (var item in DLLASMHelper.GetAllAssemblies())
            {
                Type[] lists;
                try
                {
                    lists = item.GetTypes();
                }
                catch (Exception ex)
                {

                    LogCache.WriteLog(ex.Message);

                    continue;
                }

                foreach (Type t in lists)
                {
                    if (t.Name.ToUpper() == servicename.ToUpper())
                    {
                        if (t.GetInterface("IBusiness") != null)
                        {
                            try
                            {
                                Isvc = (IBusiness)System.Activator.CreateInstance(t);
                            }
                            catch (Exception ex)
                            {

                                LogCache.WriteLog(ex.Message);

                                break;
                            }

                            break;

                        }
                    }
                }
            }
            return Isvc;
        }
        public static object GetObject(string servicename, string interfacename = null)
        {
            object Isvc = null;
            foreach (var item in DLLASMHelper.GetDllFullPathWithName())
            {
                Assembly assembly = Assembly.LoadFrom(item.Value);
                Type[] lists;
                try
                {
                    lists = assembly.GetTypes();
                }
                catch (Exception ex)
                {

                    LogCache.WriteLog(ex.Message);

                    continue;
                }

                foreach (Type t in lists)
                {

                    if (t.Name.ToUpper() == servicename.ToUpper())
                    {
                        if (interfacename != null)
                        {
                            if (t.GetInterface(interfacename) != null)
                            {
                                try
                                {
                                    Isvc = System.Activator.CreateInstance(t);
                                }
                                catch (Exception ex)
                                {

                                    LogCache.WriteLog(ex.Message);


                                }
                                break;


                            }
                        }
                        else
                        {
                            try
                            {
                                Isvc = System.Activator.CreateInstance(t);
                            }
                            catch (Exception ex)
                            {

                                LogCache.WriteLog(ex.Message);


                            }
                            break;
                        }
                    }
                }
            }
            return Isvc;
        }


    }
}

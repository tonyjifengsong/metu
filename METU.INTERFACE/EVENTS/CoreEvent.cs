using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.EVENTS
{
    public class CoreEvent<T> : IFEvent<T>
    {
        public bool isSuccess { get; set; }
        public string MSG { get; set; }

        public event Func<T, object> BusinessBus;
        public event Action<T, object> Business;
        public void DoAction(T paramlist, object obj)
        {
            isSuccess = true;
            if (Business != null)
            {
                try
                {
                    Business(paramlist, obj);
                }
                catch(Exception ex)
                {
                    isSuccess = false;
                    MSG = ex.Message;
                }
            }
        }


        public dynamic DoWork(T paramlist, EventResultType resultType = EventResultType.LAST)
        {
            isSuccess = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            object rs = null;
            if (resultType == EventResultType.LAST)
            {
                if (BusinessBus != null)
                {
                    try
                    {
                        rs = BusinessBus(paramlist);
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
                    try { 
                    rs = BusinessBus(paramlist);
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
                    foreach (Func<T, object> itm in dlist)
                    {
                        try
                        {
                            var rsobj = itm(paramlist);
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

    }
}

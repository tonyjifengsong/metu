using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.EVENTS
{
    public class Events : IEvent
    {
        public bool isSuccess { get; set; }
        public string MSG { get; set; }

        public event Func<Dictionary<string, string>, object> BusinessBus;
        public event Func<Dictionary<string, object>, object> BsBus;
        public event Action<Dictionary<string, string>, object> Business;
        public event Action<Dictionary<string, object>, object> Bs;

        public void DoAction(object paramlist, object obj)
        {
            isSuccess = true;
            if(paramlist is Dictionary<string, string>)
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

        public dynamic DoWork(object paramlist, EventResultType resultType = EventResultType.LAST)
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
                        try { 
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
                        try { 
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
                        try { 
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
                        try { 
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.EVENTS
{
    public class EventPredicate : IEventPredicate
    {
        public string MSG { get; set; }
        public bool isSuccess { get; set; }

        public event Predicate<Dictionary<string, string>> BusinessBus;
        public event Predicate<Dictionary<string, object>> BsBus;
        public event Predicate<object> BoBus;

        public dynamic DoWork(object paramlist, EventResultType resultType = EventResultType.LAST)
        {
            isSuccess = true;
            bool result = true;
            bool rs = false;
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

                    return rs;
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

                    return rs;
                }
                else
                {
                    if (BusinessBus != null)
                    {
                        Delegate[] dlist = BusinessBus.GetInvocationList();
                        foreach (Predicate<Dictionary<string, string>> itm in dlist)
                        {
                            try
                            {

                                bool rsobj = itm((Dictionary<string, string>)paramlist);
                                result = result && rsobj;
                            }
                            catch (Exception ex)
                            {
                                isSuccess = false;
                                MSG = ex.Message;

                                result = false;
                                break;
                            }

                        }
                        return result;
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

                    return rs;
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

                    return rs;
                }
                else
                {
                    if (BsBus != null)
                    {
                        Delegate[] dlist = BsBus.GetInvocationList();
                        foreach (Predicate<Dictionary<string, object>> itm in dlist)
                        {
                            try
                            {
                                bool rsobj = itm((Dictionary<string, object>)paramlist);
                                result = result && rsobj;
                            }
                            catch (Exception ex)
                            {
                                isSuccess = false;
                                MSG = ex.Message;

                                result = false;
                                break;
                            }

                        }
                        return result;
                    }
                }
                return null;
            }
            else
            {
                if (resultType == EventResultType.LAST)
                {
                    if (BoBus != null)
                    {
                        try
                        {
                            rs = BoBus(paramlist);
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            MSG = ex.Message;
                        }
                    }

                    return rs;
                }
                if (resultType == EventResultType.NONE)
                {
                    if (BoBus != null)
                    {
                        try
                        {
                            rs = BoBus(paramlist);
                        }
                        catch (Exception ex)
                        {
                            isSuccess = false;
                            MSG = ex.Message;
                        }
                    }

                    return rs;
                }
                else
                {
                    if (BoBus != null)
                    {
                        Delegate[] dlist = BoBus.GetInvocationList();
                        foreach (Predicate<object> itm in dlist)
                        {
                            try
                            {
                                bool rsobj = itm(paramlist);
                                result = result && rsobj;
                            }
                            catch (Exception ex)
                            {
                                isSuccess = false;
                                MSG = ex.Message;

                                result = false;
                                break;
                            }

                        }
                        return result;
                    }
                }
                return null;
            }
            return null;
        }
    }
}

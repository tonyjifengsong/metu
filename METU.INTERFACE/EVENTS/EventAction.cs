using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE.EVENTS
{
    public class EventAction : IEventInit
    {
        public string MSG { get ; set; }
        public bool isSuccess { get ; set; }

        public event Action<Dictionary<string, string>, object> BusinessBus;
        public event Action<Dictionary<string, object>, object> BsBus;

        public void ClearEventBus()
        {
            
        }

        public void ClearEventQueue()
        {
            
        }

        public void DoQueueWork(Dictionary<string, string> dic, object obj)
        {
            
        }

        public void DoWork(object paramlist, object obj)
        {
            isSuccess = true;
            if (paramlist is Dictionary<string, string>)
            {
                if (BusinessBus != null)
                {
                    try
                    {
                        BusinessBus((Dictionary<string, string>)paramlist, obj);
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
                if (BsBus != null)
                {
                    try
                    {
                        BsBus((Dictionary<string, object>)paramlist, obj);
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        MSG = ex.Message;
                    }
                }
            }
        }

        public void InitialBus()
        {
            
        }

        public void InitialQueue()
        {
            
        }
    }
}

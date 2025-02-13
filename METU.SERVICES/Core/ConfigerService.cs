using System;
using System.Collections.Generic;

namespace METU.SERVICES.Core
{
    /// <summary>
    /// Created by tony
    /// </summary>
    public class ConfigerService : IServiceFilter
    {
        public QueueReturnList Services = new QueueReturnList();
        public bool DoFilter(Dictionary<string, string> dic = null)
        {

            for (int i = 0; i < Services.GetQueueCount(); i++)
            {
                if (Services.IsHaveElement())
                {
                    var itm = Services.Get();
                    bool result = (bool)itm(dic);
                    if (!result) return false;
                }
            }
            return true;
        }
    }
    public interface IServiceFilter
    {
        bool DoFilter(Dictionary<string, string> dic = null);

    }
}

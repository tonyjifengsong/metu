using System.Collections.Generic;

namespace METU.INTERFACE
{

    public interface IWinService
    {
        Dictionary<string, object> GetConfigs();
        bool IsExecuteService();
        void ExecuteService(object Context = null);

    }
}

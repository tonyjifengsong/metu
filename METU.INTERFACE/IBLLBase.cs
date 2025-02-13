using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    public interface IBLLBase : IBase, IService
    {
        object DoWork(dynamic Model);
        List<object> DoWorks(dynamic Model);
        string ExecuteDicReturnString(Dictionary<string, string> Dic);
        bool ExecuteDicBool(Dictionary<string, string> Dic);
        Dictionary<string, string> ExecuteDic(Dictionary<string, string> Dic);

    }
}

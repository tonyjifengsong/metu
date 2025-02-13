using System.Collections.Generic;

namespace METU.CONFIGS.PipeLine
{
    public interface IArgs
    {
        Dictionary<string, object> DicContext { get; set; }
        object Context { get; set; }
        string TraceID { get; set; }
        string Name { get; set; }
        long Order { get; set; }
        string Evn { get; set; }
    }
}

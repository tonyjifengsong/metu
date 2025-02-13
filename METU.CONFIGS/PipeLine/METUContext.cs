using System;
using System.Collections.Generic;

namespace METU.CONFIGS.PipeLine
{
    public class METUContext : IArgs
    {
        public METUContext() {
            TraceID = Guid.NewGuid().ToString();
            Order = DateTime.Now.Ticks;
            DicContext = new Dictionary<string, object>();
            Evn = "Development";
            Context = new object();
            Name = TraceID;
        }
        public Dictionary<string,object> DicContext { get; set; }
        public object Context { get ; set ; }
        public string TraceID { get ; set ; }
        public string Name { get ; set ; }
        public long Order { get  ; set ; }
        public string Evn { get  ; set ; }
    }
}

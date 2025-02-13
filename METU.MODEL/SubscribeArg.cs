using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    [DataContract]
    public class SubscribeArg : ArgumentBase<int>
    {
        [DataMember]
        public String Username { get; set; }
        public List<int> Alarms { get; set; }
        public SubscribeArg()
        {
            Alarms = new List<int>();
        }
    }
}

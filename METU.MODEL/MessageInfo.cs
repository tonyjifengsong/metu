using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    [DataContract]
    public class MessageInfo
    {
        [DataMember]
        public string Receipt { get; set; }
        [DataMember]
        public string ReceiveMode { get; set; }
        [DataMember]
        public string ReceiveOrgan { get; set; }
        [DataMember]
        public string ReceiveUser { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Sender { get; set; }
        [DataMember]
        public string SendTime { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{

    [DataContract]
    public class ConfigModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string IP { get; set; }
        [DataMember]
        public string Port { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string UserPassword { get; set; }
        [DataMember]
        public string Desc { get; set; }
        [DataMember]
        public string Age { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ServerIP { get; set; }
        [DataMember]
        public string DataBaseName { get; set; }
        [DataMember]
        public string DataBaseUserName { get; set; }
        [DataMember]
        public string DataBaseUserPassword { get; set; }
    }
}

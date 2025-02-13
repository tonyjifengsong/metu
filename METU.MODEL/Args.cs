using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    [DataContract]
    /// <summary>
    /// 通用参数
    /// </summary>
    public  class Args: ArgumentBase<object>
    {
        [DataMember]
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 企业ID
        /// </summary>
        [DataMember]
        public string CID
        {
            get;
            set;
        }
        /// <summary>
        /// 企业ID
        /// </summary>
        [DataMember]
        public string BID
        {
            get;
            set;
        }
        /// <summary>
        /// Token
        /// </summary>
        [DataMember]
        public string Token
        {
            get;
            set;
        }
        /// <summary>
        /// AK
        /// </summary>
        [DataMember]
        public string AK
        {
            get;
            set;
        }
        /// <summary>
        /// SK
        /// </summary>
        [DataMember]
        public string SK
        {
            get;
            set;
        }
        /// <summary>
        /// APPID
        /// </summary>
        [DataMember]
        public string APPID
        {
            get;
            set;
        }
        /// <summary>
        /// PID
        /// </summary>
        [DataMember]
        public string PID
        {
            get;
            set;
        }
    }
}

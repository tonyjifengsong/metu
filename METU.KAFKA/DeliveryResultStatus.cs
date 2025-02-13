using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
    public enum DeliveryResultStatus
    {
        /// <summary>
        /// 消息提交失败
        /// </summary>
        NotPersisted = 0,
        /// <summary>
        /// 消息已提交，是否成功未知
        /// </summary>
        PossiblyPersisted = 1,
        /// <summary>
        /// 消息提交成功
        /// </summary>
        Persisted = 2
    }
}

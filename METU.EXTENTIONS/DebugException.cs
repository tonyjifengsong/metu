using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{/// <summary>
/// 
/// </summary>
    public class DebugException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public DebugException(string code, object msg)
        {
            this.Data.Add("Code", code);
            this.Data.Add("Msg", msg);
          
        }
        /// <summary>
        /// 
        /// </summary>
        public override string Message
        {
            get
            {

                return this.Data.toJson();
            }

        }
    }
}

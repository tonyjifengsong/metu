using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    [Serializable]
    public class ArgumentBase<T>
    {
        private int code;
        private string msg;
        private T model;

        public int Code
        {
            get { return code; }
            set { code = value; }
        }
        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
        public T Data
        {
            get { return model; }
            set { model = value; }

        }
    }
}

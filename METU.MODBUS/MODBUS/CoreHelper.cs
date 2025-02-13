using System;
using System.Collections.Generic;
using System.Text;

namespace METU.MODBUS.MODBUS
{
    public class CoreHelper
    {
        #region 大小端判断
        public static bool LittleEndian = false;

        static CoreHelper()
        {
            unsafe
            {
                int tester = 1;
                LittleEndian = (*(byte*)(&tester)) == (byte)1;
            }
        }
        #endregion

        #region Factory
        public static CoreHelper? _Instance = null;
        internal static CoreHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = LittleEndian ? new LetterCoreHelper() : new CoreHelper();
                }
                return _Instance;
            }
        }
        #endregion

        protected CoreHelper()
        {

        }

        public virtual Byte[] GetBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual Byte[] GetBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual Byte[] GetBytes(float value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual Byte[] GetBytes(double value)
        {
            return BitConverter.GetBytes(value);
        }

        public virtual short GetShort(byte[] data)
        {
            return BitConverter.ToInt16(data, 0);
        }

        public virtual int GetInt(byte[] data)
        {
            return BitConverter.ToInt32(data, 0);
        }

        public virtual float GetFloat(byte[] data)
        {
            return BitConverter.ToSingle(data, 0);
        }

        public virtual double GetDouble(byte[] data)
        {
            return BitConverter.ToDouble(data, 0);
        }
    }

    
}

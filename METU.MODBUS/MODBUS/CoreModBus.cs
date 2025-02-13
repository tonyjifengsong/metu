using System;
using System.Collections.Generic;
using System.Text;

namespace METU.MODBUS.MODBUS
{
   

    public abstract class CoreModBus : IDisposable
    {
        private static CoreModBus? _Instance;

        public static CoreModBus CreateInstance(Protocol protocol)
        {
            if (_Instance == null)
            {
                switch (protocol)
                {
                    case Protocol.TCPIP:
                        _Instance = new CoreModBusTCPIP();
                        break;
                    case Protocol.SerialPort:
                        _Instance = new CoreModBusSerialPort();
                        break;
                    default:
                        _Instance = new CoreModBusSerialPort();
                        break;
                }
            }
            return _Instance;
        }


        #region Transaction Identifier
        /// <summary>
        /// 数据序号标识
        /// </summary>
        private byte dataIndex = 0;

        protected byte CurrentDataIndex
        {
            get { return this.dataIndex; }
        }

        protected byte NextDataIndex()
        {
            return ++this.dataIndex;
        }
        #endregion

      

        public abstract void Connect();

        public abstract byte[] Receive();

        public abstract void Send(byte[] data);
        public abstract string ReceiveData();

        public abstract void SendData(string data);
        #region IDisposable 成员
        public virtual void Dispose()
        {
        }
        #endregion
    }
}

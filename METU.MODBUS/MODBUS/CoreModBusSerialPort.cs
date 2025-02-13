using System;
using System.Collections.Generic;
using System.Text;

namespace METU.MODBUS.MODBUS
{
    internal class CoreModBusSerialPort : CoreModBus, IDisposable
    {
        public override void Connect()
        {
            throw new NotImplementedException();
        }

        public override byte[] Receive()
        {
            throw new NotImplementedException();
        }

        public override void Send(byte[] data)
        {
            throw new NotImplementedException();
        }
        public override string ReceiveData()
        {
            return Encoding.ASCII.GetString(Receive());
        }

        public override void SendData(string data)
        {
            Send(Encoding.ASCII.GetBytes(data.Trim()));
        }
        #region IDisposable 成员
        public override void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

using System.Text;
using System.Net.Sockets;
using System.Net;
using METU.Core;

namespace METU.MODBUS.MODBUS
{
    internal class CoreSocket : IDisposable
    {
        private static string IP = ConfigHelper.GetConfigSettings("serialport:ip");
        private static int Port = Int32.Parse(ConfigHelper.GetConfigSettings("serialport:port"));
        private static int TimeOut = Int32.Parse(ConfigHelper.GetConfigSettings("serialport:timeout"));

     
        private Socket? socket;

        public void Connect()
        {    if (socket != null)
            {
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, TimeOut);

                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(IP), Port);
                this.socket.Connect(ip);
            }
        }

        public byte[] Read(int length)
        {
            byte[] data = new byte[length];
            if (socket != null) socket.Receive(data);
           FileHelper.Writelog("Receive:" );
            FileHelper.Writelog(  data);
            return data;
        }

        public void Write(byte[] data)
        { 
            FileHelper.Writelog("Send:");
            FileHelper.Writelog(data);
          if(socket!=null)   socket.Send(data);
        }
 

        #region IDisposable 成员
        public void Dispose()
        {
            if (this.socket != null)
            {
                this.socket.Close();
            }
        }
        #endregion
    }
}

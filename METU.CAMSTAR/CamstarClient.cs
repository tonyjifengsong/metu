using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{
    public class ServerConnection
    {
        private const int BufferSize = 0x124f8;
        public string mHostName = "127.0.0.1";
        public int mPortNum = 2881;


        public ServerConnection(string hostName, int port)
        {
            mHostName = hostName;
            mPortNum = port;
        }

        protected string Receive(TcpClient tcpClient, NetworkStream stream)
        {
            string str = "";
            int index = 0;
            byte[] buffer = new byte[tcpClient.ReceiveBufferSize + 1];
            do
            {
                index = stream.Read(buffer, 0, tcpClient.ReceiveBufferSize);
                if (index > 0)
                {
                    if ((index % 2) == 1)
                    {
                        index--;
                    }
                    str = str + Encoding.Unicode.GetString(buffer, 0, index);
                }
            }
            while ((index > 0) || stream.DataAvailable);
            return str;
        }

        protected void Send(NetworkStream stream, string sendDoc)
        {
            sendDoc = sendDoc + "\0";
            byte[] bytes = Encoding.Unicode.GetBytes(sendDoc);
            stream.Write(bytes, 0, bytes.Length);
        }

        public string Submit(string sendDoc)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(System.Net.IPAddress.Parse(mHostName), mPortNum);


            tcpClient.SendBufferSize = 0x124f8;

            NetworkStream stream = tcpClient.GetStream();
            this.Send(stream, sendDoc);
            string str = this.Receive(tcpClient, stream);
            stream.Close();
            tcpClient.Close();

            return str;
        }
    }
}

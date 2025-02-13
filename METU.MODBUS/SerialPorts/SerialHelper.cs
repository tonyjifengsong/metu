using System.IO.Ports;
using System.Text;

namespace METU.SerialAPI.SerialPorts
{
    public class SerialHelper
    {
        #region 字段/属性/委托
        /// <summary>
        /// 串行端口对象
        /// </summary>
        private SerialPort sp;

        /// <summary>
        /// 串口接收数据委托
        /// </summary>
        public delegate void ComReceiveDataHandler(string data);

        public ComReceiveDataHandler? OnComReceiveDataHandler = null;

        /// <summary>
        /// 端口名称数组
        /// </summary>
        public string[] PortNameArr { get; set; }

        /// <summary>
        /// 串口通信开启状态
        /// </summary>
        public bool PortState { get; set; } = false;

        /// <summary>
        /// 编码类型
        /// </summary>
        public Encoding EncodingType { get; set; } = Encoding.ASCII;
        #endregion

        #region 方法
        public SerialHelper()
        {
            PortNameArr = SerialPort.GetPortNames();
            sp = new SerialPort();
            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="portName">端口名称</param>
        /// <param name="boudRate">波特率</param>
        /// <param name="dataBit">数据位</param>
        /// <param name="stopBit">停止位</param>
        /// <param name="timeout">超时时间</param>
        public void OpenPort(string portName, int boudRate = 115200, int dataBit = 8, int stopBit = 1, int timeout = 5000)
        {
            try
            {
                sp.PortName = portName;
                sp.BaudRate = boudRate;
                sp.DataBits = dataBit;
                sp.StopBits = (StopBits)stopBit;
                sp.ReadTimeout = timeout;
                sp.Open();
                PortState = true;
            }
            catch (Exception e)
            {
                FileHelper.Writelog(e.Message);
            }
        }
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="portName">端口名称</param>
        /// <param name="boudRate">波特率</param>
        /// <param name="dataBit">数据位</param>
        /// <param name="stopBit">停止位</param>
        /// <param name="timeout">超时时间</param>
        public object OpenAndSendData(string portName, int boudRate = 115200, int dataBit = 8, int stopBit = 1, int timeout = 5000,string sendData="test")
        {
            SerialPort sp1 = new SerialPort();
            try
            {
                sp1.PortName = portName;
                sp1.BaudRate = boudRate;
                sp1.DataBits = dataBit;
                sp1.StopBits = (StopBits)stopBit;
                sp1.ReadTimeout = timeout;               
                sp1.Open();
                PortState = true;              
                    sp1.Encoding = EncodingType;
                    sp1.Write(sendData);
                return "true";
            }
            catch (Exception e)
            {
                FileHelper.Writelog(e.Message);
                return e.Message;
            }
        }
        public object OpenSendData(string portName,string parity, string boudRate = "115200", string dataBit = "8", string stopBit = "1",string flowcontroller="", string sendData = "test", int timeout = 5000)
        {
            int boudRate1 = 115200;
            int dataBit1 = 8;
            int stopBit1 = 1;
            if (int.TryParse(boudRate,out boudRate1))
            {
                boudRate1 = 115200;
            }
            if (int.TryParse(dataBit, out dataBit1))
            {
                dataBit1 =8;
            }
            if (int.TryParse(stopBit, out stopBit1))
            {
                stopBit1 = 1;
            }
           if( boudRate1==(int)BaudRates.BR_75 || boudRate1 == (int)BaudRates.BR_110||
                boudRate1 == (int)BaudRates.BR_115200 || boudRate1 == (int)BaudRates.BR_38400 ||
                boudRate1 == (int)BaudRates.BR_1200 || boudRate1 == (int)BaudRates.BR_128000 ||
                boudRate1 == (int)BaudRates.BR_14400 || boudRate1 == (int)BaudRates.BR_150 ||
                boudRate1 == (int)BaudRates.BR_9600 || boudRate1 == (int)BaudRates.BR_600 ||
                boudRate1 == (int)BaudRates.BR_57600 || boudRate1 == (int)BaudRates.BR_4800 ||
                boudRate1 == (int)BaudRates.BR_230400 || boudRate1 == (int)BaudRates.BR_2400 ||
                boudRate1 == (int)BaudRates.BR_300 || boudRate1 == (int)BaudRates.BR_19200||
                boudRate1 == (int)BaudRates.BR_56000  )
            {

            }
            else
            {
                boudRate1 = 9600;
            }

            SerialPort sp1 = new SerialPort();
            try
            {
               
                if (parity.ToLower() == "odd")
                {
                    sp1.Parity = Parity.Odd;
                }
                if (parity.ToLower() == "even")
                {
                    sp1.Parity = Parity.Even;
                }
                if (parity.ToLower() == "mark")
                {
                    sp1.Parity = Parity.Mark;
                }
                if (parity.ToLower() == "space")
                {
                    sp1.Parity = Parity.Space;
                }
                else if (parity.ToLower() == "none")
                {
                    sp1.Parity = Parity.None;
                }
                if (stopBit1 == 3)
                {
                    sp1.StopBits = StopBits.OnePointFive;
                }
                if (stopBit1 == 2)
                {
                    sp1.StopBits = StopBits.Two;
                }
                if (stopBit1 == 1)
                {
                    sp1.StopBits = StopBits.One;
                }else
                if (stopBit1 == 0)
                {
                    sp1.StopBits = StopBits.None;
                }
                if (flowcontroller.ToLower() == "none")
                {
 sp1.Handshake = Handshake.None;
                }
                if (flowcontroller.ToLower() == "RequestToSend".ToLower())
                {
                    sp1.Handshake = Handshake.RequestToSend;
                }
                if (flowcontroller.ToLower() == "XOnXOff".ToLower())
                {
                    sp1.Handshake = Handshake.XOnXOff;
                }
                if (flowcontroller.ToLower() == "RequestToSendXOnXOff".ToLower())
                {
                    sp1.Handshake = Handshake.RequestToSendXOnXOff;
                }

                sp1.PortName = portName;
                sp1.BaudRate = boudRate1;
                sp1.DataBits = dataBit1;
                sp1.ReadTimeout = timeout;
                if(!sp1.IsOpen)sp1.Open();
                sp1.Encoding = EncodingType;
                sp1.Write(sendData);
                sp1.Close();               
                return "true";
            }
            catch (Exception e)
            {
                FileHelper.Writelog(e.Message);
                return e.Message;
            }
        }
        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            try
            {
                sp.Close();
                PortState = false;
            }
            catch (Exception e)
            {
                FileHelper.Writelog(e.Message);
            }
        }
        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort(SerialPort sp)
        {
            try
            {
                sp.Close();
                PortState = false;
            }
            catch (Exception e)
            {
                FileHelper.Writelog(e.Message);
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendData"></param>
        public void SendData(string sendData)
        {
            try
            {
                sp.Encoding = EncodingType;
                sp.Write(sendData);
            }
            catch (Exception e)
            {
                FileHelper.Writelog(e.Message);
            }
        }

        /// <summary>
        /// 接收数据回调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[sp.BytesToRead];
            sp.Read(buffer, 0, buffer.Length);
            string str = EncodingType.GetString(buffer);
            if (OnComReceiveDataHandler != null)
            {
                OnComReceiveDataHandler(str);
            }
        }
        #endregion
    }
}

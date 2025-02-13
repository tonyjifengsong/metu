using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace METU.MODBUS.MODBUS
{
    public class ModbusTcp : IDisposable
    {
        public ModbusTcp(int _Port, string _Ip)
        {
            _ip = _Ip;
            _port = _Port;
            _online = false;
            Ping ping = new Ping();
            try
            {
                PingReply pingReply = ping.Send(_Ip);
                if (pingReply.Status == IPStatus.Success)
                {
                    _online = true;
                    // Console.WriteLine("当前在线，已ping通！");
                }
                else
                {
                    _online = false;
                }
            }
            catch
            {


            }
        }
        public ModbusTcp()
        {
        }
        public string _ip = "127.0.0.1";
        public int _port = 80;
        public bool _online = false;
        /// <summary>
        /// 获取服务器是否在线
        /// </summary>
        public bool _conn { get; }
        private static Socket _SocketClient=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // private Socket clientSocket;
        private Socket _socket
        {
            get
            {


                if (_SocketClient == null)
                {
                    int port = _port;
                    string host = _ip;//服务器端ip地址
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port);
                    _SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _SocketClient.Connect(ipe);
                    return _SocketClient;
                }
                //掉线重连
                if (!_SocketClient.Connected)
                {
                    _SocketClient.Close();
                    int port = _port;
                    string host = _ip;//服务器端ip地址
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port);
                    _SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _SocketClient.Connect(ipe);
                    return _SocketClient;
                }


                return _SocketClient;
            }
        }
        /// <summary>
        /// 读取位状态
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public bool[] _bite(int _addr, int _len)
        {
            #region
            /*——————————————————————————————————————————————————————————
             * MODBUS_TCP---01功能码说明
             * 读取多个位状态
             * 发送报文：
             * 00 00 00 00 00 06（此字节之后共有多少字节） 01（站号）01（功能码）61（地址高位）80（地址低位） 00（读取数量高位） 0A （读取数量低位） 
             * 返回报文：
             * 00 00 00 00 00 05 01 01 02（读取结果有多少个字节） 33（0---7的位寄存） 03（8---15的位寄存） 
             * 读取的结果以字节存储，8位一个字节，读取10个位则需要两个字节存储状态。将字节拆分，即为单个位状态
             * 关于返回值：
             *    返回 bool 数组，数组的0号值为连续读取位  的首地址位状态，数组1号值为读取的第二个地址的位状态
             * ——————————————————————————————————————————————————
            */
            #endregion
            bool[] _bites = new bool[1000];
            if (_online)
            {
                if (_len >= 1)
                {
                    int _LEN = _len * 1;
                    byte[] _recebyte = new byte[1000];
                    byte _lenH = 0;
                    byte _lenL = 1;
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _lengbyte = BitConverter.GetBytes(_LEN);
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    if (_LEN > 0xFF)
                    {
                        _lenL = _lengbyte[_lengbyte.Length - 3];
                        _lenH = _lengbyte[_lengbyte.Length - 4];
                    }
                    if (_LEN <= 0xFF)
                    {
                        _lenL = 0;
                        _lenH = _lengbyte[0];
                    }
                    if (_addr > 0xFF)
                    {
                        _addrL = _addrbyte[_addrbyte.Length - 3];
                        _addrH = _addrbyte[_addrbyte.Length - 4];
                    }
                    if (_addr <= 0xFF)
                    {
                        _addrL = 0;
                        _addrH = _addrbyte[0];
                    }
                    sedbyte[0] = 0x00;
                    sedbyte[1] = 0x00;
                    sedbyte[2] = 0x00;
                    sedbyte[3] = 0x00;
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x06;
                    sedbyte[6] = 0x01;
                    sedbyte[7] = 0x01;
                    sedbyte[8] = _addrL;//MODBUS地址低位
                    sedbyte[9] = _addrH;//modbus地址高位
                    sedbyte[10] = _lenL;//读取长度低位
                    sedbyte[11] = _lenH;//读取长度高位
                    try
                    {
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        int _recl1 = _recebyte[5] + 6;


                        if (_recelen == _recl1)
                        {
                            for (int i = 0; i < _recebyte[8]; i++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    _bites[i * 8 + j] = (_recebyte[9 + i] & Convert.ToInt16(Math.Pow(2, j))) == Convert.ToInt16(Math.Pow(2, j));
                                }
                            }
                        }
                    }
                    catch
                    {


                    }
                }
            }
            return _bites;
        }
        /// <summary>
        /// 读取单精度浮点数
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public float[] _float(int _addr, int _len)
        {


            float[] _float = new float[100];
            try
            {
                if (_online)
                {
                    if (_len >= 1)
                    {
                        int _LEN = _len * 2;
                        byte[] _recebyte = new byte[1000];
                        byte _lenH = 0;
                        byte _lenL = 1;
                        byte _addrH = 0;
                        byte _addrL = 1;
                        byte[] sedbyte = new byte[20];
                        byte[] _lengbyte = BitConverter.GetBytes(_LEN);
                        byte[] _addrbyte = BitConverter.GetBytes(_addr);
                        if (_LEN > 0xFF)
                        {
                            _lenL = _lengbyte[_lengbyte.Length - 3];
                            _lenH = _lengbyte[_lengbyte.Length - 4];
                        }
                        if (_LEN <= 0xFF)
                        {
                            _lenL = 0;
                            _lenH = _lengbyte[0];
                        }
                        if (_addr > 0xFF)
                        {
                            _addrL = _addrbyte[_addrbyte.Length - 3];
                            _addrH = _addrbyte[_addrbyte.Length - 4];
                        }
                        if (_addr <= 0xFF)
                        {
                            _addrL = 0;
                            _addrH = _addrbyte[0];
                        }
                        sedbyte[0] = 0x00;
                        sedbyte[1] = 0x00;
                        sedbyte[2] = 0x00;
                        sedbyte[3] = 0x00;
                        sedbyte[4] = 0x00;
                        sedbyte[5] = 0x06;
                        sedbyte[6] = 0x01;
                        sedbyte[7] = 0x03;
                        sedbyte[8] = _addrL;//MODBUS地址低位
                        sedbyte[9] = _addrH;//modbus地址高位
                        sedbyte[10] = _lenL;//读取长度低位
                        sedbyte[11] = _lenH;//读取长度高位
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        int _s1 = _len * 4 + 9;
                        if (_recelen == _s1)//校验返回的数据
                        {
                            byte[] _bs = new byte[4];
                            //   rec = rec + 1;
                            for (int i = 0; i < _len; i++)
                            {
                                _bs[0] = _recebyte[10 + i * 4];
                                _bs[1] = _recebyte[9 + i * 4];
                                _bs[2] = _recebyte[12 + i * 4];
                                _bs[3] = _recebyte[11 + i * 4];
                                _float[i] = BitConverter.ToSingle(_bs, 0);
                            }
                            //  MessageBox.Show("ok!");
                        }
                    }
                }
            }
            catch
            {


            }
            return _float;
        }
        /// <summary>
        /// 读取有符号32位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public Int32[] _int32(int _addr, int _len)
        {
            Int32[] _int32 = new Int32[100];
            try
            {
                if (_online)
                {
                    if (_len >= 1)
                    {
                        int _LEN = _len * 2;
                        byte[] _recebyte = new byte[1000];
                        byte _lenH = 0;
                        byte _lenL = 1;
                        byte _addrH = 0;
                        byte _addrL = 1;
                        byte[] sedbyte = new byte[20];
                        byte[] _lengbyte = BitConverter.GetBytes(_LEN);
                        byte[] _addrbyte = BitConverter.GetBytes(_addr);
                        if (_LEN > 0xFF)
                        {
                            _lenL = _lengbyte[_lengbyte.Length - 3];
                            _lenH = _lengbyte[_lengbyte.Length - 4];
                        }
                        if (_LEN <= 0xFF)
                        {
                            _lenL = 0;
                            _lenH = _lengbyte[0];
                        }
                        if (_addr > 0xFF)
                        {
                            _addrL = _addrbyte[_addrbyte.Length - 3];
                            _addrH = _addrbyte[_addrbyte.Length - 4];
                        }
                        if (_addr <= 0xFF)
                        {
                            _addrL = 0;
                            _addrH = _addrbyte[0];
                        }
                        sedbyte[0] = 0x00;
                        sedbyte[1] = 0x00;
                        sedbyte[2] = 0x00;
                        sedbyte[3] = 0x00;
                        sedbyte[4] = 0x00;
                        sedbyte[5] = 0x06;
                        sedbyte[6] = 0x01;
                        sedbyte[7] = 0x03;
                        sedbyte[8] = _addrL;//MODBUS地址低位
                        sedbyte[9] = _addrH;//modbus地址高位
                        sedbyte[10] = _lenL;//读取长度低位
                        sedbyte[11] = _lenH;//读取长度高位
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        int _s1 = _len * 4 + 9;
                        if (_recelen == _s1)//校验返回的数据
                        {
                            byte[] _bs = new byte[4];
                            //   rec = rec + 1;
                            for (int i = 0; i < _len; i++)
                            {
                                _bs[0] = _recebyte[10 + i * 4];
                                _bs[1] = _recebyte[9 + i * 4];
                                _bs[2] = _recebyte[12 + i * 4];
                                _bs[3] = _recebyte[11 + i * 4];
                                _int32[i] = BitConverter.ToInt32(_bs, 0);
                            }
                            //  MessageBox.Show("ok!");
                        }
                    }
                }
            }
            catch
            {


            }
            return _int32;
        }
        /// <summary>
        /// 读取无符号32位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public uint[] _uint32(int _addr, int _len)
        {
            uint[] _uint = new uint[100];
            try
            {
                if (_online)
                {
                    if (_len >= 1)
                    {
                        int _LEN = _len * 2;
                        byte[] _recebyte = new byte[1000];
                        byte _lenH = 0;
                        byte _lenL = 1;
                        byte _addrH = 0;
                        byte _addrL = 1;
                        byte[] sedbyte = new byte[20];
                        byte[] _lengbyte = BitConverter.GetBytes(_LEN);
                        byte[] _addrbyte = BitConverter.GetBytes(_addr);
                        if (_LEN > 0xFF)
                        {
                            _lenL = _lengbyte[_lengbyte.Length - 3];
                            _lenH = _lengbyte[_lengbyte.Length - 4];
                        }
                        if (_LEN <= 0xFF)
                        {
                            _lenL = 0;
                            _lenH = _lengbyte[0];
                        }
                        if (_addr > 0xFF)
                        {
                            _addrL = _addrbyte[_addrbyte.Length - 3];
                            _addrH = _addrbyte[_addrbyte.Length - 4];
                        }
                        if (_addr <= 0xFF)
                        {
                            _addrL = 0;
                            _addrH = _addrbyte[0];
                        }
                        sedbyte[0] = 0x00;
                        sedbyte[1] = 0x00;
                        sedbyte[2] = 0x00;
                        sedbyte[3] = 0x00;
                        sedbyte[4] = 0x00;
                        sedbyte[5] = 0x06;
                        sedbyte[6] = 0x01;
                        sedbyte[7] = 0x03;
                        sedbyte[8] = _addrL;//MODBUS地址低位
                        sedbyte[9] = _addrH;//modbus地址高位
                        sedbyte[10] = _lenL;//读取长度低位
                        sedbyte[11] = _lenH;//读取长度高位
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        int _s1 = _len * 4 + 9;
                        if (_recelen == _s1)//校验返回的数据
                        {
                            byte[] _bs = new byte[4];
                            // rec = rec + 1;
                            for (int i = 0; i < _len; i++)
                            {
                                _bs[0] = _recebyte[10 + i * 4];
                                _bs[1] = _recebyte[9 + i * 4];
                                _bs[2] = _recebyte[12 + i * 4];
                                _bs[3] = _recebyte[11 + i * 4];
                                _uint[i] = BitConverter.ToUInt32(_bs, 0);
                            }
                            //  MessageBox.Show("ok!");
                        }
                    }
                }
            }
            catch
            {
            }
            return _uint;
        }
        /// <summary>
        /// 读取有符号16位数据
        /// </summary>
        /// <param name="_addr">MOFBUS地址</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public short[] _short(int _addr, int _len)
        {


            short[] _ints = new short[1000];
            try
            {
                if (_online)
                {
                    // float[] _float = new float[100];
                    if (_len >= 1)
                    {
                        int _LEN = _len * 1;
                        byte[] _recebyte = new byte[1000];
                        byte _lenH = 0;
                        byte _lenL = 1;
                        byte _addrH = 0;
                        byte _addrL = 1;
                        byte[] sedbyte = new byte[20];
                        byte[] _lengbyte = BitConverter.GetBytes(_LEN);
                        byte[] _addrbyte = BitConverter.GetBytes(_addr);
                        if (_LEN > 0xFF)
                        {
                            _lenL = _lengbyte[_lengbyte.Length - 3];
                            _lenH = _lengbyte[_lengbyte.Length - 4];
                        }
                        if (_LEN <= 0xFF)
                        {
                            _lenL = 0;
                            _lenH = _lengbyte[0];
                        }
                        if (_addr > 0xFF)
                        {
                            _addrL = _addrbyte[_addrbyte.Length - 3];
                            _addrH = _addrbyte[_addrbyte.Length - 4];
                        }
                        if (_addr <= 0xFF)
                        {
                            _addrL = 0;
                            _addrH = _addrbyte[0];
                        }
                        sedbyte[0] = 0x00;
                        sedbyte[1] = 0x00;
                        sedbyte[2] = 0x00;
                        sedbyte[3] = 0x00;
                        sedbyte[4] = 0x00;
                        sedbyte[5] = 0x06;
                        sedbyte[6] = 0x01;
                        sedbyte[7] = 0x03;
                        sedbyte[8] = _addrL;//MODBUS地址低位
                        sedbyte[9] = _addrH;//modbus地址高位
                        sedbyte[10] = _lenL;//读取长度低位
                        sedbyte[11] = _lenH;//读取长度高位
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        int _recl = _recebyte[8] + 9;
                        if (_recelen == _recl)
                        {
                            for (int i = 0; i < _recebyte[8]; i++)
                            {
                                _ints[i] = (short)((_recebyte[9 + i * 2] << 8) | (_recebyte[10 + i * 2]));
                            }
                        }
                    }
                }
            }
            catch
            {


            }
            return _ints;
        }
        /// <summary>
        /// 读取无符号16位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public UInt16[] _uint16(int _addr, int _len)
        {
            UInt16[] _ints = new UInt16[1000];
            try
            {
                if (_online)
                {
                    if (_len >= 1)
                    {
                        int _LEN = _len * 1;
                        byte[] _recebyte = new byte[1000];
                        byte _lenH = 0;
                        byte _lenL = 1;
                        byte _addrH = 0;
                        byte _addrL = 1;
                        byte[] sedbyte = new byte[20];
                        byte[] _lengbyte = BitConverter.GetBytes(_LEN);
                        byte[] _addrbyte = BitConverter.GetBytes(_addr);
                        if (_LEN > 0xFF)
                        {
                            _lenL = _lengbyte[_lengbyte.Length - 3];
                            _lenH = _lengbyte[_lengbyte.Length - 4];
                        }
                        if (_LEN <= 0xFF)
                        {
                            _lenL = 0;
                            _lenH = _lengbyte[0];
                        }
                        if (_addr > 0xFF)
                        {
                            _addrL = _addrbyte[_addrbyte.Length - 3];
                            _addrH = _addrbyte[_addrbyte.Length - 4];
                        }
                        if (_addr <= 0xFF)
                        {
                            _addrL = 0;
                            _addrH = _addrbyte[0];
                        }
                        sedbyte[0] = 0x00;
                        sedbyte[1] = 0x00;
                        sedbyte[2] = 0x00;
                        sedbyte[3] = 0x00;
                        sedbyte[4] = 0x00;
                        sedbyte[5] = 0x06;
                        sedbyte[6] = 0x01;
                        sedbyte[7] = 0x03;
                        sedbyte[8] = _addrL;//MODBUS地址低位
                        sedbyte[9] = _addrH;//modbus地址高位
                        sedbyte[10] = _lenL;//读取长度低位
                        sedbyte[11] = _lenH;//读取长度高位
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        int _recl = _recebyte[8] + 9;
                        if (_recelen == _recl)
                        {
                            for (int i = 0; i < _recebyte[8] / 2; i++)
                            {
                                _ints[i] = Convert.ToUInt16(((_recebyte[9 + i * 2] << 8) | (_recebyte[10 + i * 2])));
                            }
                        }
                    }
                }
            }
            catch
            {


            }
            return _ints;
        }
        /// <summary>
        /// 写单个位
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_bool">值：TRUE  or FALSE</param>
        public bool _wbite(int _addr, bool _bool)
        {
            bool _successful = false;
            try
            {
                if (_online)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte _bools = 0;
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    if (_bool)
                    {
                        _bools = 1;
                    }
                    if (_addr > 0xFF)
                    {
                        _addrL = _addrbyte[_addrbyte.Length - 3];
                        _addrH = _addrbyte[_addrbyte.Length - 4];
                    }
                    if (_addr <= 0xFF)
                    {
                        _addrL = 0;
                        _addrH = _addrbyte[0];
                    }
                    byte[] sedbyte = new byte[30];
                    sedbyte[0] = 0x00;
                    sedbyte[1] = 0x00;
                    sedbyte[2] = 0x00;
                    sedbyte[3] = 0x00;
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x06;
                    sedbyte[6] = 0x01;
                    sedbyte[7] = 0x05;
                    sedbyte[8] = _addrL;
                    sedbyte[9] = _addrH;
                    sedbyte[10] = _bools;
                    _socket.Send(sedbyte, 0, 11, SocketFlags.None);
                    int _recelen = _socket.Receive(_recebyte, 0);
                    if (_recelen == 12)
                    {
                        _successful = true;
                    }
                }
            }
            catch
            {


            }
            return _successful;
        }
        /// <summary>
        /// 写入单个无符号16位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_data">无符号16位数据</param>
        /// <returns></returns>
        public bool _wuint(int _addr, UInt16 _data)
        {
            bool _successful = false;
            try
            {
                if (_online)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    if (_data < 65535)
                    {
                        if (_addr > 0xFF)
                        {
                            _addrL = _addrbyte[_addrbyte.Length - 3];
                            _addrH = _addrbyte[_addrbyte.Length - 4];
                        }
                        if (_addr <= 0xFF)
                        {
                            _addrL = 0;
                            _addrH = _addrbyte[0];
                        }
                        UInt16 _y = Convert.ToUInt16(_data);
                        byte[] intBuff = BitConverter.GetBytes(_y);
                        sedbyte[0] = 0x00;
                        sedbyte[1] = 0x00;
                        sedbyte[2] = 0x00;
                        sedbyte[3] = 0x00;
                        sedbyte[4] = 0x00;
                        sedbyte[5] = 0x06;
                        sedbyte[6] = 0x01;
                        sedbyte[7] = 0x06;
                        sedbyte[8] = _addrL;//MODBUS地址低位
                        sedbyte[9] = _addrH;//modbus地址高位
                        sedbyte[10] = intBuff[1];
                        sedbyte[11] = intBuff[0];
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        if (_recelen == 12)
                        {
                            _successful = true;
                        }
                    }
                }
            }
            catch
            {


            }
            return _successful;
        }
        /// <summary>
        /// 写入单个有符号16位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_data">有符号16位数据</param>
        /// <returns></returns>
        public bool _wint(int _addr, Int16 _data)
        {
            bool _successful = false;
            try
            {
                if (_online)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    if (_data < 32767 || _data < -32767)
                    {
                        if (_addr > 0xFF)
                        {
                            _addrL = _addrbyte[_addrbyte.Length - 3];
                            _addrH = _addrbyte[_addrbyte.Length - 4];
                        }
                        if (_addr <= 0xFF)
                        {
                            _addrL = 0;
                            _addrH = _addrbyte[0];
                        }
                        Int16 _y = Convert.ToInt16(_data);
                        byte[] intBuff = BitConverter.GetBytes(_y);
                        sedbyte[0] = 0x00;
                        sedbyte[1] = 0x00;
                        sedbyte[2] = 0x00;
                        sedbyte[3] = 0x00;
                        sedbyte[4] = 0x00;
                        sedbyte[5] = 0x06;
                        sedbyte[6] = 0x01;
                        sedbyte[7] = 0x06;
                        sedbyte[8] = _addrL;//MODBUS地址低位
                        sedbyte[9] = _addrH;//modbus地址高位
                        sedbyte[10] = intBuff[1];
                        sedbyte[11] = intBuff[0];
                        _socket.Send(sedbyte, 0, 12, SocketFlags.None);//发送报文
                        int _recelen = _socket.Receive(_recebyte, 0);
                        if (_recelen == 12)
                        {
                            _successful = true;
                        }
                    }
                }
            }
            catch
            {


            }
            return _successful;
        }
        /// <summary>
        /// 写入单个有符号32位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_data">数据</param>
        /// <returns></returns>
        public bool _wint32(int _addr, Int32 _data)
        {
            //形参_data 单精度  传入实参时，实参数字后需要加f表示单精度
            bool _successful = false;
            try
            {
                if (_online)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    byte[] intBuff = BitConverter.GetBytes(_data);
                    if (_addr > 0xFF)
                    {
                        _addrL = _addrbyte[_addrbyte.Length - 3];
                        _addrH = _addrbyte[_addrbyte.Length - 4];
                    }
                    if (_addr <= 0xFF)
                    {
                        _addrL = 0;
                        _addrH = _addrbyte[0];
                    }
                    sedbyte[0] = 0x00;
                    sedbyte[1] = 0x00;
                    sedbyte[2] = 0x00;
                    sedbyte[3] = 0x00;
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x0B;
                    sedbyte[6] = 0x01;
                    sedbyte[7] = 0x10;
                    sedbyte[8] = _addrL;//MODBUS地址低位
                    sedbyte[9] = _addrH;//modbus地址高位
                    sedbyte[10] = 0x00;
                    sedbyte[11] = 0x02;
                    sedbyte[12] = 0x04;
                    sedbyte[13] = intBuff[1];
                    sedbyte[14] = intBuff[0];
                    sedbyte[15] = intBuff[3];
                    sedbyte[16] = intBuff[2];
                    _socket.Send(sedbyte, 0, 17, SocketFlags.None);//发送报文
                    int _recelen = _socket.Receive(_recebyte, 0);
                    if (_recelen == 12)
                    {
                        _successful = true;
                    }
                }
            }
            catch
            {


            }
            return _successful;
        }
        /// <summary>
        /// 写入单个无符号32数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_data">无符号32位数据</param>
        /// <returns></returns>
        public bool _wuint32(int _addr, UInt32 _data)
        {
            //形参_data 单精度  传入实参时，实参数字后需要加f表示单精度
            bool _successful = false;
            try
            {
                if (_online)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    byte[] intBuff = BitConverter.GetBytes(_data);
                    if (_addr > 0xFF)
                    {
                        _addrL = _addrbyte[_addrbyte.Length - 3];
                        _addrH = _addrbyte[_addrbyte.Length - 4];
                    }
                    if (_addr <= 0xFF)
                    {
                        _addrL = 0;
                        _addrH = _addrbyte[0];
                    }
                    sedbyte[0] = 0x00;
                    sedbyte[1] = 0x00;
                    sedbyte[2] = 0x00;
                    sedbyte[3] = 0x00;
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x0B;
                    sedbyte[6] = 0x01;
                    sedbyte[7] = 0x10;
                    sedbyte[8] = _addrL;//MODBUS地址低位
                    sedbyte[9] = _addrH;//modbus地址高位
                    sedbyte[10] = 0x00;
                    sedbyte[11] = 0x02;
                    sedbyte[12] = 0x04;
                    sedbyte[13] = intBuff[1];
                    sedbyte[14] = intBuff[0];
                    sedbyte[15] = intBuff[3];
                    sedbyte[16] = intBuff[2];
                    _socket.Send(sedbyte, 0, 17, SocketFlags.None);//发送报文
                    int _recelen = _socket.Receive(_recebyte, 0);
                    if (_recelen == 12)
                    {
                        _successful = true;
                    }
                }
            }
            catch
            {


            }
            return _successful;
        }
        /// <summary>
        /// 单寄存器写入单精度浮点数
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_data">单精度浮点数数据</param>
        /// <returns></returns>
        public bool _wfloat(int _addr, Single _data)
        {
            //形参_data 单精度  传入实参时，实参数字后需要加f表示单精度
            bool _successful = false;
            try
            {
                if (_online)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    byte[] intBuff = BitConverter.GetBytes(_data);
                    if (_addr > 0xFF)
                    {
                        _addrL = _addrbyte[_addrbyte.Length - 3];
                        _addrH = _addrbyte[_addrbyte.Length - 4];
                    }
                    if (_addr <= 0xFF)
                    {
                        _addrL = 0;
                        _addrH = _addrbyte[0];
                    }
                    sedbyte[0] = 0x00;
                    sedbyte[1] = 0x00;
                    sedbyte[2] = 0x00;
                    sedbyte[3] = 0x00;
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x0B;
                    sedbyte[6] = 0x01;
                    sedbyte[7] = 0x10;
                    sedbyte[8] = _addrL;//MODBUS地址低位
                    sedbyte[9] = _addrH;//modbus地址高位
                    sedbyte[10] = 0x00;
                    sedbyte[11] = 0x02;
                    sedbyte[12] = 0x04;
                    sedbyte[13] = intBuff[1];
                    sedbyte[14] = intBuff[0];
                    sedbyte[15] = intBuff[3];
                    sedbyte[16] = intBuff[2];
                    _socket.Send(sedbyte, 0, 17, SocketFlags.None);//发送报文
                    int _recelen = _socket.Receive(_recebyte, 0);
                    if (_recelen == 12)
                    {
                        _successful = true;
                    }
                }
            }
            catch
            {


            }
            return _successful;
        }
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {


                    // TODO: 释放托管状态(托管对象)。
                }


                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。


                disposedValue = true;
            }
        }


        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        //~ModbusTcp() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        //    }


        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

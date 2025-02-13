using System.Collections;
using System.IO.Ports;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace METU.MODBUS.MODBUS
{
    public class ModbusRtu
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="com">端口</param>
        /// <param name="BaudRate">波特率</param>
        /// <param name="stopbits">停止位</param>
        /// <param name="parity">校验方式</param>
        /// <param name="databites">数据位</param>
        public ModbusRtu(string com, int BaudRate, StopBits stopbits, Parity parity, int databites)
        {
            try
            {
                _Portname = com.ToUpper();
                _BaudRate = BaudRate;
                _StopBites = stopbits;
                _Parity = parity;
                _Databites = databites;
                string[] _allcom = SerialPort.GetPortNames();
                for (int i = 0; i < _allcom.Length; i++)
                {
                    _allcom[i] = _allcom[i].ToUpper();
                }
              _switch1 = ((IList)_allcom).Contains(_Portname); //判断COM口是否存在。
                _SerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_serportRece);
                if (!_SerialPort.IsOpen)
                {
                    _SerialPort.Open();
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
        }
        private void _serportRece(object sender, SerialDataReceivedEventArgs e)
        {


            //MessageBox.Show("Test");
        }
        /// <summary>
        /// 端口名称
        /// </summary>
        public string _Portname="com1";
        /// <summary>
        /// 波特率
        /// </summary>
        public int _BaudRate=9600;
        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits _StopBites= StopBits.One;
        /// <summary>
        /// 校验位
        /// </summary>
        public Parity _Parity= Parity.None;
        /// <summary>
        /// 数据位
        /// </summary>
        public int _Databites=8;
        /// <summary>
        /// 开关1判断是否存在COM口
        /// </summary>
        public bool _switch1 = false;
        private static SerialPort _SerialPort1 = new SerialPort();
        private SerialPort _SerialPort
        {
            get
            {
                try
                {
                    if (!_SerialPort1.IsOpen)//判断端口是否打开
                    {
                        _SerialPort1.DataBits = _Databites;
                        _SerialPort1.BaudRate = _BaudRate;
                        _SerialPort1.PortName = _Portname;
                        _SerialPort1.StopBits = _StopBites;
                        _SerialPort1.Parity = _Parity;
                        _SerialPort1.Open();
                        return _SerialPort1;
                    }
                    string[] _allcom = SerialPort.GetPortNames();


                    for (int i = 0; i < _allcom.Length; i++)
                    {
                        _allcom[i] = _allcom[i].ToUpper();
                    }

                    _switch1 = ((IList)_allcom).Contains(_Portname.ToUpper()); //判断COM口是否存在。


                }
                catch (Exception ex)
                {
                    FileHelper.Writelog(ex.Message);
                }
                return _SerialPort1;
            }
        }
        // public byte[] _sendbyte = new byte[30];
        private byte[] _bytearr(int _data)
        {
            byte[] _s = new byte[2];
            try
            {
                byte[] s1 = BitConverter.GetBytes(_data);
                if (_data > 0xFF)
                {
                    _s[0] = s1[s1.Length - 3];
                    _s[1] = s1[s1.Length - 4];
                }
                if (_data <= 0xFF)
                {
                    _s[0] = 0;
                    _s[1] = s1[0];
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            //  s = BitConverter.GetBytes(_data);
            return _s;
        }
        /// <summary>
        /// CRC16校验
        /// </summary>
        /// <param name="instructions">校验的字节数组</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public byte[] _crcchecking(byte[] instructions, uint start, uint length)
        {
            uint i, j;
            uint crc16 = 0xFFFF;//crc寄存器赋初值
            try
            {
                length = length + start;
                for (i = start; i < length; i++)
                {
                    crc16 ^= instructions[i];
                    for (j = 0; j < 8; j++)
                    {
                        if ((crc16 & 0x01) == 1)
                        {
                            crc16 = (crc16 >> 1) ^ 0xA001;
                        }
                        else
                        {
                            crc16 = crc16 >> 1;
                        }
                    }
                }
                //   UInt16 X = (UInt16)(crc16 *256);
                // UInt16 Y = (UInt16)(crc16/256);
                //crc16 = (UInt16)(X ^ Y);
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            return BitConverter.GetBytes(crc16);
        }
        /// <summary>
        /// 读取无符号16位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public UInt16[] _ruint16(int _addr, byte _stand, int _len)
        {
            UInt16[] _s = new UInt16[_len];
            byte[] _send = new byte[20];
            try
            {
                byte[] _addrbyte = BitConverter.GetBytes(_addr);
                if (_switch1)
                {
                    _send[0] = _stand;
                    _send[1] = 0x03;
                    _send[2] = _bytearr(_addr)[0];
                    _send[3] = _bytearr(_addr)[1];
                    _send[4] = _bytearr(_len)[0];
                    _send[5] = _bytearr(_len)[1];
                    _send[6] = _crcchecking(_send, 0, 6)[0];
                    _send[7] = _crcchecking(_send, 0, 6)[1];
                    _SerialPort.Write(_send, 0, 8);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    byte[] _rec = new byte[_reclen];
                    if (_reclen > 5)
                    {
                        _SerialPort.Read(_rec, 0, _reclen);
                        byte reccrcL = _rec[_reclen - 1];//取接收到的报文  校验码低位
                        byte reccrcH = _rec[_reclen - 2];//取校验码高位
                        byte[] crc = _crcchecking(_rec, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[1] && reccrcH == crc[0])//比对校验码
                        {
                            byte fun = _rec[1];//功能码
                            if (fun == 0x03)//modbus03功能码
                            {
                                byte databyteNum = _rec[2];//接收字节数
                                for (int i = 0; i < databyteNum; i++)
                                {
                                    _s[i] = (UInt16)(_rec[i * 2 + 3] << 8 | _rec[i * 2 + 4]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            return _s;
        }
        /// <summary>
        /// 读取有符号16位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_len">读取长度</param>
        /// <returns></returns>
        public Int16[] _rint16(int _addr, byte _stand, int _len)
        {
            Int16[] _s = new Int16[_len];
            byte[] _send = new byte[20];
            try
            {
                byte[] _addrbyte = BitConverter.GetBytes(_addr);
                if (_switch1)
                {
                    _send[0] = _stand;
                    _send[1] = 0x03;
                    _send[2] = _bytearr(_addr)[0];
                    _send[3] = _bytearr(_addr)[1];
                    _send[4] = _bytearr(_len)[0];
                    _send[5] = _bytearr(_len)[1];
                    _send[6] = _crcchecking(_send, 0, 6)[0];
                    _send[7] = _crcchecking(_send, 0, 6)[1];
                    _SerialPort.Write(_send, 0, 8);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    byte[] _rec = new byte[_reclen];
                    if (_reclen > 5)
                    {
                        _SerialPort.Read(_rec, 0, _reclen);
                        byte reccrcL = _rec[_reclen - 1];//取接收到的报文  校验码低位
                        byte reccrcH = _rec[_reclen - 2];//取校验码高位
                        byte[] crc = _crcchecking(_rec, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[1] && reccrcH == crc[0])//比对校验码
                        {
                            byte fun = _rec[1];//功能码
                            if (fun == 0x03)//modbus03功能码
                            {
                                byte databyteNum = _rec[2];//接收字节数
                                for (int i = 0; i < databyteNum; i++)
                                {
                                    _s[i] = (Int16)(_rec[i * 2 + 3] << 8 | _rec[i * 2 + 4]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            return _s;
        }
        /// <summary>
        /// 读取无符号32位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_length">长度</param>
        /// <returns></returns>
        public UInt32[] _ruint32(int _addr, byte _stand, int _length)
        {
            int _len = _length * 2;
            UInt32[] _s = new UInt32[_length];
            byte[] _send = new byte[20];
            try
            {
                byte[] _addrbyte = BitConverter.GetBytes(_addr);
                if (_switch1)
                {
                    _send[0] = _stand;
                    _send[1] = 0x03;
                    _send[2] = _bytearr(_addr)[0];
                    _send[3] = _bytearr(_addr)[1];
                    _send[4] = _bytearr(_len)[0];
                    _send[5] = _bytearr(_len)[1];
                    _send[6] = _crcchecking(_send, 0, 6)[0];
                    _send[7] = _crcchecking(_send, 0, 6)[1];
                    _SerialPort.Write(_send, 0, 8);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    byte[] _rec = new byte[_reclen];
                    if (_reclen > 5)
                    {
                        _SerialPort.Read(_rec, 0, _reclen);
                        byte reccrcL = _rec[_reclen - 1];//取接收到的报文  校验码低位
                        byte reccrcH = _rec[_reclen - 2];//取校验码高位
                        byte[] crc = _crcchecking(_rec, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[1] && reccrcH == crc[0])//比对校验码
                        {
                            byte fun = _rec[1];//功能码
                            if (fun == 0x03)//modbus03功能码
                            {
                                byte databyteNum = _rec[2];//接收字节数
                                for (int j = 0; j < databyteNum / 4; j++)
                                {
                                    byte[] _nt = { _rec[4 + j * 4], _rec[3 + j * 4], _rec[6 + j * 4], _rec[5 + j * 4] };
                                    _s[j] = BitConverter.ToUInt32(_nt, 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            return _s;
        }
        /// <summary>
        /// 读取有符号32位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_length">读取长度</param>
        /// <returns></returns>
        public Int32[] _rint32(int _addr, byte _stand, int _length)
        {
            int _len = _length * 2;
            Int32[] _s = new Int32[_length];
            byte[] _send = new byte[20];
            try
            {
                byte[] _addrbyte = BitConverter.GetBytes(_addr);
                if (_switch1)
                {
                    _send[0] = _stand;
                    _send[1] = 0x03;
                    _send[2] = _bytearr(_addr)[0];
                    _send[3] = _bytearr(_addr)[1];
                    _send[4] = _bytearr(_len)[0];
                    _send[5] = _bytearr(_len)[1];
                    _send[6] = _crcchecking(_send, 0, 6)[0];
                    _send[7] = _crcchecking(_send, 0, 6)[1];
                    _SerialPort.Write(_send, 0, 8);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    byte[] _rec = new byte[_reclen];
                    if (_reclen > 5)
                    {
                        _SerialPort.Read(_rec, 0, _reclen);
                        byte reccrcL = _rec[_reclen - 1];//取接收到的报文  校验码低位
                        byte reccrcH = _rec[_reclen - 2];//取校验码高位
                        byte[] crc = _crcchecking(_rec, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[1] && reccrcH == crc[0])//比对校验码
                        {
                            byte fun = _rec[1];//功能码
                            if (fun == 0x03)//modbus03功能码
                            {
                                byte databyteNum = _rec[2];//接收字节数
                                for (int j = 0; j < databyteNum / 4; j++)
                                {
                                    byte[] _nt = { _rec[4 + j * 4], _rec[3 + j * 4], _rec[6 + j * 4], _rec[5 + j * 4] };
                                    _s[j] = BitConverter.ToInt32(_nt, 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            return _s;
        }
        /// <summary>
        /// 读取32位浮点数
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_length">读取长度</param>
        /// <returns></returns>
        public Single[] _rfloat(int _addr, byte _stand, int _length)
        {
            int _len = _length * 2;
            Single[] _s = new Single[_length];
            byte[] _send = new byte[20];
            try
            {
                byte[] _addrbyte = BitConverter.GetBytes(_addr);
                if (_switch1)
                {
                    _send[0] = _stand;
                    _send[1] = 0x03;
                    _send[2] = _bytearr(_addr)[0];
                    _send[3] = _bytearr(_addr)[1];
                    _send[4] = _bytearr(_len)[0];
                    _send[5] = _bytearr(_len)[1];
                    _send[6] = _crcchecking(_send, 0, 6)[0];
                    _send[7] = _crcchecking(_send, 0, 6)[1];
                    _SerialPort.Write(_send, 0, 8);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    byte[] _rec = new byte[_reclen];
                    if (_reclen > 5)
                    {
                        _SerialPort.Read(_rec, 0, _reclen);
                        byte reccrcL = _rec[_reclen - 1];//取接收到的报文  校验码低位
                        byte reccrcH = _rec[_reclen - 2];//取校验码高位
                        byte[] crc = _crcchecking(_rec, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[1] && reccrcH == crc[0])//比对校验码
                        {
                            byte fun = _rec[1];//功能码
                            if (fun == 0x03)//modbus03功能码
                            {
                                byte databyteNum = _rec[2];//接收字节数
                                for (int j = 0; j < databyteNum / 4; j++)
                                {
                                    byte[] _nt = { _rec[4 + j * 4], _rec[3 + j * 4], _rec[6 + j * 4], _rec[5 + j * 4] };
                                    _s[j] = BitConverter.ToSingle(_nt, 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            return _s;
        }
        /// <summary>
        /// 读取位状态
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_length">读取长度</param>
        /// <returns></returns>
        public bool[] _rbite(int _addr, byte _stand, int _length)
        {
            int _len = _length * 1;
            bool[] _s = new bool[1000];
            byte[] _send = new byte[20];
            try
            {
                byte[] _addrbyte = BitConverter.GetBytes(_addr);
                if (_switch1)
                {
                    _send[0] = _stand;
                    _send[1] = 0x01;
                    _send[2] = _bytearr(_addr)[0];
                    _send[3] = _bytearr(_addr)[1];
                    _send[4] = _bytearr(_len)[0];
                    _send[5] = _bytearr(_len)[1];
                    _send[6] = _crcchecking(_send, 0, 6)[0];
                    _send[7] = _crcchecking(_send, 0, 6)[1];
                    _SerialPort.Write(_send, 0, 8);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    byte[] _rec = new byte[_reclen];
                    if (_reclen > 5)
                    {
                        _SerialPort.Read(_rec, 0, _reclen);
                        byte reccrcL = _rec[_reclen - 1];//取接收到的报文  校验码低位
                        byte reccrcH = _rec[_reclen - 2];//取校验码高位
                        byte[] crc = _crcchecking(_rec, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[1] && reccrcH == crc[0])//比对校验码
                        {
                            byte databyteNum = _rec[2];//接收字节数
                            byte fun = _rec[1];//功能码
                            if (fun == 0x01)//modbus03功能码
                            {

                                for (int j = 0; j < databyteNum; j++)
                                {
                                    for (int i = 0; i < 8; i++)
                                    {
                                        _s[j * 8 + i] = (_rec[3 + j] & Convert.ToInt16(Math.Pow(2, i))) == Convert.ToInt16(Math.Pow(2, i));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }
            return _s;
        }
        /// <summary>
        /// 写单个位状态
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_bool">BOOL值</param>
        /// <returns></returns>
        public bool _wbite(int _addr, byte _stand, bool _bool)
        {


            bool s = false;
            byte bools = 0;
            byte[] _send = new byte[20];
            if (_switch1)
            {
                byte[] _addrbyte = BitConverter.GetBytes(_addr);
                byte _addrH = 0;
                byte _addrL = 1;
                byte[] sedbyte = new byte[20];
                try
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
                    if (_bool)
                    {
                        bools = 1;
                    }
                    _send[0] = _stand;
                    _send[1] = 0x05;
                    _send[2] = _addrL;
                    _send[3] = _addrH;
                    _send[4] = bools;
                    _send[5] = _crcchecking(_send, 0, 5)[0];
                    _send[6] = _crcchecking(_send, 0, 5)[1];
                    _SerialPort.Write(_send, 0, 7);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    byte[] _rec = new byte[_reclen];
                    if (_reclen > 5)
                    {
                        _SerialPort.Read(_rec, 0, _reclen);
                        byte reccrcL = _rec[_reclen - 3];//取接收到的报文  校验码低位
                        byte reccrcH = _rec[_reclen - 2];//取校验码高位
                        byte[] crc = _crcchecking(_rec, 0, (uint)_reclen - 3);//重新校验报文
                        if (reccrcL == crc[0] && reccrcH == crc[1])//比对校验码
                        {
                            byte databyteNum = _rec[2];//接收字节数
                            byte fun = _rec[1];//功能码
                            if (fun == 0x05)//modbus03功能码
                            {
                                s = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    FileHelper.Writelog(ex.Message);
                    //throw;
                }
            }
            return s;
        }
        /// <summary>
        /// 写单个16位有符号数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_data">16位有符号数据</param>
        /// <returns></returns>
        public bool _wint16(int _addr, byte _stand, Int16 _data)
        {
            bool _successful = false;
            try
            {
                if (_switch1)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
                    if (_data < 32767 || _data > -32767)
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
                        sedbyte[0] = _stand;
                        sedbyte[1] = 0x06;
                        sedbyte[2] = _addrL;//MODBUS地址低位
                        sedbyte[3] = _addrH;//modbus地址高位
                        sedbyte[4] = intBuff[1];
                        sedbyte[5] = intBuff[0];
                        sedbyte[6] = _crcchecking(sedbyte, 0, 6)[0];
                        sedbyte[7] = _crcchecking(sedbyte, 0, 6)[1];
                        _SerialPort.Write(sedbyte, 0, 8);//发送报文
                        int _reclen = _SerialPort.BytesToRead;
                        _SerialPort.Read(_recebyte, 0, _reclen);
                        if (_reclen > 5)
                        {
                            byte reccrcL = _recebyte[_reclen - 2];//取接收到的报文  校验码低位
                            byte reccrcH = _recebyte[_reclen - 1];//取校验码高位
                            byte[] crc = _crcchecking(_recebyte, 0, (uint)_reclen - 2);//重新校验报文
                            if (reccrcL == crc[0] && reccrcH == crc[1])//比对校验码
                            {
                                byte fun = _recebyte[1];//功能码
                                if (fun == 0x06)//modbus03功能码
                                {
                                    _successful = true;
                                }
                            }
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
        /// 写单个无符号16位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_data">16位无符号数据</param>
        /// <returns></returns>
        public bool _wuint16(int _addr, byte _stand, UInt16 _data)
        {
            bool _successful = false;
            try
            {
                if (_switch1)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
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
                    sedbyte[0] = _stand;
                    sedbyte[1] = 0x06;
                    sedbyte[2] = _addrL;//MODBUS地址低位
                    sedbyte[3] = _addrH;//modbus地址高位
                    sedbyte[4] = intBuff[1];
                    sedbyte[5] = intBuff[0];
                    sedbyte[6] = _crcchecking(sedbyte, 0, 6)[0];
                    sedbyte[7] = _crcchecking(sedbyte, 0, 6)[1];
                    _SerialPort.Write(sedbyte, 0, 8);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    _SerialPort.Read(_recebyte, 0, _reclen);
                    if (_reclen > 5)
                    {
                        byte reccrcL = _recebyte[_reclen - 2];//取接收到的报文  校验码低位
                        byte reccrcH = _recebyte[_reclen - 1];//取校验码高位
                        byte[] crc = _crcchecking(_recebyte, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[0] && reccrcH == crc[1])//比对校验码
                        {
                            byte fun = _recebyte[1];//功能码
                            if (fun == 0x06)//modbus03功能码
                            {
                                _successful = true;
                            }
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
        /// <param name="_stand">站号</param>
        /// <param name="_data">32位有符号数据</param>
        /// <returns></returns>
        public bool _wint32(int _addr, byte _stand, Int32 _data)
        {
            bool _successful = false;
            try
            {
                if (_switch1)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
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
                    byte[] intBuff = BitConverter.GetBytes(_data);
                    sedbyte[0] = _stand;
                    sedbyte[1] = 0x10;
                    sedbyte[2] = _addrL;//MODBUS地址低位
                    sedbyte[3] = _addrH;//modbus地址高位
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x02;
                    sedbyte[6] = 0x04;
                    sedbyte[7] = intBuff[1];
                    sedbyte[8] = intBuff[0];
                    sedbyte[9] = intBuff[3];
                    sedbyte[10] = intBuff[2];
                    sedbyte[11] = _crcchecking(sedbyte, 0, 11)[0];
                    sedbyte[12] = _crcchecking(sedbyte, 0, 11)[1];
                    _SerialPort.Write(sedbyte, 0, 13);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    _SerialPort.Read(_recebyte, 0, _reclen);
                    if (_reclen > 5)
                    {
                        byte reccrcL = _recebyte[_reclen - 2];//取接收到的报文  校验码低位
                        byte reccrcH = _recebyte[_reclen - 1];//取校验码高位
                        byte[] crc = _crcchecking(_recebyte, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[0] && reccrcH == crc[1])//比对校验码
                        {
                            byte fun = _recebyte[1];//功能码
                            if (fun == 0x10)//modbus03功能码
                            {
                                _successful = true;
                            }
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
        /// 写入单个无符号32位数据
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_data">无符号32位数据</param>
        /// <returns></returns>
        public bool _wuint32(int _addr, byte _stand, UInt32 _data)
        {
            bool _successful = false;
            try
            {
                if (_switch1)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
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
                    byte[] intBuff = BitConverter.GetBytes(_data);
                    sedbyte[0] = _stand;
                    sedbyte[1] = 0x10;
                    sedbyte[2] = _addrL;//MODBUS地址低位
                    sedbyte[3] = _addrH;//modbus地址高位
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x02;
                    sedbyte[6] = 0x04;
                    sedbyte[7] = intBuff[1];
                    sedbyte[8] = intBuff[0];
                    sedbyte[9] = intBuff[3];
                    sedbyte[10] = intBuff[2];
                    sedbyte[11] = _crcchecking(sedbyte, 0, 11)[0];
                    sedbyte[12] = _crcchecking(sedbyte, 0, 11)[1];
                    _SerialPort.Write(sedbyte, 0, 13);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    _SerialPort.Read(_recebyte, 0, _reclen);
                    if (_reclen > 5)
                    {
                        byte reccrcL = _recebyte[_reclen - 2];//取接收到的报文  校验码低位
                        byte reccrcH = _recebyte[_reclen - 1];//取校验码高位
                        byte[] crc = _crcchecking(_recebyte, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[0] && reccrcH == crc[1])//比对校验码
                        {
                            byte fun = _recebyte[1];//功能码
                            if (fun == 0x10)//modbus03功能码
                            {
                                _successful = true;
                            }
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
        /// 写入单个单精度浮点数
        /// </summary>
        /// <param name="_addr">MODBUS地址</param>
        /// <param name="_stand">站号</param>
        /// <param name="_data">单精度浮点数</param>
        /// <returns></returns>
        public bool _wfloat(int _addr, byte _stand, Single _data)
        {
            bool _successful = false;
            try
            {
                if (_switch1)
                {
                    byte[] _recebyte = new byte[1000];
                    byte _addrH = 0;
                    byte _addrL = 1;
                    byte[] sedbyte = new byte[20];
                    byte[] _addrbyte = BitConverter.GetBytes(_addr);
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
                    byte[] intBuff = BitConverter.GetBytes(_data);
                    sedbyte[0] = _stand;
                    sedbyte[1] = 0x10;
                    sedbyte[2] = _addrL;//MODBUS地址低位
                    sedbyte[3] = _addrH;//modbus地址高位
                    sedbyte[4] = 0x00;
                    sedbyte[5] = 0x02;
                    sedbyte[6] = 0x04;
                    sedbyte[7] = intBuff[1];
                    sedbyte[8] = intBuff[0];
                    sedbyte[9] = intBuff[3];
                    sedbyte[10] = intBuff[2];
                    sedbyte[11] = _crcchecking(sedbyte, 0, 11)[0];
                    sedbyte[12] = _crcchecking(sedbyte, 0, 11)[1];
                    _SerialPort.Write(sedbyte, 0, 13);//发送报文
                    int _reclen = _SerialPort.BytesToRead;
                    _SerialPort.Read(_recebyte, 0, _reclen);
                    if (_reclen > 5)
                    {
                        byte reccrcL = _recebyte[_reclen - 2];//取接收到的报文  校验码低位
                        byte reccrcH = _recebyte[_reclen - 1];//取校验码高位
                        byte[] crc = _crcchecking(_recebyte, 0, (uint)_reclen - 2);//重新校验报文
                        if (reccrcL == crc[0] && reccrcH == crc[1])//比对校验码
                        {
                            byte fun = _recebyte[1];//功能码
                            if (fun == 0x10)//modbus03功能码
                            {
                                _successful = true;
                            }
                        }


                    }
                }
            }


            catch (Exception ex)
            {

                FileHelper.Writelog(ex.Message);
            }
            return _successful;
        }
    }
   
}

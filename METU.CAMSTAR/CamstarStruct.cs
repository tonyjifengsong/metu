using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{

    /// <summary>
    /// 用于容纳SQL查询结果的表数据结构
    /// </summary>
    public struct TableData
    {
        public List<string> columns;
        public List<List<string>> datas;
        public TableData(List<string> colsIn, List<List<string>> datasIn)
        {
            columns = new List<string>();
            datas = new List<List<string>>();
            for (int i = 0; i < colsIn.Count; i++)
            {
                columns.Add(colsIn[i]);
            }
            for (int i = 0; i < datasIn.Count; i++)
            {
                List<string> rowInTemp = datasIn[i];
                List<string> rowTemp = new List<string>();
                for (int j = 0; j < rowInTemp.Count; j++)
                {
                    rowTemp.Add(rowInTemp[j]);
                }
                datas.Add(rowTemp);
            }
        }

        public override string ToString()
        {
            string strRet = "";
            char gap = Convert.ToChar(0x09);
            for (int i = 0; i < columns.Count; i++)
            {
                strRet += columns[i] + gap;
            }
            strRet += Environment.NewLine;
            for (int i = 0; i < datas.Count; i++)
            {
                List<string> rowTemp = datas[i];
                for (int j = 0; j < rowTemp.Count; j++)
                {
                    strRet += rowTemp[j] + gap;
                }
                strRet += Environment.NewLine;
            }
            return strRet;
        }


        /// <summary>
        /// 根据列名获取列数据集
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns></returns>
        public List<string> GetByColName(string colName)
        {
            if (IsNull())
            {
                return new List<string>();
            }
            List<string> datasRet = new List<string>();
            int colNum = -1;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].ToUpper() == colName.ToUpper())
                {
                    colNum = i;
                    break;
                }
            }
            for (int i = 0; i < datas.Count; i++)
            {
                try
                {
                    List<string> datasTemp = datas[i];
                    datasRet.Add(datasTemp[colNum]);
                }
                catch { datasRet.Add(""); }
            }

            return datasRet;
        }

        //获取单元格数据
        public string GetUnit(int row, int col)
        {
            if (IsNull())
            {
                return "";
            }
            if (datas.Count < row + 1 || datas[row].Count < col + 1)
            {
                return "";
            }
            return datas[row][col];
        }

        //根据列名获取单元格数据
        public string GetUnit(int row, string colName)
        {
            if (IsNull())
            {
                return "";
            }
            int colNum = -1;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].ToUpper() == colName.ToUpper())
                {
                    colNum = i;
                    break;
                }
            }
            if (colNum >= 0)
            {
                return datas[row][colNum];
            }
            return null;
        }

        //判断数据是否为空
        public bool IsNull()
        {
            if (datas == null || datas.Count < 1)
            {
                return true;
            }
            return false;
        }

        //获取行数
        public int GetRowCount()
        {
            if (datas == null)
            {
                return 0;
            }
            return datas.Count;
        }
    }



    /// <summary>
    /// 连接信息结构体，包括服务器地址、端口、用户名、密码
    /// </summary>
    public struct csConnection
    {
        public string host;
        public int port;
        public string user;
        public string password;
        public csConnection(string hostIn, int portIn, string userIn, string pwIn)
        {
            this.host = hostIn;
            this.port = portIn;
            this.user = userIn;
            this.password = pwIn;
        }

        //获取服务器连接对象
        public ServerConnection GetServerConnection()
        {
            return new ServerConnection(this.host, this.port);
        }

        public override string ToString()
        {
            string strRet = "服务器=" + host + ";"
                + "端口=" + port.ToString() + ";"
                + "用户=" + user + ";"
                + "密码=" + password + ";";
            return strRet;
        }

        public bool Valid(ref XMLResponse res)
        {
            if (string.IsNullOrEmpty(host))
            {
                res.SetErrorInfo("连接服务器设置为空!");
                return false;
            }
            if (port <= 0)
            {
                res.SetErrorInfo("连接服务器端口需大于0!");
                return false;
            }
            if (string.IsNullOrEmpty(user))
            {
                res.SetErrorInfo("连接用户名为空!");
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                res.SetErrorInfo("连接用户密码为空!");
                return false;
            }
            return true;
        }
    }
}

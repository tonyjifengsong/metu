using METU.CONFIGS;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{
    public class CamstarBase
    {
        protected string strIP = AppSettingsHelper.GetIP, strUsername = AppSettingsHelper.GetUserName, strPassword =AppSettingsHelper.GetPWD;
        protected csConnection conn;
        protected int iPort = Convert.ToInt32( AppSettingsHelper.GetPort);
        protected XMLResponse res = new XMLResponse("", "", exceptType.normal, true, "", "");
        protected TableData tt;

        protected OracleConnection OracleConn;  //Oracle连接对象
        protected OracleCommand oraCommand;     //Oracle触发器
        protected OracleDataAdapter oraAdapter; //Oracle适配器
        protected OracleTransaction trans;      //Oracle事务
        public csConnection connObj;

        public CamstarBase()
        {
            conn = new csConnection(strIP, iPort, strUsername, strPassword);
            connObj = conn;
        }

        #region XML方式访问数据库
        /// <summary>
        /// 通过发送XML获取报表数据(分页)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="startIndex">开始</param>
        /// <param name="endIndex">结束</param>
        /// <param name="totalCount">总数</param>
        /// <returns>EXCEL:全部数据,DATAINFO:分页数据</returns>
        protected DataSet GetXMLForDataSet(string strSQL, int startIndex, int endIndex, out int totalCount)
        {
            DataSet ds = new DataSet();
            DataTable dtParams = new DataTable();
            DataTable dt = new DataTable();
            strSQL = GetQueryXML(strSQL, strUsername, strPassword);
            ServerConnection connect = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strSQL, connect);
            TableData tt = GetTableDataFromResponse(res.lastResponse);
            totalCount = tt.datas.Count;
            foreach (var td in tt.columns)
            {
                dtParams.Columns.Add(td);
                dt.Columns.Add(td);
            }
            for (int i = 0; i < tt.datas.Count; i++)
            {
                DataRow drParams = dtParams.NewRow();
                DataRow dr = dt.NewRow();
                for (int j = 0; j < tt.columns.Count; j++)
                {
                    if (i >= startIndex && i < endIndex)
                    {
                        if (tt.datas[i][j].Contains("+08:00") && tt.datas[i][j].Contains("T"))
                        {
                            dr[j] = DateTime.Parse(tt.datas[i][j]).ToString().Replace("0:00:00", "").Trim();
                            drParams[j] = DateTime.Parse(tt.datas[i][j]).ToString().Replace("0:00:00", "").Trim();
                        }
                        else
                        {
                            dr[j] = tt.datas[i][j].ToString().Trim();
                            drParams[j] = tt.datas[i][j].ToString().Trim();
                        }
                    }
                    else
                    {
                        if (tt.datas[i][j].Contains("+08:00") && tt.datas[i][j].Contains("T"))
                        {
                            drParams[j] = DateTime.Parse(tt.datas[i][j]).ToString().Replace("0:00:00", "").Trim();
                        }
                        else
                        {
                            drParams[j] = tt.datas[i][j].ToString().Trim();
                        }
                    }
                }
                dtParams.Rows.Add(drParams);
                if (i >= startIndex && i < endIndex)
                {
                    dt.Rows.Add(dr);
                }
            }
            dtParams.AcceptChanges();
            dt.AcceptChanges();
            ds.Tables.Add(dtParams);
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "EXCEL";
            ds.Tables[1].TableName = "DATAINFO";
            return ds;
        }
        /// <summary>
        /// 通过发送XML获取报表数据(不分页)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>EXCEL:全部数据</returns>
        protected DataSet GetXMLForDataSet(string strSQL)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            strSQL = GetQueryXML(strSQL, strUsername, strPassword);
            ServerConnection connect = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strSQL, connect);
            TableData tt = GetTableDataFromResponse(res.lastResponse);
            foreach (var td in tt.columns)
            {
                dt.Columns.Add(td);
            }
            for (int i = 0; i < tt.datas.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < tt.columns.Count; j++)
                {
                    if (tt.datas[i][j].Contains("+08:00") && tt.datas[i][j].Contains("T"))
                    {
                        dr[j] = DateTime.Parse(tt.datas[i][j]).ToString().Replace("0:00:00", "").Trim();
                    }
                    else
                    {
                        dr[j] = tt.datas[i][j].ToString().Trim();
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "EXCEL";
            return ds;
        }
        /// <summary>
        /// 通过发送XML获取DataTable
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>返回DataTable值</returns>
        protected DataTable GetXMLForDataTable(string strSQL)
        {
            DataTable dt = new DataTable();
            strSQL = GetQueryXML(strSQL, strUsername, strPassword);
            ServerConnection connect = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strSQL, connect);
            TableData tt = GetTableDataFromResponse(res.lastResponse);
            foreach (var td in tt.columns)
            {
                dt.Columns.Add(td);
            }
            for (int i = 0; i < tt.datas.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < tt.columns.Count; j++)
                {
                    if (tt.datas[i][j].Contains("+08:00") && tt.datas[i][j].Contains("T"))
                    {
                        dr[j] = DateTime.Parse(tt.datas[i][j]).ToString().Replace("0:00:00", "").Trim();
                    }
                    else
                    {
                        dr[j] = tt.datas[i][j].ToString().Trim();
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// 获取数据返回TableData格式
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        protected TableData GetQueryDataForTableData(string strSQL)
        {
            TableData tRtn = new TableData();
            strSQL = GetQueryXML(strSQL, strUsername, strPassword);
            ServerConnection connect = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strSQL, connect);
            tRtn = GetTableDataFromResponse(res.lastResponse);
            return tRtn;
        }

        /// <summary>
        /// 获取数据返回Dictionary
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        protected Dictionary<string, List<string>> GetQueryDataFordDictionary(string strSQL)
        {
            TableData tbd = new TableData();
            strSQL = GetQueryXML(strSQL, strUsername, strPassword);
            ServerConnection connect = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strSQL, connect);
            tbd = GetTableDataFromResponse(res.lastResponse);
            Dictionary<string, List<string>> dc = new Dictionary<string, List<string>>();
            for (int i = 0; i < tbd.columns.Count; i++)
            {
                dc.Add(tbd.columns[i], tbd.GetByColName(tbd.columns[i]));
            }
            return dc;
        }

        /// <summary>
        /// 获取数据返回DataTable格式
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        protected DataSet GetQueryDataForDataTable(string strSQL)
        {
            DataSet dsNew = new DataSet();
            DataTable dt = new DataTable();
            strSQL = GetQueryXML(strSQL, strUsername, strPassword);
            ServerConnection connect = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strSQL, connect);
            TableData tt = GetTableDataFromResponse(res.lastResponse);
            foreach (var td in tt.columns)
            {
                dt.Columns.Add(td);
            }
            for (int i = 0; i < tt.datas.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < tt.columns.Count; j++)
                {
                    if (tt.datas[i][j].Contains("+08:00") && tt.datas[i][j].Contains("T"))
                    {
                        dr[j] = DateTime.Parse(tt.datas[i][j]).ToString().Trim();
                    }
                    else
                    {
                        dr[j] = tt.datas[i][j].ToString().Trim();
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            dsNew.Tables.Add(dt);
            return dsNew;
        }
        /// <summary>
        /// 获取数据返回LIST格式
        /// </summary>
        /// <typeparam name="T">对象模型</typeparam>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        protected List<T> GetQueryDataForList<T>(string strSQL)
        {
            List<T> list = new List<T>();
            TableData tRtn = new TableData();

            strSQL = GetQueryXML(strSQL, strUsername, strPassword);
            ServerConnection connect = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strSQL, connect);
            tRtn = GetTableDataFromResponse(res.lastResponse);
            for (int i = 0; i < tRtn.datas.Count; i++)
            {
                T obj = Activator.CreateInstance<T>();
                Type type = obj.GetType();
                PropertyInfo[] pis = type.GetProperties();
                for (int j = 0; j < pis.Length; j++)
                {
                    if (!string.IsNullOrEmpty(tRtn.datas[i][j]))
                    {
                        pis[j].SetValue(obj, tRtn.datas[i][j].ToString());
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// 把TableData转换成DataTable
        /// </summary>
        /// <param name="tt"></param>
        /// <returns></returns>
        protected DataTable TableToDT(TableData tt)
        {
            DataTable dt = new DataTable();
            foreach (var td in tt.columns)
            {
                dt.Columns.Add(td);
            }
            for (int i = 0; i < tt.datas.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < tt.columns.Count; j++)
                {
                    if (tt.datas[i][j].Contains("+08:00") && tt.datas[i][j].Contains("T"))
                    {
                        dr[j] = DateTime.Parse(tt.datas[i][j]).ToString().Trim();
                    }
                    else
                    {
                        dr[j] = tt.datas[i][j].ToString().Trim();
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            return dt;
        }
        #endregion

        //根据SQL查询返回的XML，解析表结构
        public TableData GetTableDataFromResponse(string xml)
        {
            string strSetHead = XMLOps.ReadAtNode(xml, "recordSetHeader", false);
            string strSet = XMLOps.ReadAtNode(xml, "recordSet", false);
            List<string> cols = new List<string>();
            List<List<string>> datas = new List<List<string>>();

            while (true)
            {
                NodePos pos = XMLOps.FindNode(strSetHead, "column");
                if (!pos.Valid())
                {
                    break;
                }
                string nameTemp = XMLOps.ReadAtNode(strSetHead, "name", false);

                strSetHead = StrOps.GetPartString(strSetHead, pos.endRight + 1);
                cols.Add(nameTemp);
            }

            while (true)
            {
                if (strSet == null)
                {
                    break;
                }
                NodePos pos = XMLOps.FindNode(strSet, "row");
                if (!pos.Valid())
                {
                    break;
                }
                List<string> row = new List<string>();
                for (int i = 0; i < cols.Count; i++)
                {
                    string strRowTemp = XMLOps.ReadAtNode(strSet, cols[i]);
                    row.Add(strRowTemp);
                }
                strSet = StrOps.GetPartString(strSet, pos.endRight + 1);
                datas.Add(row);
            }
            return new TableData(cols, datas);
        }

        public const string template =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?><__InSite __version=\"1.1\"><__session><__connect><user><__name></__name></user>"
            + "<password __encrypted=\"no\"></password></__connect></__session><__service __serviceType=\"MoveStd\"><__utcOffset><![CDATA[08:00:00]]>"
            + "</__utcOffset><__inputData></__inputData><__requestData><CompletionMsg/></__requestData></__service><__query><__queryText></__queryText></__query></__InSite>";

        //根据查询语句，获取查询XML
        public string GetQueryXML(string text, string user, string password)
        {
            string strRet = XMLOps.InsertValue(template, "queryText", text);
            strRet = XMLOps.InsertValue(strRet, "name", user);
            strRet = XMLOps.InsertValue(strRet, "password", password);
            return strRet;
        }
        #region added by tony  2017-10-17  Memo: 执行XML
        /// <summary>
        /// 执行CAMSTARXML，返回结果
        /// </summary>
        /// <param name="request">CamstarXML参数</param>
        /// <param name="connect">Camstar链接字符串对象</param>
        /// <returns>返回执行结果字符串</returns>
        public string ExecuteCamstarXML(string request, ServerConnection connect)
        {

            string retXML = "";

            //获取异常信息
            try
            {
                retXML = connect.Submit(request);
            }
            catch
            {
                exceptType exception = exceptType.cannotConnect;
                return exception.ToString();
            }
            return retXML;


        }
        /// <summary>
        /// 执行CAMSTARXML，返回结果
        /// </summary>
        /// <param name="request">CamstarXML参数</param>
        /// <returns>返回执行结果</returns>
        public string ExecuteCamstarXML(string request)
        {
            ServerConnection connect = new ServerConnection(strIP, iPort);



            string retXML = "";

            //获取异常信息
            try
            {
                retXML = connect.Submit(request);
            }
            catch
            {
                exceptType exception = exceptType.cannotConnect;
                return exception.ToString();
            }
            return retXML;

        }

        public Dictionary<string, string> DicExecuteCamstarXML(string request)
        {
            ServerConnection connect = new ServerConnection(strIP, iPort);
            Dictionary<string, string> result = new Dictionary<string, string>();


            string retXML = "";

            //获取异常信息
            try
            {
                retXML = connect.Submit(request);
            }
            catch
            {
                exceptType exception = exceptType.cannotConnect;
                result.Add("EXCEPTION", exception.ToString());
                return result;
            }

            var resultstr = retXML.XMLSearchInnerNode("errorDescription");
            result.Add("errorDescription".ToUpper(), resultstr);
            var CompletionMsgstr = retXML.XMLSearchInnerNode("CompletionMsg");
            result.Add("CompletionMsg".ToUpper(), CompletionMsgstr);
            result.Add("returns".ToUpper(), retXML);
            return result;

        }
        #endregion
    }
}

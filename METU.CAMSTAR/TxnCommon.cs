using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{
    /// <summary>
    /// XML请求的通用类
    /// </summary>
    public static class TxnCommon
    {
        /// <summary>
        /// 事务XML模板的通用部分
        /// </summary>
        public const string xmlTempCommon = "<?xml version=\"1.0\" encoding=\"utf-16\"?><__InSite __version=\"1.1\" __encryption=\"2\">" +
            "<__session><__connect><user><__name></__name></user><password __encrypted=\"no\">" + "</password ></__connect></__session>";

        /// <summary>
        /// xml心跳信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string GetHeartBeat(string username, string password)
        {
            string strRet = "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
                + "<__InSite __version=\"1.1\"><__session><__connect><user><__name>"
                + username + "</__name></user><password __encrypted=\"no\">" + password
                + "</password></__connect></__session></__InSite>";
            return strRet;
        }

        /// <summary>
        /// 设置账号
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string SetAccount(string xml, string account)
        {
            List<string> nodes = new List<string>();
            nodes.Add("user");
            nodes.Add("name");
            string strRet = XMLOps.InsertValue(xml, nodes, account);
            return strRet;
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string SetPassword(string xml, string password)
        {
            string strRet = XMLOps.InsertValue(xml, "password", password);
            return strRet;
        }

        /// <summary>
        /// 插入执行节点
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string AddExecute(string xml)
        {
            string strExecute = XMLOps.GetCompleteNode("execute", true);
            string strRet = XMLOps.InsertValueBeforeANode(xml, "requestData", strExecute);
            return strRet;
        }

        /// <summary>
        /// 设置容器
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public static string SetContainer(string xml, string containerName)
        {
            string value = XMLOps.GetNodeString("Container", "", false);
            string strAdd = XMLOps.GetNodeString("name", containerName, true);
            value = XMLOps.InsertValue(value, "Container", strAdd);
            string strRet = XMLOps.InsertValue(xml, "inputData", value);
            return strRet;
        }

        /// <summary>
        /// 设置资源
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string SetResource(string xml, string resourceName)
        {
            string value = XMLOps.GetNodeString("Resource", "", false);
            string strAdd = XMLOps.GetNodeString("name", resourceName, true);
            value = XMLOps.InsertValue(value, "Resource", strAdd);
            string strRet = XMLOps.InsertValue(xml, "inputData", value);
            return strRet;
        }

        /// <summary>
        /// 添加Perform及事件节点
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static string AddPerformAndEvent(string xml, string eventName)
        {
            string value = XMLOps.GetNodeString("perform", "", true);
            string temp = XMLOps.GetNodeString("eventName", eventName, true);
            value = XMLOps.InsertValue(value, "perform", temp);
            xml = XMLOps.InsertValueAfterANode(xml, "inputData", value);
            return xml;
        }

        /// <summary>
        /// 添加事件节点 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static string AddEvent(string xml, string eventName)
        {
            string value = XMLOps.GetNodeString("eventName", eventName, true);
            xml = XMLOps.InsertValue(xml, "perform", value);
            return xml;
        }

        /// <summary>
        /// 添加请求数据节点
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string AddRequestData(string xml, string request)
        {
            string value = XMLOps.GetCompleteNode(request, false);
            xml = XMLOps.InsertValue(xml, "requestData", value);
            return xml;
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="XML"></param>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        public static string AddService(string XML, string ServiceName)
        {
            string ser = "<__service __serviceType=\"" + ServiceName + "\"><__utcOffset><![CDATA[08:00:00]]></__utcOffset>" +
            "<__inputData></__inputData><__requestData><CompletionMsg/></__requestData></__service> ";
            XML = XMLOps.InsertValueAfterANode(XML, "session", ser);
            return XML;
        }
        //由请求XML，得到XML请求的返回结构体
        public static XMLResponse GetResponse(string request, ServerConnection connect)
        {
            XMLResponse response = new XMLResponse("", "", exceptType.normal, true, "", "");

            response.lastRequest = request;

            string retXML = "";

            //获取异常信息
            try
            {
                retXML = connect.Submit(request);
            }
            catch
            {
                response.exception = exceptType.cannotConnect;
                return response;
            }
            response.lastResponse = retXML;

            //获取XML执行结果
            if (XMLOps.FindNode(retXML, "errorDescription").Valid())
            {
                response.result = false;
            }
            else
            {
                response.result = true;
            }

            //获取执行结果详细信息
            if (!response.result)
            {
                response.errorDiscription = XMLOps.ReadAtNode(response.lastResponse, "errorDescription");
            }
            else
            {
                response.successMessage = XMLOps.ReadAtNode(response.lastResponse, "CompletionMsg");
            }
            return response;
        }


        public static XMLResponse res;



        //获取日志信息
        public static string GetErrorInfo()
        {
            return res.GetErrorInfo();
        }
        public static string GetSuccessInfo()
        {
            return res.successMessage;
        }
        public static string GetLastRequestXml()
        {
            return res.lastRequest;
        }
        public static string GetLastResponseXml()
        {
            return res.lastResponse;
        }

        public static TableData GetTableDataBySQL(string strSQL, csConnection conn, ref XMLResponse res)
        {
            res.Reset();
            TableData datas = new TableData();
            List<string> containers = new List<string>();
            string xml = CQuery.GetQueryXML(strSQL, conn.user, conn.password);
            res = TxnCommon.GetResponse(xml, conn.GetServerConnection());
            if (!res.Success())
            {
                return datas;
            }
            datas = CQuery.GetTableDataFromResponse(res.lastResponse);

            return datas;
        }
    }
}

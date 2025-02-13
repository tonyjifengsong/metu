using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{
    /// <summary>
    /// MoveStd服务的XML生成类
    /// </summary>
    public class MoveStd
    {
        public const string template =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?><__InSite __version=\"1.1\"><__session><__connect><user><__name></__name></user>"
            + "<password __encrypted=\"no\"></password></__connect></__session><__service __serviceType=\"MoveStd\"><__utcOffset><![CDATA[08:00:00]]>"
            + "</__utcOffset><__inputData></__inputData><__requestData><CompletionMsg/></__requestData></__service></__InSite>";

        public string contName = "";
        // public csDCD dcd = new csDCD();
        public string PDDName = "";
        public List<object> PDDParams = new List<object>();
        public csConnection conn;
        public string isToDiscard = "";//是否报废
        public string countOfOperator = "";//操作人数

        // public csContainer contInfo = new csContainer();//批次当前信息
        public XMLResponse res = new XMLResponse("", "", exceptType.normal, true, "", "");
        public string resource = "";

        public MoveStd()
        {
            conn = new csConnection("", 2881, "", "");
        }

        public MoveStd(string serverIn, int portIn, string userIn, string pwIn)
        {
            this.conn = new csConnection(serverIn, portIn, userIn, pwIn);
        }

        public MoveStd(csConnection connIn)
        {
            this.conn = connIn;
        }

        //加载模板
        public static string LoadTemplate()
        {
            return template;
        }

        //设置数据收集定义
        //public static string SetDataCollectionDef(string xml, csDCD dcd)
        //{
        //    string strRet = XMLOps.InsertValue(xml, "inputData", dcd.ToString());
        //    return strRet;
        //}

        //添加PDD
        public static string AddPDD(string xml, string pddName)
        {
            if (pddName == "" || pddName == null)
            {
                return xml;
            }
            string strPDD = "<ParametricData __action=\"create\" __CDOTypeName=\"" + pddName
                + "\" > " + "</ParametricData>";
            string strRet = XMLOps.InsertValue(xml, "inputData", strPDD);
            return strRet;
        }

        //添加PDD参数-非List类型
        public static string AddPDDValue(string xml, string name, string value)
        {
            string strParam = XMLOps.GetNodeString(name, value, false);
            string strRet = XMLOps.InsertValue(xml, "ParametricData", strParam);
            return strRet;
        }

        //添加PDD的List类型参数
        public static string AddPDDListValue(string xml, string name, List<string> values)
        {
            string strRet = "";
            string strValues = "";
            strValues += "<" + name + ">";
            for (int i = 0; i < values.Count; i++)
            {
                strValues += XMLOps.GetNodeString("listItem", values[i], true);
            }
            strValues += "</" + name + ">";
            strRet = XMLOps.InsertValue(xml, "ParametricData", strValues);
            return strRet;
        }

        //过站请求
        public bool MoveRequest()
        {
            if (Common.IsStringNull(contName))
            {
                res.result = false;
                res.errorDiscription = "未设置批次号";
                return false;
            }
            string strXML = LoadTemplate();
            strXML = TxnCommon.SetAccount(strXML, conn.user);
            strXML = TxnCommon.SetPassword(strXML, conn.password);
            strXML = TxnCommon.SetContainer(strXML, contName);
            strXML = TxnCommon.SetResource(strXML, resource);
            strXML = XMLOps.EditHeadNodeParam(strXML, "service", "serviceType", "BYD_MoveStd");//修改服务名
            //if (!Common.IsStringNull(dcd.name))
            //{
            //    strXML = SetDataCollectionDef(strXML, dcd);
            //}
            if (!Common.IsStringNull(PDDName))
            {
                strXML = AddPDD(strXML, PDDName);
                for (int i = 0; i < PDDParams.Count; i++)
                {
                    if (PDDParams[i] is prmValPair)
                    {
                        prmValPair pair = (prmValPair)PDDParams[i];
                        strXML = AddPDDValue(strXML, pair.name, pair.value);
                    }
                    else if (PDDParams[i] is List<prmValPair>)
                    {
                        List<prmValPair> pairs = (List<prmValPair>)PDDParams[i];
                        string name = pairs[0].name;
                        List<string> vals = new List<string>();
                        for (int j = 0; j < pairs.Count; j++)
                        {
                            vals.Add(pairs[j].value);
                        }
                        strXML = AddPDDListValue(strXML, name, vals);
                    }
                }
            }
            //插入是否报废节点
            if (!Common.IsStringNull(isToDiscard))
            {
                string discardXml = XMLOps.GetNodeString("IsToDiscard", isToDiscard, false);
                strXML = XMLOps.InsertValue(strXML, "inputData", discardXml);
            }
            if (!Common.IsStringNull(countOfOperator))
            {
                string operatorXml = XMLOps.GetNodeString("CountOfOperator", countOfOperator, false);
                strXML = XMLOps.InsertValue(strXML, "inputData", operatorXml);
            }
            strXML = TxnCommon.AddExecute(strXML);
            res = TxnCommon.GetResponse(strXML, conn.GetServerConnection());
            return res.Success();
        }


        public string GetErrorInfo()
        {
            return res.GetErrorInfo();
        }

        public string GetSuccessInfo()
        {
            return res.successMessage;
        }

        public string GetLastRequestXml()
        {
            return res.lastRequest;
        }

        public string GetLastResponseXml()
        {
            return res.lastResponse;
        }


        /// <summary>
        /// 添加返工原因
        /// </summary>
        /// <param name="str"></param>
        /// <param name="reworkReason"></param>
        /// <returns></returns>
        public static string AddReworkReason(string str, string reworkReason)
        {
            string strValue = "";
            string strTemp = XMLOps.GetNodeString("ReworkReason", "", false);
            strValue += strTemp;
            strTemp = XMLOps.GetNodeString("name", reworkReason, true);
            strValue = XMLOps.InsertValue(strValue, "ReworkReason", strTemp);

            return XMLOps.InsertValue(str, "inputData", strValue);
        }

        //添加到工序
        public static string AddToStep(string str, string toStep)
        {
            string strValue = "";
            string strTemp = XMLOps.GetNodeString("ToStep", "", false);
            strValue += strTemp;
            strTemp = XMLOps.GetNodeString("name", toStep, true);
            strValue = XMLOps.InsertValue(strValue, "ToStep", strTemp);

            return XMLOps.InsertValue(str, "inputData", strValue);
        }


        /// <summary>
        /// 添加到工作流
        /// </summary>
        /// <param name="str"></param>
        /// <param name="toWorkflow"></param>
        /// <returns></returns>
        //public static string AddToWorkflow(string str, csRO toWorkflow)
        //{
        //    return XMLOps.InsertValue(str, "inputData", toWorkflow.ToXmlString());
        //}

        //添加返工工序
        public static string AddReEntryStep(string str, string ReEntryStep)
        {
            string strValue = "";
            string strTemp = XMLOps.GetNodeString("ReEntryStep", "", false);
            strValue += strTemp;
            strTemp = XMLOps.GetNodeString("Id", ReEntryStep, true);


            strValue = XMLOps.InsertValue(strValue, "ReEntryStep", strTemp);

            return XMLOps.InsertValue(str, "inputData", strValue);
        }



        public static class CQuery
        {
            private static string template = "<__InSite __version=\"1.1\"><__session><__connect><user><__name></__name></user><password __encrypted = \"no\">" +
                "</password></__connect></__session><__query><__queryText></__queryText></__query></__InSite>";
            public static string queryMfgLine(string container)
            {
                string text = "select C.CONTAINERNAME,ML.MFGLINENAME from " + "CONTAINER C " + "inner join A_MFGLINE ML on ML.MFGLINEID = C.MFGLINEID "
                                + "where CONTAINERNAME = '" + container + "'";
                return text;
            }

            //根据SQL查询返回的XML，解析表结构
            public static TableData GetTableDataFromResponse(string xml)
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


            /// <summary>
            /// 根据SQL查询，返回DataTable对象，所有字段名大写 by liyujie 20170301
            /// </summary>
            /// <param name="SQLQuery">要执行的查询语句，注意数据库查询语法的差异</param>
            /// <param name="connection">连接信息</param>
            /// <param name="response">返回信息结构体</param>
            /// <returns>DataTable对象</returns>
            public static void GetDataTableBySQL(string SQLQuery, csConnection connection, ref XMLResponse response, ref DataTable dtSysTable)
            {
                TableData tdUserTable = new TableData();
                string strRequest = XMLOps.InsertValue(template, "queryText", SQLQuery);
                strRequest = TxnCommon.SetAccount(strRequest, connection.user);
                strRequest = TxnCommon.SetPassword(strRequest, connection.password);
                //执行SQL查询语句
                response = TxnCommon.GetResponse(strRequest, new ServerConnection(connection.host, connection.port));
                if (!response.Success())
                {

                    return;
                }
                //获取放回的工单列表
                tdUserTable = CQuery.GetTableDataFromResponse(response.lastResponse);

                //将用户表转换为系统表格式

                CQuery.TableDataToDataTable(tdUserTable, ref dtSysTable);


                //返回系统表
                return;

            }


            /// <summary>
            /// 将从XML获取的TableData表对象信息转换为系统的DataTable,可以通过字段名引用，所有字段名都是大写
            /// </summary>
            /// <param name="userTableData"></param>
            /// <returns></returns>
            public static void TableDataToDataTable(TableData userTableData, ref DataTable SysDatatable)
            {
                //DataTable SysDatatable = new DataTable();


                try
                {
                    //遍历字段名
                    foreach (string column in userTableData.columns)
                    {
                        string strcolumn = column.ToUpper();
                        //检查字段名是否已经在表中存在
                        int index = SysDatatable.Columns.IndexOf(strcolumn);
                        //如果已经存在就不再添加字段
                        if (index < 0)
                        {
                            SysDatatable.Columns.Add(strcolumn);

                        }
                    }


                    //遍历行数据
                    foreach (List<string> Listvalue in userTableData.datas)
                    {

                        SysDatatable.Rows.Add();


                        //遍历字段名
                        for (int i = 0; i < SysDatatable.Columns.Count; i++)
                        {
                            //string column = SysDatatable.Columns[i].ColumnName;
                            //往字段里面赋值
                            SysDatatable.Rows[SysDatatable.Rows.Count - 1][i] = Listvalue[i];

                        }

                    }
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                }

            }

            //根据sql语句获取TableData
            public static TableData QueryTableData(string sql, csConnection conn, ref XMLResponse res)
            {
                string xml = GetQueryXML(sql, conn.user, conn.password);
                res = TxnCommon.GetResponse(xml, conn.GetServerConnection());
                if (!res.Success())
                {
                    return new TableData();
                }
                return GetTableDataFromResponse(res.lastResponse);
            }

            //根据查询语句，获取查询XML
            public static string GetQueryXML(string text, string user, string password)
            {
                string strRet = XMLOps.InsertValue(template, "queryText", text);
                strRet = XMLOps.InsertValue(strRet, "name", user);
                strRet = XMLOps.InsertValue(strRet, "password", password);
                return strRet;
            }

            //根据批次号查询产线
            public static string MfgLineByContainer(string container, string user, string password)
            {
                string strRet = "select C.CONTAINERNAME,ML.MFGLINENAME from " + Environment.NewLine +
                    "CONTAINER C" + Environment.NewLine +
                    "inner join A_MFGLINE ML on ML.MFGLINEID = C.MFGLINEID" + Environment.NewLine +
                    "where CONTAINERNAME = '" + container + "'";
                return GetQueryXML(strRet, user, password);
            }

            //根据产线和Spec过滤容器
            public static string FilterContainerBySpecAndMfgLine(string strMfgOrder, string mfgLine, string SpecName, string SpecRev, string user, string password)
            {
                string sql = "";
                //csRO spec = new csRO("Spec", SpecName, SpecRev);
                sql += "Select C.Containername,pb.ProductName,p.Description,C.Qty,CS.Lastmovedate OriginalStartDate,p.productrevision Ver,MO.MfgorderName "
                    + "from Container C inner join A_MFGLINE ML on ML.MFGLINEID=C.MFGLINEID "
                    + "inner join MfgOrder MO on C.Mfgorderid=MO.Mfgorderid "
                    + "inner join CurrentStatus CS on C.Currentstatusid=CS.Currentstatusid "
                    + "inner join Spec Sp On CS.SpecID=SP.SpecID "
                    + "inner join Containerlevel cl on c.levelid = cl.containerlevelid and cl.containerlevelname not in (N'绑定',N'单件') "
                    + "inner join SpecBase Spb On Spb.Specbaseid=Sp.Specbaseid "
                    + "inner join Product p on C.ProductId = p.ProductId "
                    + "inner join ProductBase pb on p.ProductBaseId = pb.ProductBaseId "
                    + "inner join ProductType pt on p.ProductTypeId = pt.ProductTypeId "
                    + "where C.Qty>0 and " + "Spb.Specname=N'" + SpecName + "' "
                    + " and pt.ProductTypeName in ('RAW',N'原材料','COMPONENT',N'半成品',N'成品') and  ML.Mfglinename='" + mfgLine + "' and MO.MfgorderName like '" + strMfgOrder + "%' "
                    + " order by C.OriginalStartDate";
                return GetQueryXML(sql, user, password);
            }

            //根据工厂和工作中心和产线得到产线工单生产状况
            public static string GetLineInformationByFactoryAndMfgLine(string Factory, string WorkCenter, string mfgLine, string user, string password)
            {
                string sql = "";
                sql += "Select ML.MfgLineName,PB.ProductName,P.Description,MO.MfgOrderName,MO.Qty,MO.ReleaseDate,"
                   + "Sum(Case when wfs.workflowstepname = 'GYXS-ZB' then 1 else 0 end) as SumCount,"
                   + "Sum(Case when wfs.workflowstepname = 'GYXS-ZB' and Z.LASTMOVEDATE > to_date(to_char(sysdate, 'yyyy-MM-dd'), 'yyyy-MM-dd') then 1 else 0 end) as CurrentCount "
                   + "from Container C inner join MfgOrder MO on MO.MfgOrderId = C.MfgOrderId "
                   + "inner join A_MFGLINE ML on ML.MFGLINEID = C.MFGLINEID "
                   + "inner join Product P on C.ProductId = P.ProductId "
                   + "inner join ProductBase PB on P.ProductBaseId = PB.ProductBaseId "
                   + "inner join Currentstatus Z on C.CURRENTSTATUSID = Z.CURRENTSTATUSID "
                   + "inner join Workflowstep wfs on Z.workflowstepid = wfs.workflowstepid "
                   + "inner join Factory F on F.FactoryId = Z.FactoryId "
                   + "where ML.Mfglinename = '" + mfgLine + "' and F.FactoryName = '" + Factory + "' "
                   + "and C.Containername like '01046%' and MO.ReleaseDate >= to_date(to_char(sysdate-1, 'yyyy-MM-dd'), 'yyyy-MM-dd') "
                   + "Group by ML.MfgLineName,PB.ProductName,P.Description,MO.MfgOrderName,MO.Qty,MO.ReleaseDate";
                return GetQueryXML(sql, user, password);
            }

            //得到工厂信息
            public static string GetFactory(string user, string password)
            {
                string sql = "";
                sql += "Select distinct FactoryName as Value,Description as Name from Factory order by FactoryName";
                return GetQueryXML(sql, user, password);
            }

            //得到产线信息
            public static string GetMfgLine(string user, string password)
            {
                string sql = "";
                sql += "Select distinct MfgLineName as Value,Description as Name from A_MfgLine order by MfgLineName";
                return GetQueryXML(sql, user, password);
            }

            //获取最后返工原因
            public static string GetLastReworkReason_Sql(string container, string user, string password)
            {
                string sql =
                    "select hm.ContainerName,rr.ReworkReasonName ReworkReasonName,hm.LastMoveDate from HistoryMainline hm " +
                    "inner join MoveHistory mh on hm.HistoryMainlineId=mh.HistoryMainlineId " +
                    "inner join ReworkReason rr on rr.ReworkReasonId=mh.ReworkReasonId " +
                    "where hm.ContainerName='" + container + "'" + " order by LastMoveDate desc";
                return GetQueryXML(sql, user, password);
            }

            //查询工站产品堆积的Sql。ws:工站编号。finishedProdType:成品的产品类型名
            public static string QueryWSContainerSql(string ws, string finishedProdType)
            {
                string sql = "select C.ContainerName " + Environment.NewLine +
                    "from " + Environment.NewLine +
                    "Container C " + Environment.NewLine +
                    "inner join CurrentStatus CS on C.CurrentStatusId=CS.CurrentStatusId " + Environment.NewLine +
                    "inner join Spec Sp on Sp.SpecId=CS.SpecId " + Environment.NewLine +
                    "inner join ResourceGroupEntries RGE on RGE.ResourceGroupId=Sp.ResourceGroupId " + Environment.NewLine +
                    "inner join ResourceDef RD on RD.ResourceId=RGE.EntriesId and RD.MfgLineId=C.MfglineId " + Environment.NewLine +
                    "inner join CDODefinition CDef on CDef.CDODefId=RD.CDOTypeId and " + Environment.NewLine +
                    "CDef.CDOName='BYD_Workstation' " + Environment.NewLine +
                    "inner join Product P on C.ProductID=P.ProductID " + Environment.NewLine +
                    "inner join ProductType PT on PT.ProductTypeID=P.ProductTypeID " + Environment.NewLine +
                    "where PT.ProductTypeName='" + finishedProdType + "' and RD.ResourceName='" + ws +
                    "'";
                return sql;
            }

            //获取故障维修设备。cdoType=资源的CDO类型，statusName=状态名称
            public static string QueryResAbnormalSql(List<string> cdoTypes, string statusName = "故障维修")
            {
                string sql = "select rd.resourcename,rsc.resourcestatuscodename,cdo.cdoname" +
                    " from productionstatus pts " +
                    " inner join resourcestatuscode rsc on pts.statusid=rsc.resourcestatuscodeid" +
                    " inner join resourcedef rd on rd.resourceid=pts.resourceid" +
                    " inner join cdodefinition cdo on cdo.cdodefid=rd.cdotypeid" +
                    " where rsc.resourcestatuscodename='" + statusName + "'";
                //if (Common.IsStrsValid(cdoTypes))
                //{
                //    sql += " and(";
                //    for (int i = 0; i < cdoTypes.Count; i++)
                //    {
                //        if (i > 0)
                //        {
                //            sql += " or";
                //        }
                //        sql += " cdo.cdoname='" + cdoTypes[i] + "'";
                //    }
                //    sql += ")";
                //}
                return sql;
            }

            //获取故障维修设备。cdoType=资源的CDO类型，statusName=状态名称
            public static string QueryResAbnormalSql(string cdoType, string statusName = "故障维修")
            {
                List<string> types = new List<string>() { cdoType };
                return QueryResAbnormalSql(types, statusName);
            }

            //获取停线工单
            public static string QueryMfgOrderStop(string stopName = "暂停")
            {
                string sql = "select mo.mfgordername,pt.producttypename,t.byd_stoplinereasonname stopreason" + Environment.NewLine +
                    " from mfgorder mo" + Environment.NewLine +
                    " inner join orderstatus os on mo.orderstatusid=os.orderstatusid" + Environment.NewLine +
                    " inner join product p on p.productbaseid=mo.productbaseid" + Environment.NewLine +
                    " inner join producttype pt on pt.producttypeid=p.producttypeid" + Environment.NewLine +
                    " left join(select * from(" + Environment.NewLine +
                    " select * from byd_mfgorderhistory moh" + Environment.NewLine +
                    " inner join historymainline hm on moh.historymainlineid=hm.historymainlineid" + Environment.NewLine +
                    " inner join byd_stoplinereason slr on slr.byd_stoplinereasonid=moh.byd_stoplinereasonid" + Environment.NewLine +
                    " order by hm.txndate desc )" + Environment.NewLine +
                    " where rownum=1" + Environment.NewLine +
                    " ) t on mo.mfgorderid=t.mfgorderid" + Environment.NewLine +
                    " where os.orderstatusname='" + stopName + "'";

                return sql;
            }

            //查询工位历史的sql语句
            public static string QueryWsHistorySql(string ws, DateTime startTime, DateTime endTime)
            {
                string sql = "select HM.TxnTypeName,HM.TxnDate,HM.ContainerName from HistoryMainline HM " +
                    "inner join ResourceDef RD on RD.ResourceId=HM.ResourceId where RD.ResourceName='" + ws + "'" +
                    "and HM.Txndate > to_date('" + startTime.ToString() + "','yyyy-mm-dd hh24:mi:ss')" +
                    "and HM.Txndate < to_date('" + endTime.ToString() + "','yyyy-mm-dd hh24:mi:ss')" +
                    "order by HM.TxnDate";
                sql = sql.Replace(">", "&gt;");
                sql = sql.Replace("<", "&lt;");
                return sql;
            }

            ////查询工位返工历史的sql语句
            //public static string QueryWsReworkHistorySql(string ws, DateTime startTime, DateTime endTime)
            //{
            //    string sql = "select HM.TxnDate,HM.TxnTypeName,HM.ContainerName,RR.Reworkreasonname,pb.productname,p.productrevision rev" + Environment.NewLine +
            //        " from HistoryMainline HM " + Environment.NewLine +
            //        "inner join ResourceDef RD on RD.ResourceId=HM.ResourceId " + Environment.NewLine +
            //        "inner join MoveHistory MH on MH.Historymainlineid=HM.Historymainlineid " + Environment.NewLine +
            //        "inner join Reworkreason RR on MH.Reworkreasonid=RR.Reworkreasonid " + Environment.NewLine +
            //        "inner join Container c on HM.Containername=c.containername " + Environment.NewLine +
            //        "inner join product p on p.productid=c.productid " + Environment.NewLine +
            //        "inner join productbase pb on p.productbaseid=pb.productbaseid " + Environment.NewLine +
            //        "where HM.Txntypename='Rework' " + "and RD.ResourceName='" + ws + "'" + Environment.NewLine +
            //        "and HM.Txndate > to_date('" + startTime.ToString() + "','yyyy-mm-dd hh24:mi:ss') " + Environment.NewLine +
            //        "and HM.Txndate < to_date('" + endTime.ToString() + "','yyyy-mm-dd hh24:mi:ss') " + Environment.NewLine +
            //        "order by HM.TxnDate ";
            //    sql = BYDSQLScript.ReplaceSQlSymbol(sql);
            //    return sql;
            //}

            //获取资源类型
            public static string GetResourceType(string name, csConnection conn, ref XMLResponse res)
            {
                if (Common.IsStringNull(name))
                {
                    return null;
                }
                string sql = "select cdo.cdoname from resourcedef res " +
                    "inner join cdodefinition cdo on cdo.cdodefid=res.cdotypeid " +
                    "where res.resourcename='" + name + "'";
                string xml = GetQueryXML(sql, conn.user, conn.password);
                res = TxnCommon.GetResponse(xml, conn.GetServerConnection());
                string xmlRet = res.lastResponse;
                TableData table = GetTableDataFromResponse(xmlRet);
                return table.datas[0][0];
            }

            //获取资源的吞吐量和最大吞吐量
            public static string GetResourceThruput(string resourceName)
            {
                string sql = "select ps.TotalThruputQty-ms.LastThruputQty as \"ThruputQty\",mr.Qty as \"MaxQty\" from ResourceDef rd" + Environment.NewLine
                    + "inner join ProductionStatus ps on ps.ResourceId = rd.ResourceId" + Environment.NewLine
                    + "inner join MaintenanceStatus ms on ms.ResourceId = rd.ResourceId" + Environment.NewLine
                    + "inner join AssignedMaintReq amr on amr.AssignedMaintReqId = ms.AssignedMaintReqId" + Environment.NewLine
                    + "inner join MaintenanceReq mr on mr.MaintenanceReqId = amr.MaintenanceReqId" + Environment.NewLine
                    + "where rd.ResourceName = '" + resourceName + "' and ms.LastThruputQty is not null" + Environment.NewLine;

                return sql;
            }

            //根据Spec名称获取一个Container
            public static string GetAContainerBySpec(string specName)
            {
                string sql = "select c.containername from container c" + Environment.NewLine
                    + "inner join currentstatus cs on c.currentstatusid=cs.currentstatusid" + Environment.NewLine
                    + "inner join spec s on cs.specid=s.specid" + Environment.NewLine
                    + "inner join specbase sb on s.specbaseid=sb.specbaseid" + Environment.NewLine
                    + "where sb.specname='" + specName + "'" + Environment.NewLine
                    + "and rownum=1" + Environment.NewLine;
                return sql;
            }

            //获取默认资源组
            public static string GetDefaultResourceGroup(string resource)
            {
                string sql = "select rg.resourcegroupname from resourcegroup rg" + Environment.NewLine
                    + "inner join resourcegroupentries rge on rge.resourcegroupid=rg.resourcegroupid" + Environment.NewLine
                    + "inner join resourcedef rd on rd.resourceid=rge.entriesid" + Environment.NewLine
                    + "where rd.resourcename='" + resource + "'" + Environment.NewLine;
                return sql;
            }

            //获取默认Spec
            public static string GetDefaultSpec(string resGroup)
            {
                string sql = "select sb.specname from spec s" + Environment.NewLine
                    + "inner join specbase sb on s.specbaseid=sb.specbaseid" + Environment.NewLine
                    + "inner join resourcegroup rg on rg.resourcegroupid=s.resourcegroupid" + Environment.NewLine
                    + "where rg.resourcegroupname='" + resGroup + "'" + Environment.NewLine;
                return sql;
            }
        }





    }


    public class Start
    {

        public string strIP, strUsername, strPassword;
        public int iPort;
        /// <summary>
        /// 可填充Start基本信息
        /// </summary>
        public struct StartInfo
        {
            public string strMfgOrder;            //工单号
            public string strContainer;           //Container
            public string strStarQty;             //Star数量
            public string strPorductRev;          //产品版本
            public string strOwner;               //负责人
            public string strPorductName;          //产品编码
            public string strLevel;               //批次等级
            public string strStartReason;         //开始原因
            public string strStep;                //步骤
            public string strWorkflow;            //工作流
            public string strMfgLine;               //线体
            public string strCapacity;              //最大装箱数
            public string strOrderStatus;              //工单状态
        }

        /// <summary>
        /// 返回的内容
        /// </summary>
        public struct ResponeStartInfo
        {
            public string strMfgOrder;            //工单号
            public string strContainer;           //Container
            public string strStarQty;             //Star数量
            public string strPorductName;          //产品编码
            public string strPorductRev;          //产品版本
            public string strPorductDesc;           //产品描述
            public string strOwner;               //负责人
            public string strLevel;               //批次等级
            public string strStartReason;         //开始原因
            public string strStep;                //步骤
            public string strWorkflow;            //工作流
            public string strMfgLine;               //线体
            public string strMfgOrderQty;           //工单数量
            public string strMfgOrderQtyStarted;           //工单已开始数量
            public string strCapacity;              //最大装箱数
            public string strChildCount;            //子Contianer数量
            public string strDescriptionInEnglish;      //英文描述
            public string strDescriptionInOtherLanguage;        //其他语言描述
            public string strDescriptionAppendForCode; //得到附加码
            public string strParameter1;                //标签参数1
            public string strParameter2;                //标签参数2
            /// <summary>
            /// 是否产生了错误
            /// </summary>
            public bool bErrorFlag
            {
                get;
                internal set;
            }


            /// <summary>
            /// 错误信息
            /// </summary>
            public string strErrorText
            {
                get;
                internal set;
            }
        }


        /// <summary>
        /// Start的基本信息
        /// </summary>
        public StartInfo rqStartInfo;

        /// <summary>
        /// 返回的信息
        /// </summary>
        public ResponeStartInfo rpStartInfo;

        /// <summary>
        /// 返回的XMLResponse类
        /// </summary>
        public XMLResponse res;

        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// 
        public Start(string strip, int iport, string strusername, string strpassword)
        {
            //检测参数是否为空
            if (Common.IsStringNull(strip) || Common.IsStringNull(strusername) || Common.IsStringNull(strpassword) || Common.IsStringNull(iport.ToString()))
            {
                return;

            }
            strIP = strip;
            iPort = iport;
            strUsername = strusername;
            strPassword = strpassword;

        }

        public Start()
        {
            //strIP = SystemBaseInfo.sysConnection.host;
            //iPort = SystemBaseInfo.sysConnection.port;
            //strUsername = SystemBaseInfo.sysConnection.user;
            //strPassword = SystemBaseInfo.sysConnection.password;

        }
        #endregion

        #region Start事务类
        /// <summary>
        /// 执行Start,需要填充的数据在rqStartInfo属性中填充,如果使用工单号生成，会调用MfgOrderStar服务创建Contianer，否则使用Star
        /// </summary>
        /// <param name="ExecuteFalg">是否执行事务</param>
        /// <returns></returns>
        public bool StartGO(bool ExecuteFalg = true)
        {
            #region 节点设置
            //插入工单
            List<string> lstMfgOrder = new List<string>();
            lstMfgOrder.Add("MfgOrder");
            lstMfgOrder.Add("name");

            //获取Container名称
            List<string> lstContainerName = new List<string>();
            lstContainerName.Add("Container");
            lstContainerName.Add("name");

            //插入Container数量
            List<string> lstStartQty = new List<string>();
            lstStartQty.Add("Details");
            lstStartQty.Add("Qty");

            //获取Container数量
            List<string> lstRPStartQty = new List<string>();
            lstRPStartQty.Add("Container");
            lstRPStartQty.Add("Qty");

            //获得工单数量
            List<string> lstMfgOrderQty = new List<string>();
            lstMfgOrderQty.Add("MfgOrder");
            lstMfgOrderQty.Add("Qty");

            //获得工单已开始数量
            List<string> lstMfgOrderQtyStarted = new List<string>();
            lstMfgOrderQtyStarted.Add("MfgOrder");
            lstMfgOrderQtyStarted.Add("QtyStarted");

            List<string> lstProduct = new List<string>();
            lstProduct.Add("Product");
            lstProduct.Add("name");

            List<string> lstProductrev = new List<string>();
            lstProductrev.Add("Product");
            lstProductrev.Add("rev");

            List<string> lstPorductDesc = new List<string>();
            lstPorductDesc.Add("Product");
            lstPorductDesc.Add("Description");

            List<string> lstOwner = new List<string>();
            lstOwner.Add("Owner");
            lstOwner.Add("name");

            List<string> lstLevel = new List<string>();
            lstLevel.Add("Level");
            lstLevel.Add("name");

            List<string> lstStartReason = new List<string>();
            lstStartReason.Add("StartReason");
            lstStartReason.Add("name");

            List<string> lstWorkflow = new List<string>();
            lstWorkflow.Add("Workflow");
            lstWorkflow.Add("name");

            List<string> lstWorkflowStep = new List<string>();
            lstWorkflowStep.Add("WorkflowStep");
            lstWorkflowStep.Add("name");

            List<string> lstMfgLine = new List<string>();
            lstMfgLine.Add("MfgLine");
            lstMfgLine.Add("name");

            List<string> lstCapacity = new List<string>();
            lstCapacity.Add("Capacity");

            List<string> lstOrderStatus = new List<string>();
            lstOrderStatus.Add("OrderStatus");
            lstOrderStatus.Add("name");

            #endregion

            #region XML脚本   

            string strXmlRequest = "<__InSite __version=\"1.1\">" +
"	<__session>" +
"		<__connect>" +
"			<user>" +
"				<__name></__name>" +
"			</user>" +
"			<password __encrypted=\"no\"></password>" +
"		</__connect>" +
"	</__session>" +
"	<__service __serviceType=\"BYD_Start\">" +
"		<__utcOffset>08:00</__utcOffset>" +
"		<__inputData>" +
"            <Capacity></Capacity>   " +
"			<Details>" +
"				<ContainerName></ContainerName>" +
"				<Level>" +
"					<__name></__name>" +
"				</Level>" +
"				<Product>" +
"					<__name></__name>" +
"					<__rev></__rev>" +
"                   <__useROR>true</__useROR>" +
"				</Product>" +
"				<Owner>" +
"					<__name></__name>" +
"				</Owner>" +
"				<StartReason>" +
"					<__name></__name>" +
"				</StartReason>" +
"				<MfgLine>" +
"					<__name></__name>" +
"				</MfgLine>" +
"				<MfgOrder>" +
"					<__name></__name>" +
"				</MfgOrder>" +
"				<Qty></Qty>" +
"			</Details>" +
"			<CurrentStatusDetails>" +
"				<Workflow>" +
"					<__name>" +
"					</__name>" +
"					<__rev></__rev>" +
"                   <__useROR>true</__useROR>" +
"				</Workflow>" +
"				<WorkflowStep>" +
"					<__name>" +
"					</__name>" +
"				</WorkflowStep>" +
"			</CurrentStatusDetails>" +
"			<OrderStatus>" +
"				<__name>" +
"				</__name>" +
"			</OrderStatus>" +
"		</__inputData>" +

"		<__requestData>" +
"           <Capacity/>" +
"			<Container>" +
"				<Workflow/>" +
"				<Level/>" +
"				<UOM/>" +
"				<Product>" +
"					<name></name>" +
"					<Revision></Revision>" +
"					<Description/><DescriptionInEnglish/><DescriptionInOtherLanguage/><DescriptionAppendForCode/><Parameter1/><Parameter2/>" +
"				</Product>" +
"				<Owner/>" +
"				<StartReason/>" +
"				<MfgLine/>" +
"<ChildCount/>" +
"				<WorkflowStep/>" +
"				<Qty/>" +
"" +
"			</Container>" +
"			<MfgOrder>" +
"				<Qty>" +
"" +
"				</Qty>" +
"				<QtyStarted></QtyStarted>" +
"			</MfgOrder>" +
"		</__requestData>" +
"	</__service>" +
"</__InSite>";

            #endregion

            #region 填充账号信息
            strXmlRequest = TxnCommon.SetAccount(strXmlRequest, strUsername);
            strXmlRequest = TxnCommon.SetPassword(strXmlRequest, strPassword);
            #endregion

            #region 插入工单名称  MfgOrder
            if (!Common.IsStringNull(rqStartInfo.strMfgOrder))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstMfgOrder, rqStartInfo.strMfgOrder);
            }
            #endregion

            #region 插入数量 StarQty
            //double qty = Common.ParseDouble(rqStartInfo.strStarQty);
            //if (qty < 0)
            //{
            //    res = Common.resCommon;
            //    res.SetErrorInfo("启动数量无效!");
            //    return false;
            //}
            strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstStartQty, rqStartInfo.strStarQty);
            #endregion

            #region 插入Container
            if (!Common.IsStringNull(rqStartInfo.strContainer))
            {

                strXmlRequest = XMLOps.InsertValue(strXmlRequest, "ContainerName", rqStartInfo.strContainer);
            }
            #endregion

            #region 插入产品 Product
            if (!Common.IsStringNull(rqStartInfo.strPorductName))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstProduct, rqStartInfo.strPorductName);
                if (rqStartInfo.strPorductRev != "")
                    strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstProductrev, rqStartInfo.strPorductRev);
            }
            #endregion

            #region 插入负责人 Owner

            if (!Common.IsStringNull(rqStartInfo.strOwner))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstOwner, rqStartInfo.strOwner);
            }

            #endregion

            #region 插入等级 Level

            if (!Common.IsStringNull(rqStartInfo.strLevel))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstLevel, rqStartInfo.strLevel);
            }


            #endregion

            #region 插入开始原因 StartReason
            if (!Common.IsStringNull(rqStartInfo.strStartReason))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstStartReason, rqStartInfo.strStartReason);
            }
            #endregion

            #region 插入工作流 Workflow

            if (!Common.IsStringNull(rqStartInfo.strWorkflow))
            {
                //插入负责人
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstWorkflow, rqStartInfo.strWorkflow);

            }
            #endregion

            #region 插入步骤   Step 
            if (!Common.IsStringNull(rqStartInfo.strStep))
            {
                //插入负责人

                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstWorkflowStep, rqStartInfo.strStep);

            }
            #endregion

            #region 线体 MfgLine
            if (!Common.IsStringNull(rqStartInfo.strMfgLine))
            {

                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstMfgLine, rqStartInfo.strMfgLine);
            }
            #endregion

            #region 容量/最大装箱数 Capacity
            if (!Common.IsStringNull(rqStartInfo.strMfgLine))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstCapacity, rqStartInfo.strCapacity);
            }
            #endregion

            #region 工单状态 OrderStatus
            if (!Common.IsStringNull(rqStartInfo.strOrderStatus))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstOrderStatus, rqStartInfo.strOrderStatus);
            }
            #endregion

            #region 是否执行事务
            if (ExecuteFalg)
            {
                strXmlRequest = TxnCommon.AddExecute(strXmlRequest);
            }
            #endregion

            #region 执行事务
            //创建连接
            ServerConnection serConnection = new ServerConnection(strIP, iPort);


            res = TxnCommon.GetResponse(strXmlRequest, serConnection);
            //判断是否有异常
            if (!res.Success())
            {
                rpStartInfo.bErrorFlag = false;
                rpStartInfo.strErrorText = res.errorDiscription;
                return false;
            }
            #endregion

            #region 获取返回结果
            //返回结果赋值
            rpStartInfo.strContainer = XMLOps.ReadAtNode(res.lastResponse, lstContainerName);
            rpStartInfo.strStarQty = XMLOps.ReadAtNode(res.lastResponse, lstRPStartQty);
            rpStartInfo.strMfgOrder = XMLOps.ReadAtNode(res.lastResponse, lstMfgOrder);
            rpStartInfo.strPorductName = XMLOps.ReadAtNode(res.lastResponse, lstProduct);
            rpStartInfo.strPorductRev = XMLOps.ReadAtNode(res.lastResponse, lstProductrev);
            rpStartInfo.strOwner = XMLOps.ReadAtNode(res.lastResponse, lstOwner);
            rpStartInfo.strLevel = XMLOps.ReadAtNode(res.lastResponse, lstLevel);
            rpStartInfo.strStartReason = XMLOps.ReadAtNode(res.lastResponse, lstStartReason);
            rpStartInfo.strWorkflow = XMLOps.ReadAtNode(res.lastResponse, lstWorkflow);
            rpStartInfo.strStep = XMLOps.ReadAtNode(res.lastResponse, lstWorkflowStep);
            rpStartInfo.strMfgLine = XMLOps.ReadAtNode(res.lastResponse, "MfgLine");
            rpStartInfo.strPorductDesc = XMLOps.ReadAtNode(res.lastResponse, lstPorductDesc);
            rpStartInfo.strMfgOrderQty = XMLOps.ReadAtNode(res.lastResponse, lstMfgOrderQty);
            rpStartInfo.strMfgOrderQtyStarted = XMLOps.ReadAtNode(res.lastResponse, lstMfgOrderQtyStarted);
            rpStartInfo.strCapacity = XMLOps.ReadAtNode(res.lastResponse, lstCapacity);
            rpStartInfo.strChildCount = XMLOps.ReadAtNode(res.lastResponse, "ChildCount");
            rpStartInfo.strDescriptionInEnglish = XMLOps.ReadAtNode(res.lastResponse, "DescriptionInEnglish");
            rpStartInfo.strDescriptionInOtherLanguage = XMLOps.ReadAtNode(res.lastResponse, "DescriptionInOtherLanguage");
            rpStartInfo.strDescriptionAppendForCode = XMLOps.ReadAtNode(res.lastResponse, "DescriptionAppendForCode");
            rpStartInfo.strParameter1 = XMLOps.ReadAtNode(res.lastResponse, "Parameter1");
            rpStartInfo.strParameter2 = XMLOps.ReadAtNode(res.lastResponse, "Parameter2");
            #endregion

            rpStartInfo.bErrorFlag = true;
            rpStartInfo.strErrorText = res.errorDiscription;
            return true;


        }

        #endregion

    }


    public static class CQuery
    {
        private static string template = "<__InSite __version=\"1.1\"><__session><__connect><user><__name></__name></user><password __encrypted = \"no\">" +
            "</password></__connect></__session><__query><__queryText></__queryText></__query></__InSite>";
        public static string queryMfgLine(string container)
        {
            string text = "select C.CONTAINERNAME,ML.MFGLINENAME from " + "CONTAINER C " + "inner join A_MFGLINE ML on ML.MFGLINEID = C.MFGLINEID "
                            + "where CONTAINERNAME = '" + container + "'";
            return text;
        }

        //根据SQL查询返回的XML，解析表结构
        public static TableData GetTableDataFromResponse(string xml)
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



        /// <summary>
        /// 根据SQL查询，返回DataTable对象，所有字段名大写 by liyujie 20170301
        /// </summary>
        /// <param name="SQLQuery">要执行的查询语句，注意数据库查询语法的差异</param>
        /// <param name="connection">连接信息</param>
        /// <param name="response">返回信息结构体</param>
        /// <returns>DataTable对象</returns>
        public static void GetDataTableBySQL(string SQLQuery, csConnection connection, ref XMLResponse response, ref DataTable dtSysTable)
        {
            TableData tdUserTable = new TableData();
            string strRequest = XMLOps.InsertValue(template, "queryText", SQLQuery);
            strRequest = TxnCommon.SetAccount(strRequest, connection.user);
            strRequest = TxnCommon.SetPassword(strRequest, connection.password);
            //执行SQL查询语句
            response = TxnCommon.GetResponse(strRequest, new ServerConnection(connection.host, connection.port));
            if (!response.Success())
            {

                return;
            }
            //获取放回的工单列表
            tdUserTable = CQuery.GetTableDataFromResponse(response.lastResponse);

            //将用户表转换为系统表格式

            CQuery.TableDataToDataTable(tdUserTable, ref dtSysTable);


            //返回系统表
            return;

        }


        /// <summary>
        /// 将从XML获取的TableData表对象信息转换为系统的DataTable,可以通过字段名引用，所有字段名都是大写
        /// </summary>
        /// <param name="userTableData"></param>
        /// <returns></returns>
        public static void TableDataToDataTable(TableData userTableData, ref DataTable SysDatatable)
        {
            //DataTable SysDatatable = new DataTable();


            try
            {
                //遍历字段名
                foreach (string column in userTableData.columns)
                {
                    string strcolumn = column.ToUpper();
                    //检查字段名是否已经在表中存在
                    int index = SysDatatable.Columns.IndexOf(strcolumn);
                    //如果已经存在就不再添加字段
                    if (index < 0)
                    {
                        SysDatatable.Columns.Add(strcolumn);

                    }
                }


                //遍历行数据
                foreach (List<string> Listvalue in userTableData.datas)
                {

                    SysDatatable.Rows.Add();


                    //遍历字段名
                    for (int i = 0; i < SysDatatable.Columns.Count; i++)
                    {
                        //string column = SysDatatable.Columns[i].ColumnName;
                        //往字段里面赋值
                        SysDatatable.Rows[SysDatatable.Rows.Count - 1][i] = Listvalue[i];

                    }

                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

        }

        //根据sql语句获取TableData
        public static TableData QueryTableData(string sql, csConnection conn, ref XMLResponse res)
        {
            string xml = GetQueryXML(sql, conn.user, conn.password);
            res = TxnCommon.GetResponse(xml, conn.GetServerConnection());
            if (!res.Success())
            {
                return new TableData();
            }
            return GetTableDataFromResponse(res.lastResponse);
        }

        //根据查询语句，获取查询XML
        public static string GetQueryXML(string text, string user, string password)
        {
            string strRet = XMLOps.InsertValue(template, "queryText", text);
            strRet = XMLOps.InsertValue(strRet, "name", user);
            strRet = XMLOps.InsertValue(strRet, "password", password);
            return strRet;
        }

        //根据批次号查询产线
        public static string MfgLineByContainer(string container, string user, string password)
        {
            string strRet = "select C.CONTAINERNAME,ML.MFGLINENAME from " + Environment.NewLine +
                "CONTAINER C" + Environment.NewLine +
                "inner join A_MFGLINE ML on ML.MFGLINEID = C.MFGLINEID" + Environment.NewLine +
                "where CONTAINERNAME = '" + container + "'";
            return GetQueryXML(strRet, user, password);
        }

        //根据工厂和工作中心和产线得到产线工单生产状况
        public static string GetLineInformationByFactoryAndMfgLine(string Factory, string WorkCenter, string mfgLine, string user, string password)
        {
            string sql = "";
            sql += "Select ML.MfgLineName,PB.ProductName,P.Description,MO.MfgOrderName,MO.Qty,MO.ReleaseDate,"
               + "Sum(Case when wfs.workflowstepname = 'GYXS-ZB' then 1 else 0 end) as SumCount,"
               + "Sum(Case when wfs.workflowstepname = 'GYXS-ZB' and Z.LASTMOVEDATE > to_date(to_char(sysdate, 'yyyy-MM-dd'), 'yyyy-MM-dd') then 1 else 0 end) as CurrentCount "
               + "from Container C inner join MfgOrder MO on MO.MfgOrderId = C.MfgOrderId "
               + "inner join A_MFGLINE ML on ML.MFGLINEID = C.MFGLINEID "
               + "inner join Product P on C.ProductId = P.ProductId "
               + "inner join ProductBase PB on P.ProductBaseId = PB.ProductBaseId "
               + "inner join Currentstatus Z on C.CURRENTSTATUSID = Z.CURRENTSTATUSID "
               + "inner join Workflowstep wfs on Z.workflowstepid = wfs.workflowstepid "
               + "inner join Factory F on F.FactoryId = Z.FactoryId "
               + "where ML.Mfglinename = '" + mfgLine + "' and F.FactoryName = '" + Factory + "' "
               + "and C.Containername like '01046%' and MO.ReleaseDate >= to_date(to_char(sysdate-1, 'yyyy-MM-dd'), 'yyyy-MM-dd') "
               + "Group by ML.MfgLineName,PB.ProductName,P.Description,MO.MfgOrderName,MO.Qty,MO.ReleaseDate";
            return GetQueryXML(sql, user, password);
        }

        //得到工厂信息
        public static string GetFactory(string user, string password)
        {
            string sql = "";
            sql += "Select distinct FactoryName as Value,Description as Name from Factory order by FactoryName";
            return GetQueryXML(sql, user, password);
        }

        //得到产线信息
        public static string GetMfgLine(string user, string password)
        {
            string sql = "";
            sql += "Select distinct MfgLineName as Value,Description as Name from A_MfgLine order by MfgLineName";
            return GetQueryXML(sql, user, password);
        }

        //获取最后返工原因
        public static string GetLastReworkReason_Sql(string container, string user, string password)
        {
            string sql =
                "select hm.ContainerName,rr.ReworkReasonName ReworkReasonName,hm.LastMoveDate from HistoryMainline hm " +
                "inner join MoveHistory mh on hm.HistoryMainlineId=mh.HistoryMainlineId " +
                "inner join ReworkReason rr on rr.ReworkReasonId=mh.ReworkReasonId " +
                "where hm.ContainerName='" + container + "'" + " order by LastMoveDate desc";
            return GetQueryXML(sql, user, password);
        }

        //查询工站产品堆积的Sql。ws:工站编号。finishedProdType:成品的产品类型名
        public static string QueryWSContainerSql(string ws, string finishedProdType)
        {
            string sql = "select C.ContainerName " + Environment.NewLine +
                "from " + Environment.NewLine +
                "Container C " + Environment.NewLine +
                "inner join CurrentStatus CS on C.CurrentStatusId=CS.CurrentStatusId " + Environment.NewLine +
                "inner join Spec Sp on Sp.SpecId=CS.SpecId " + Environment.NewLine +
                "inner join ResourceGroupEntries RGE on RGE.ResourceGroupId=Sp.ResourceGroupId " + Environment.NewLine +
                "inner join ResourceDef RD on RD.ResourceId=RGE.EntriesId and RD.MfgLineId=C.MfglineId " + Environment.NewLine +
                "inner join CDODefinition CDef on CDef.CDODefId=RD.CDOTypeId and " + Environment.NewLine +
                "CDef.CDOName='BYD_Workstation' " + Environment.NewLine +
                "inner join Product P on C.ProductID=P.ProductID " + Environment.NewLine +
                "inner join ProductType PT on PT.ProductTypeID=P.ProductTypeID " + Environment.NewLine +
                "where PT.ProductTypeName='" + finishedProdType + "' and RD.ResourceName='" + ws +
                "'";
            return sql;
        }



        //获取停线工单
        public static string QueryMfgOrderStop(string stopName = "暂停")
        {
            string sql = "select mo.mfgordername,pt.producttypename,t.byd_stoplinereasonname stopreason" + Environment.NewLine +
                " from mfgorder mo" + Environment.NewLine +
                " inner join orderstatus os on mo.orderstatusid=os.orderstatusid" + Environment.NewLine +
                " inner join product p on p.productbaseid=mo.productbaseid" + Environment.NewLine +
                " inner join producttype pt on pt.producttypeid=p.producttypeid" + Environment.NewLine +
                " left join(select * from(" + Environment.NewLine +
                " select * from byd_mfgorderhistory moh" + Environment.NewLine +
                " inner join historymainline hm on moh.historymainlineid=hm.historymainlineid" + Environment.NewLine +
                " inner join byd_stoplinereason slr on slr.byd_stoplinereasonid=moh.byd_stoplinereasonid" + Environment.NewLine +
                " order by hm.txndate desc )" + Environment.NewLine +
                " where rownum=1" + Environment.NewLine +
                " ) t on mo.mfgorderid=t.mfgorderid" + Environment.NewLine +
                " where os.orderstatusname='" + stopName + "'";

            return sql;
        }

        //查询工位历史的sql语句
        public static string QueryWsHistorySql(string ws, DateTime startTime, DateTime endTime)
        {
            string sql = "select HM.TxnTypeName,HM.TxnDate,HM.ContainerName from HistoryMainline HM " +
                "inner join ResourceDef RD on RD.ResourceId=HM.ResourceId where RD.ResourceName='" + ws + "'" +
                "and HM.Txndate > to_date('" + startTime.ToString() + "','yyyy-mm-dd hh24:mi:ss')" +
                "and HM.Txndate < to_date('" + endTime.ToString() + "','yyyy-mm-dd hh24:mi:ss')" +
                "order by HM.TxnDate";
            sql = sql.Replace(">", "&gt;");
            sql = sql.Replace("<", "&lt;");
            return sql;
        }



        //获取资源类型
        public static string GetResourceType(string name, csConnection conn, ref XMLResponse res)
        {
            if (Common.IsStringNull(name))
            {
                return null;
            }
            string sql = "select cdo.cdoname from resourcedef res " +
                "inner join cdodefinition cdo on cdo.cdodefid=res.cdotypeid " +
                "where res.resourcename='" + name + "'";
            string xml = GetQueryXML(sql, conn.user, conn.password);
            res = TxnCommon.GetResponse(xml, conn.GetServerConnection());
            string xmlRet = res.lastResponse;
            TableData table = GetTableDataFromResponse(xmlRet);
            return table.datas[0][0];
        }

        //获取资源的吞吐量和最大吞吐量
        public static string GetResourceThruput(string resourceName)
        {
            string sql = "select ps.TotalThruputQty-ms.LastThruputQty as \"ThruputQty\",mr.Qty as \"MaxQty\" from ResourceDef rd" + Environment.NewLine
                + "inner join ProductionStatus ps on ps.ResourceId = rd.ResourceId" + Environment.NewLine
                + "inner join MaintenanceStatus ms on ms.ResourceId = rd.ResourceId" + Environment.NewLine
                + "inner join AssignedMaintReq amr on amr.AssignedMaintReqId = ms.AssignedMaintReqId" + Environment.NewLine
                + "inner join MaintenanceReq mr on mr.MaintenanceReqId = amr.MaintenanceReqId" + Environment.NewLine
                + "where rd.ResourceName = '" + resourceName + "' and ms.LastThruputQty is not null" + Environment.NewLine;

            return sql;
        }

        //根据Spec名称获取一个Container
        public static string GetAContainerBySpec(string specName)
        {
            string sql = "select c.containername from container c" + Environment.NewLine
                + "inner join currentstatus cs on c.currentstatusid=cs.currentstatusid" + Environment.NewLine
                + "inner join spec s on cs.specid=s.specid" + Environment.NewLine
                + "inner join specbase sb on s.specbaseid=sb.specbaseid" + Environment.NewLine
                + "where sb.specname='" + specName + "'" + Environment.NewLine
                + "and rownum=1" + Environment.NewLine;
            return sql;
        }

        //获取默认资源组
        public static string GetDefaultResourceGroup(string resource)
        {
            string sql = "select rg.resourcegroupname from resourcegroup rg" + Environment.NewLine
                + "inner join resourcegroupentries rge on rge.resourcegroupid=rg.resourcegroupid" + Environment.NewLine
                + "inner join resourcedef rd on rd.resourceid=rge.entriesid" + Environment.NewLine
                + "where rd.resourcename='" + resource + "'" + Environment.NewLine;
            return sql;
        }

        //获取默认Spec
        public static string GetDefaultSpec(string resGroup)
        {
            string sql = "select sb.specname from spec s" + Environment.NewLine
                + "inner join specbase sb on s.specbaseid=sb.specbaseid" + Environment.NewLine
                + "inner join resourcegroup rg on rg.resourcegroupid=s.resourcegroupid" + Environment.NewLine
                + "where rg.resourcegroupname='" + resGroup + "'" + Environment.NewLine;
            return sql;
        }
    }
    //装箱类--旧版本
    public class Package
    {
        public const string template = "<?xml version=\"1.0\" encoding=\"utf-16\"?><__InSite __version=\"1.1\" __encryption=\"2\">" +
            "<__session><__connect><user><__name></__name></user><password __encrypted=\"no\"></password></__connect></__session>" +
            "<__service __serviceType=\"BYD_PackageTxn\"><__utcOffset><![CDATA[08:00:00]]></__utcOffset><__inputData>" +
            "<ContainerName></ContainerName><Capacity></Capacity><ChildContainer><__name></__name></ChildContainer>" +
            "</__inputData><__requestData><CompletionMsg/><PackagedCount/><CartonDescription/><ContainerLevel/><Capacity/>" +
            "</__requestData></__service></__InSite>";


        public string container;//箱子条码
        public string childContainer;//产品条码
        public int capacity = 0;//最大装箱数量
        public int packagedCount;//已装箱数量
        public string cartonDescription;//箱子描述
        public string containerLevel;//箱子类型

        csConnection conn;
        public Package(csConnection connIn)
        {
            this.conn = connIn;
        }


        /// <summary>
        /// 执行装箱请求
        /// </summary>
        /// <param name="res">返回的XMLResponse</param>
        /// <param name="XExecute">是否执行事务，如果false,只查询数据</param>
        public void PackageRequest(ref XMLResponse res, bool XExecute = true)
        {
            CheckFileds();
            string xml = template;
            xml = TxnCommon.SetAccount(xml, conn.user);//设置帐号
            xml = TxnCommon.SetPassword(xml, conn.password);//设置密码
            xml = XMLOps.InsertValue(xml, "ContainerName", container);//设置箱号
            xml = XMLOps.InsertValue(xml, "Capacity", capacity.ToString());//设置最大装箱数
            xml = XMLOps.InsertValue(xml, new List<string>() { "ChildContainer", "name" }, childContainer);//设置产品条码
            if (XExecute == true) xml = TxnCommon.AddExecute(xml);
            res = TxnCommon.GetResponse(xml, conn.GetServerConnection());
            if (!res.Success())
            {
                return;
            }
            //获取查询参数
            string pkdCount = XMLOps.ReadAtNode(res.lastResponse, "PackagedCount");
            this.packagedCount = int.Parse(pkdCount);
            this.cartonDescription = XMLOps.ReadAtNode(res.lastResponse, "CartonDescription");
            this.containerLevel = XMLOps.ReadAtNode(res.lastResponse, "ContainerLevel");
            this.capacity = Convert.ToInt32(XMLOps.ReadAtNode(res.lastResponse, "Capacity"));
        }

        //检查是否所有必须字段都已经赋值
        public void CheckFileds()
        {
            if (Common.IsStringNull(container))
            {
                throw new Exception("未设置箱号");
            }
            if (Common.IsStringNull(childContainer))
            {
                throw new Exception("未设置产品条码");
            }
            if (capacity < 0)
            {
                throw new Exception("未设置最大装箱数");
            }
            if (Common.IsStringNull(conn.user))
            {
                throw new Exception("未设置用户帐号");
            }
            if (Common.IsStringNull(conn.password))
            {
                throw new Exception("未设置用户密码");
            }
            if (Common.IsStringNull(conn.host))
            {
                throw new Exception("未设置服务器");
            }
        }
    }


    /// <summary>
    /// 装箱类--新版本
    /// </summary>
    public class Pack
    {
        public struct ParentContainer
        {
            public string ContainerName;
            public string ConatinerStep;
            public string Capacity;
            public string ChildQty;
            public string ProductName;
            public string MfgOrder;
        }

        public ParentContainer ParentCon = new ParentContainer();

        public XMLResponse xres = Common.resCommon;






        //上料详情
        public struct ComponentSetupItem
        {
            public string fromContainer;
            public string fromFeeder;
            public string fromFeederSlot;
            public string issueControl;
            public string IssueDifferenceReason;
            public string substitutionReason;
            public ComponentSetupItem(string fromContainer, string fromFeeder, string fromFeederSlot,
                string issueControl, string IssueDifferenceReason, string substitutionReason)
            {
                this.fromContainer = fromContainer;
                this.fromFeeder = fromFeeder;
                this.fromFeederSlot = fromFeederSlot;
                this.issueControl = issueControl;
                this.IssueDifferenceReason = IssueDifferenceReason;
                this.substitutionReason = substitutionReason;
            }
            public string ToXmlString()
            {
                string xml = "<__listItem __listItemAction=\"add\"></__listItem>";
                string xmlDetails = "";
                if (!string.IsNullOrEmpty(fromContainer))
                {
                    xmlDetails += XMLAdditional.GetFieldNameXML("MaterialContainer", fromContainer);
                }
                if (!string.IsNullOrEmpty(fromFeeder))
                {
                    xmlDetails += XMLAdditional.GetFieldNameXML("FromFeeder", fromFeeder);
                }
                if (!string.IsNullOrEmpty(fromFeederSlot))
                {
                    xmlDetails += XMLAdditional.GetFieldNameXML("FromFeederSlot", fromFeederSlot);
                }
                if (!string.IsNullOrEmpty(issueControl))
                {
                    xmlDetails += XMLOps.GetNodeString("IssueControl", issueControl, false);
                }
                if (!string.IsNullOrEmpty(IssueDifferenceReason))
                {
                    xmlDetails += XMLAdditional.GetFieldNameXML("IssueDifferenceReason", IssueDifferenceReason);
                }
                if (!string.IsNullOrEmpty(substitutionReason))
                {
                    xmlDetails += XMLAdditional.GetFieldNameXML("SubstitutionReason", substitutionReason);
                }
                xml = XMLOps.InsertValue(xml, "listItem", xmlDetails);
                return xml;
            }

            //验证数据是否有效
            public bool Valid(ref XMLResponse res)
            {
                if (string.IsNullOrEmpty(fromContainer))
                {
                    res.SetErrorInfo("未设置物料批次!");
                    return false;
                }
                if (string.IsNullOrEmpty(fromFeeder))
                {
                    res.SetErrorInfo("未设置飞达号!");
                    return false;
                }
                if (string.IsNullOrEmpty(fromFeederSlot))
                {
                    res.SetErrorInfo("未设置槽号!");
                    return false;
                }
                if (string.IsNullOrEmpty(issueControl))
                {
                    res.SetErrorInfo("未设置扣料方式!");
                    return false;
                }
                return true;
            }
        }
    }

    #region "TrainingRecordMaint_01"
    public class TrainingRecordMaint_01
    {
        public string strIP, strUsername, strPassword, strOperateType, strName, strRequirement, strExpirationDate, strStatus;
        public int iPort;
        public XMLResponse res;
        #region 定义XML指令基础
        private string strTemplate = "<__InSite __version=\"1.1\">" +
                                    "	<__session>" +
                                    "		<__connect>" +
                                    "			<user>" +
                                    "				<__name></__name>" +
                                    "			</user>" +
                                    "			<password __encrypted=\"no\"></password>" +
                                    "		</__connect>" +
                                    "	</__session>" +
                                    "	<__service __serviceType=\"TrainingRecordMaint\">" +
                                    "		<__utcOffset>08:00</__utcOffset>" +
                                    "		<__inputData>" +
                                    "           <ParentDataObject><__name></__name ></ParentDataObject> " +
                                    "           <TrainingRequirement><__name></__name ><__useROR>true</__useROR></TrainingRequirement> " +
                                    "			<__perform>" +
                                    "				<__eventName></__eventName>" +
                                    "			</__perform>" +
                                    "			<ObjectChanges>" +
                                    "			<Status>" +
                                    "				<__name></__name>" +
                                    "			</Status>" +
                                    "			</ObjectChanges>" +
                                    "		</__inputData>" +
                                    "		<__requestData>" +
                                    "		</__requestData>" +
                                    "	</__service>" +
                                    "</__InSite>";

        private string strUpdTemplate = "<__InSite __version=\"1.1\">" +
                                    "	<__session>" +
                                    "		<__connect>" +
                                    "			<user>" +
                                    "				<__name></__name>" +
                                    "			</user>" +
                                    "			<password __encrypted=\"no\"></password>" +
                                    "		</__connect>" +
                                    "	</__session>" +
                                    "	<__service __serviceType=\"TrainingRecordMaint\">" +
                                    "		<__utcOffset>08:00</__utcOffset>" +
                                    "		<__inputData>" +
                                    "           <ParentDataObject><__name></__name></ParentDataObject> " +
                                    "           <TrainingRequirement><__name></__name><__useROR>true</__useROR></TrainingRequirement> " +
                                    "			<__perform>" +
                                    "				<__eventName></__eventName>" +
                                    "			</__perform>" +
                                    "			<ObjectChanges><ExpirationDate></ExpirationDate>" +
                                    "			<Status>" +
                                    "				<__name></__name>" +
                                    "			</Status>" +
                                    "			<TrainingRequirement>" +
                                    //"				<__name></__name><__useROR>true</__useROR>" +
                                    "			</TrainingRequirement>" +
                                    "			</ObjectChanges>" +
                                    "		</__inputData>" +
                                    "		<__requestData>" +
                                    "		</__requestData>" +
                                    "	</__service>" +
                                    "</__InSite>";
        #endregion
        #region 构造函数
        public TrainingRecordMaint_01(string strip, int iport, string strusername, string strpassword)
        {
            if (Common.IsStringNull(strip) || Common.IsStringNull(strusername) || Common.IsStringNull(strpassword) || Common.IsStringNull(iport.ToString()))
            {
                return;
            }
            strIP = strip;
            iPort = iport;
            strUsername = strusername;
            strPassword = strpassword;
        }
        #endregion
        #region 数据操作事务类
        /// <summary>
        /// DB数据操作事务
        /// </summary>
        /// <param name="ExecuteFalg">是否成功</param>
        /// <returns></returns>
        public RtnResult ExceuteData(bool ExecuteFalg = true)
        {
            #region 节点设置
            List<string> lstObjType = new List<string>();
            lstObjType.Add("perform");
            lstObjType.Add("eventName");

            List<string> lstName = new List<string>();
            lstName.Add("ParentDataObject");
            lstName.Add("name");

            List<string> lstRequirement = new List<string>();
            lstRequirement.Add("TrainingRequirement");
            lstRequirement.Add("name");

            List<string> lstExpirationDate = new List<string>();
            lstExpirationDate.Add("ObjectChanges");
            lstExpirationDate.Add("ExpirationDate");

            List<string> lstStatus = new List<string>();
            lstStatus.Add("Status");
            lstStatus.Add("name");

            #endregion
            #region 拼接XML脚本   
            string strXmlRequest = this.strTemplate;
            //填充账号信息
            strXmlRequest = TxnCommon.SetAccount(strXmlRequest, strUsername);
            strXmlRequest = TxnCommon.SetPassword(strXmlRequest, strPassword);

            if (!Common.IsStringNull(strOperateType))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstObjType, strOperateType);
            }
            if (!Common.IsStringNull(strName))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstName, strName);
            }
            if (!Common.IsStringNull(strRequirement))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstRequirement, strRequirement);
            }
            if (!Common.IsStringNull(strExpirationDate))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstExpirationDate, strExpirationDate);
            }
            if (!Common.IsStringNull(strStatus))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstStatus, strStatus);
            }

            if (ExecuteFalg)
            {
                strXmlRequest = TxnCommon.AddExecute(strXmlRequest);
            }
            #endregion
            #region DB操作事务
            ServerConnection serConnection = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strXmlRequest, serConnection);
            RtnResult result = new RtnResult();
            if (!res.Success())
            {
                result.strType = "ERROR";
                result.strMessage = res.errorDiscription;
            }
            else
            {
                result.strType = "Sucess";
                result.strMessage = "";
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExecuteFalg"></param>
        /// <returns></returns>
        public RtnResult ExceuteUPDData(bool ExecuteFalg = true)
        {
            #region 节点设置
            List<string> lstObjType = new List<string>();
            lstObjType.Add("perform");
            lstObjType.Add("eventName");

            List<string> lstName = new List<string>();
            lstName.Add("ParentDataObject");
            lstName.Add("name");

            List<string> lstRequirement = new List<string>();
            lstRequirement.Add("TrainingRequirement");
            lstRequirement.Add("name");

            List<string> lstExpirationDate = new List<string>();
            lstExpirationDate.Add("ObjectChanges");
            lstExpirationDate.Add("ExpirationDate");

            List<string> lstStatus = new List<string>();
            lstStatus.Add("Status");
            lstStatus.Add("name");

            List<string> lstObjRequirement = new List<string>();
            lstObjRequirement.Add("ObjectChanges");
            lstObjRequirement.Add("TrainingRequirement");
            #endregion
            #region 拼接xml脚本
            string strXmlRequest = this.strUpdTemplate;
            //填充账号信息
            strXmlRequest = TxnCommon.SetAccount(strXmlRequest, strUsername);
            strXmlRequest = TxnCommon.SetPassword(strXmlRequest, strPassword);

            if (!Common.IsStringNull(strOperateType))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstObjType, strOperateType);
            }
            if (!Common.IsStringNull(strName))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstName, strName);
            }
            if (!Common.IsStringNull(strRequirement))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstRequirement, strRequirement);
            }
            if (!Common.IsStringNull(strExpirationDate))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstExpirationDate, strExpirationDate);
            }
            if (!Common.IsStringNull(strStatus))
            {
                strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstStatus, strStatus);
            }
            string str = "<__name>" + strRequirement + "</__name><__useROR>true</__useROR>";
            strXmlRequest = XMLOps.InsertValue(strXmlRequest, lstObjRequirement, str);

            if (ExecuteFalg)
            {
                strXmlRequest = TxnCommon.AddExecute(strXmlRequest);
            }
            #endregion
            #region DB操作事务
            ServerConnection serConnection = new ServerConnection(strIP, iPort);
            res = TxnCommon.GetResponse(strXmlRequest, serConnection);
            RtnResult result = new RtnResult();
            if (!res.Success())
            {
                result.strType = "ERROR";
                result.strMessage = res.errorDiscription;
            }
            else
            {
                result.strType = "Sucess";
                result.strMessage = "";
            }
            return result;
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// 执行返回结果对象
    /// </summary>
    public class RtnResult
    {
        public string strType;
        public string strMessage;
    }

    #endregion




}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{

    /// <summary>
    /// XML操作类
    /// </summary>
    public static class XMLOps
    {

        /// <summary>
        /// 读取节点处的值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="node"></param>
        /// <param name="cdata"></param>
        /// <returns></returns>
        public static string ReadAtNode(string str, string node, bool cdata = true)
        {
            List<string> nodes = new List<string>();
            nodes.Add(node);
            string res = ReadAtNode(str, nodes, cdata);
            return res;
        }


        /// <summary>
        /// 读取节点链处的值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="nodes"></param>
        /// <param name="cdata"></param>
        /// <returns></returns>
        public static string ReadAtNode(string str, List<string> nodes, bool cdata = true)
        {
            string strRet = "";
            NodePos pos = FindNode(str, nodes);
            if (!pos.Valid())
            {
                return Common.notFoundMessage;
            }
            strRet = StrOps.GetPartString(str, pos.startRight + 1, pos.endLeft - 1);
            if (cdata)
            {
                string value = StrOps.GetBracPairValue(strRet);
                if (value != null)
                {
                    return value;
                }
            }
            return strRet;
        }


        /// <summary>
        /// 在指定节点集处(节点尾左括号前)插入节点值
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="node">在该节点后面插入内容</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public static string InsertValue(string str, List<string> nodes, string value)
        {
            if (Common.IsStringNull(str) || nodes == null || Common.IsStringNull(value)) return str;
            NodePos pos = FindNode(str, nodes);
            if (!pos.Valid())
            {
                return str;
            }
            return str.Insert(pos.endLeft, value);
        }

        /// <summary>
        /// 在指定节点处(节点尾左括号前)插入节点值
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="node">在该节点后面插入内容</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public static string InsertValue(string str, string node, string value)
        {
            List<string> nodes = new List<string>();
            nodes.Add(node);
            return InsertValue(str, nodes, value);
        }

        /// <summary>
        /// 在某个节点前插入数值
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="node">在该节点后面插入内容</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public static string InsertValueBeforeANode(string str, List<string> nodes, string value)
        {
            NodePos pos = FindNode(str, nodes);
            if (!pos.Valid())
            {
                return str;
            }
            string strRet = str.Insert(pos.startLeft, value);
            return strRet;
        }

        /// <summary>
        /// 在指定节点的某个子节点前插入值
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="node">在该节点后面插入内容</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public static string InsertValueBeforeANode(string str, string node, string value)
        {
            List<string> nodes = new List<string>();
            nodes.Add(node);
            return InsertValueBeforeANode(str, nodes, value);
        }

        /// <summary>
        /// 在指定节点集的某个子节点后插入值--未验证
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="node">在该节点后面插入内容</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public static string InsertValueAfterANode(string str, List<string> nodes, string value)
        {
            NodePos pos = FindNode(str, nodes);
            if (pos.endLeft < 0)
            {
                return str;
            }

            string strRet = str.Insert(pos.endRight + 1, value);
            return strRet;
        }


        /// <summary>
        /// 在指定节点的某个子节点后插入值
        /// </summary>
        /// <param name="str">原始文本</param>
        /// <param name="node">在该节点后面插入内容</param>
        /// <param name="value">要插入的值</param>
        /// <returns></returns>
        public static string InsertValueAfterANode(string str, string node, string value)
        {
            List<string> nodes = new List<string>();
            nodes.Add(node);
            return InsertValueAfterANode(str, nodes, value);
        }

        /// <summary>
        /// 在指定节点集的子节点个数(从1开始)处插入值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static string InsertValueAtIndex(string str, int index, string value, List<string> nodes)
        {
            NodePos pos = FindNodeAtIndex(str, index, nodes);
            if (!pos.Valid())
            {
                return str;
            }
            str = str.Insert(pos.startRight + 1, value);
            return str;
        }


        /// <summary>
        /// 查找节点链
        /// </summary>
        /// <param name="str"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static NodePos FindNode(string str, List<string> nodes)
        {
            NodePos posTemp = NodePos.notFound;
            string strRep = str;
            int startIndex = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (i == 0)
                {
                    posTemp = FindNode(strRep, nodes[i]);
                }
                else
                {
                    posTemp = FindNode_Strict(strRep, nodes[i]);
                }
                //如果找不到节点，则返回无效信息
                if (!posTemp.Valid())
                {
                    return NodePos.notFound;
                }
                //当到达节点链末尾时，不再需要更新操作
                if (i < nodes.Count - 1)
                {
                    startIndex += posTemp.startRight + 1;
                    strRep = StrOps.GetPartString(strRep, posTemp.startRight + 1, posTemp.endLeft - 1);
                }
            }
            return posTemp.Offset(startIndex);
        }


        /// <summary>
        /// 查找节点位置-未处理Complete节点的情况
        /// </summary>
        /// <param name="str"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodePos FindNode(string str, string node)
        {
            NodePos pos = NodePos.notFound;
            string strRep = str;
            int headNum = 0;
            int offset = 0;
            bool bFirstTime = true;
            while (true)
            {
                PairType type = PairType.Others;
                int left = -1;
                int right = -1;
                //获取首个名称匹配的字符位置
                int startTemp = StrOps.FindWordHeadInString(strRep, node);
                if (startTemp == -1)
                {
                    return NodePos.notFound;
                }
                int endTemp = startTemp + node.Length - 1;
                if (endTemp >= strRep.Length - 1)
                {
                    return NodePos.notFound;
                }
                //根据之前的字符判断是否是节点
                int beforeType = XMLAdditional.GetNodeBeforeMatchString(strRep, startTemp);
                switch (beforeType)
                {
                    case 0:
                        strRep = StrOps.GetPartString(strRep, startTemp + 1);
                        offset += startTemp + 1;
                        continue;
                    case 1:
                        type = PairType.Head;
                        break;
                    case 2:
                        type = PairType.End;
                        break;
                    case 3:
                        type = PairType.Others;
                        break;
                }
                for (int i = startTemp - 1; i >= 0; i--)
                {
                    if (strRep[i] == '<')
                    {
                        left = i;
                        break;
                    }
                }
                //根据之后的字符串判断是否节点名
                switch (strRep[endTemp + 1])
                {
                    case ' ':
                    case '>':
                        break;
                    case '/'://Complete类型
                        if (type == PairType.End || strRep[endTemp + 2] != '>')
                        {
                            return NodePos.notFound;
                        }
                        type = PairType.Complete;
                        break;
                    default://不合格(非节点名)的情况
                        strRep = StrOps.GetPartString(strRep, startTemp + 1);
                        offset += startTemp + 1;
                        continue;
                }
                bool bRef = false;
                for (int i = endTemp + 1; i < strRep.Length; i++)
                {
                    if (strRep[i] == '"')//排除引号内的'>'符号
                    {
                        bRef = Common.ReverseBool(bRef);
                    }
                    if (strRep[i] == '>' && !bRef)
                    {
                        right = i;
                        break;
                    }
                }
                //完整节点的情况
                if (StrOps.GetNonSpaceBefore(strRep, right) == '/')
                {
                    if (bFirstTime)
                    {
                        return NodePos.GetCompletePos(left + offset, right + offset);
                    }
                    strRep = StrOps.GetPartString(strRep, right + 1);
                    offset += right + 1;
                    continue;
                }
                switch (type)
                {
                    case PairType.Head:
                        headNum++;
                        if (headNum == 1)
                        {
                            pos.startLeft = left + offset;
                            pos.startRight = right + offset;
                        }
                        break;
                    case PairType.End:
                        headNum--;
                        if (headNum < 0)
                        {
                            return NodePos.notFound;
                        }
                        else if (headNum == 0)
                        {
                            pos.endLeft = left + offset;
                            pos.endRight = right + offset;
                            return pos;
                        }
                        break;
                }
                bFirstTime = false;
                strRep = StrOps.GetPartString(strRep, startTemp + 1);
                offset += startTemp + 1;
            }
        }

        /// <summary>
        /// 查找节点位置-未处理Complete节点的情况
        /// </summary>
        /// <param name="str"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodePos FindNode_Strict(string str, string node)
        {
            NodePos pos = NodePos.notFound;
            string strRep = str;
            int headNum = 0;
            int offset = 0;
            bool bFirstTime = true;
            while (true)
            {
                PairType type = PairType.Others;
                int left = -1;
                int right = -1;
                //获取首个名称匹配的字符位置
                int startTemp = StrOps.FindWordHeadInString_Strict(strRep, node);
                if (startTemp == -1)
                {
                    return NodePos.notFound;
                }
                int endTemp = startTemp + node.Length - 1;
                if (endTemp >= strRep.Length - 1)
                {
                    return NodePos.notFound;
                }
                //根据之前的字符判断是否是节点
                int beforeType = XMLAdditional.GetNodeBeforeMatchString(strRep, startTemp);
                switch (beforeType)
                {
                    case 0:
                        strRep = StrOps.GetPartString(strRep, startTemp + 1);
                        offset += startTemp + 1;
                        continue;
                    case 1:
                        type = PairType.Head;
                        break;
                    case 2:
                        type = PairType.End;
                        break;
                    case 3:
                        type = PairType.Others;
                        break;
                }
                for (int i = startTemp - 1; i >= 0; i--)
                {
                    if (strRep[i] == '<')
                    {
                        left = i;
                        break;
                    }
                }
                //根据之后的字符串判断是否节点名
                switch (strRep[endTemp + 1])
                {
                    case ' ':
                    case '>':
                        break;
                    case '/'://Complete类型
                        if (type == PairType.End || strRep[endTemp + 2] != '>')
                        {
                            return NodePos.notFound;
                        }
                        type = PairType.Complete;
                        break;
                    default://不合格(非节点名)的情况
                        strRep = StrOps.GetPartString(strRep, startTemp + 1);
                        offset += startTemp + 1;
                        continue;
                }
                bool bRef = false;
                for (int i = endTemp + 1; i < strRep.Length; i++)
                {
                    if (strRep[i] == '"')//排除引号内的'>'符号
                    {
                        bRef = Common.ReverseBool(bRef);
                    }
                    if (strRep[i] == '>' && !bRef)
                    {
                        right = i;
                        break;
                    }
                }
                //完整节点的情况
                if (StrOps.GetNonSpaceBefore(strRep, right) == '/')
                {
                    if (bFirstTime)
                    {
                        return NodePos.GetCompletePos(left + offset, right + offset);
                    }
                    strRep = StrOps.GetPartString(strRep, right + 1);
                    offset += right + 1;
                    continue;
                }
                switch (type)
                {
                    case PairType.Head:
                        headNum++;
                        if (headNum == 1)
                        {
                            pos.startLeft = left + offset;
                            pos.startRight = right + offset;
                        }
                        break;
                    case PairType.End:
                        headNum--;
                        if (headNum < 0)
                        {
                            return NodePos.notFound;
                        }
                        else if (headNum == 0)
                        {
                            pos.endLeft = left + offset;
                            pos.endRight = right + offset;
                            return pos;
                        }
                        break;
                }
                bFirstTime = false;
                strRep = StrOps.GetPartString(strRep, startTemp + 1);
                offset += startTemp + 1;
            }
        }

        /// <summary>
        /// 获取字符串的首括号对
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static BracPair GetFirstBracPair(string str)
        {
            bool bRefStart = false;//判断是否进入了双引号内
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '<')
                {
                    int leftNum = 1;
                    for (int j = i + 1; j < str.Length; j++)
                    {
                        if (str[i] == '"')
                        {
                            if (!bRefStart)
                            {
                                bRefStart = true;
                            }
                            else
                            {
                                bRefStart = false;
                            }
                        }
                        if (str[j] == '<' && !bRefStart)
                        {
                            leftNum++;
                        }
                        else if (str[j] == '>' && !bRefStart)
                        {
                            leftNum--;
                            if (leftNum == 0)
                            {
                                string strTemp = StrOps.RemoveHeadEndSpace(StrOps.GetPartString(str, i + 1, j - 1));
                                PairType type = GetPairType(strTemp);
                                string name = "";
                                bool bUnderline = false;
                                List<HeadParam> prms = new List<HeadParam>();

                                if (type == PairType.Head || type == PairType.End || type == PairType.Complete || type == PairType.Others)
                                {
                                    if (strTemp[1] == '_')
                                    {
                                        bUnderline = true;
                                    }
                                    StrIndexPair pair = StrOps.GetFirstWord(strTemp);
                                    name = pair.name;
                                    string strTempForPrms = StrOps.GetPartString(strTemp, pair.index + 1);
                                    prms = StrOps.GetHeadParams(strTempForPrms);
                                }
                                else if (type == PairType.Value)
                                {
                                    name = StrOps.GetBracPairValue(strTemp);
                                }
                                else
                                {
                                    name = "";
                                }
                                return new BracPair(i, j, name, type, bUnderline, prms);
                            }
                        }
                    }
                }
            }
            return BracPair.GetInvalid();
        }

        /// <summary>
        /// 由尖括号内的字符串，获取节点类型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        private static PairType GetPairType(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[str.Length - 1] == '/')
                {
                    return PairType.Complete;
                }
                else if (str[0] == '/')
                {
                    return PairType.End;
                }
                else if (str[0] == '!')
                {
                    return PairType.Value;
                }
                else if (str[0] == '_' || StrOps.IsAlphabet(str[0]) || StrOps.IsNumber(str[0]))
                {
                    return PairType.Head;
                }
                else//首字母是'?'等等
                {
                    return PairType.Others;
                }
            }
            return PairType.Head;
        }

        /// <summary>
        /// 将节点数值转化为CDATA字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCDATAValue(string value)
        {
            if (value == "")
            {
                return "";
            }
            string strRet = "<![CDATA[" + value + "]]>";
            return strRet;
        }

        /// <summary>
        /// 获取节点字符串。无CDATA
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="bUnderLine"></param>
        /// <returns></returns>
        public static string GetNodeString_NoCDATA(string name, string value, bool bUnderLine)
        {
            string strRet = "";
            strRet += "<";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += name + ">" + value + "</";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += name + ">";
            return strRet;
        }

        /// <summary>
        /// 获取节点字符串。有CDATA
        /// </summary>
        /// <param name="name">节点名</param>
        /// <param name="value">节点值</param>
        /// <param name="bUnderLine">是否有下划线</param>
        /// <returns></returns>
        public static string GetNodeString(string name, string value, bool bUnderLine)
        {
            string strRet = "";
            strRet += "<";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += name + ">" + XMLOps.GetCDATAValue(value) + "</";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += name + ">";
            return strRet;
        }


        /// <summary>
        /// 获取整节点的字符串
        /// </summary>
        /// <param name="name">获取内容的节点</param>
        /// <param name="bUnderLine">节点名称是否带有双下划线</param>
        /// <returns></returns>
        public static string GetCompleteNode(string name, bool bUnderLine)
        {
            string strRet = "<";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += name + "/>";
            return strRet;
        }

        /// <summary>
        /// 修改节点头参数值
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="node"></param>
        /// <param name="prmName"></param>
        /// <param name="valNew"></param>
        /// <returns></returns>
        public static string EditHeadNodeParam(string xml, string node, string prmName, string valNew)
        {
            string strRet = xml;
            NodePos pos = FindNode(xml, node);
            if (!pos.Valid())
            {
                return xml;
            }
            string strTemp = StrOps.GetPartString(xml, pos.startLeft);
            BracPair pair = GetFirstBracPair(strTemp);
            for (int i = 0; i < pair.prms.Count; i++)
            {
                HeadParam prm = pair.prms[i];
                if (prm.name == prmName)
                {
                    pair.prms[i] = new HeadParam(prm.bUnderline, prm.name, valNew);
                }
            }
            strRet = StrOps.RemoveBetween(strRet, pos.startLeft, pos.startRight);
            strRet = strRet.Insert(pos.startLeft, pair.ToString());
            return strRet;
        }


        /// <summary>
        /// 获取节点的内容
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static string GetNodeContent(string xml, List<string> nodes)
        {
            NodePos pos = FindNode(xml, nodes);
            if (!pos.Valid())
            {
                return Common.notFoundMessage;
            }
            return StrOps.GetPartString(xml, pos.startRight + 1, pos.endLeft - 1);
        }

        /// <summary>
        /// 获取节点的内容
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetNodeContent(string xml, string node)
        {
            return GetNodeContent(xml, new List<string>() { node });
        }

        /// <summary>
        /// 查找同级节点下的第n个同名节点
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="index"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static NodePos FindNodeAtIndex(string xml, int index, List<string> nodes)
        {
            NodePos pos = NodePos.notFound;
            int offset = 0;
            List<string> nodeList = new List<string>();
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                nodeList.Add(nodes[i]);
            }
            string content = xml;
            if (nodeList.Count > 0)
            {
                NodePos posTemp = FindNode(xml, nodeList);
                offset += posTemp.startRight + 1;
                content = GetNodeContent(xml, nodeList);
            }
            if (Common.IsStringNull(content))
            {
                return pos;
            }
            string endNode = nodes[nodes.Count - 1];

            for (int i = 1; i <= index; i++)
            {
                pos = FindNode(content, endNode);
                if (!pos.Valid())
                {
                    return pos;
                }
                if (i == index)
                {
                    return pos.Offset(offset);
                }
                offset += pos.endRight + 1;
                content = StrOps.GetPartString(content, pos.endRight + 1);
            }
            return pos;
        }

        /// <summary>
        /// 查找同级节点下的第n个同名节点
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="index"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodePos FindNodeAtIndex(string xml, int index, string node)
        {
            return FindNodeAtIndex(xml, index, new List<string>() { node });
        }

    }

    /// <summary>
    /// 附加的XML操作类
    /// </summary>
    public static class XMLAdditional
    {
        /// <summary>
        /// 根据匹配字符前面的字符串，判断是否节点，及节点类型。返回解析：0=非节点，1=头节点，2=尾节点，3=其他节点
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static int GetNodeBeforeMatchString(string str, int pos)
        {
            if (pos <= 0)
            {
                return 0;
            }
            string strRep = StrOps.GetPartString(str, 0, pos - 1);
            int end = strRep.Length - 1;
            if (strRep[end] == '_')
            {
                if (strRep[end - 1] != '_')
                {
                    return 0;
                }
                end -= 2;
            }
            if (strRep[end] == '/')
            {
                if (strRep[end - 1] != '<')
                {
                    return 0;
                }
                return 2;
            }
            else if (strRep[end] == '?')
            {
                if (strRep[end - 1] != '<')
                {
                    return 0;
                }
                return 3;
            }
            if (strRep[end] == '<')
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 根据单词的字符首位位置,判断是否节点名
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool CheckNodeName(string str, int start, int end)
        {
            for (int i = start - 1; i >= 0; i--)
            {
                if (str[i] == '<')
                {
                    break;
                }
                if (str[i] != '/' && str[i] != '_' && str[i] != '?')
                {
                    return false;
                }
            }
            if (str[end + 1] != ' ' && str[end + 1] != '/' && str[end + 1] != '>' && str[end + 1] != '?')
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取含字段名的XML字符，<fieldName><__name><![CDATA[value]]></__name></fieldName>
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="bUnder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetFieldNameXML(string fieldName, bool bUnder, string value)
        {
            string xml = XMLOps.GetNodeString(fieldName, "", bUnder);
            string temp = XMLOps.GetNodeString("name", value, true);
            xml = XMLOps.InsertValue(xml, fieldName, temp);
            return xml;
        }

        /// <summary>
        /// 获取含字段名的XML字符，<fieldName><__name><![CDATA[value]]></__name></fieldName>
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetFieldNameXML(string fieldName, string value)
        {
            return GetFieldNameXML(fieldName, false, value);
        }
    }

    /// <summary>
    /// 括号对
    /// </summary>
    public class BracPair
    {
        public int left;
        public int right;
        public string name;
        public PairType type;
        public bool bUnderLine;
        public List<HeadParam> prms = new List<HeadParam>();

        public static BracPair GetInvalid()
        {
            return new BracPair(-1, -1, "", PairType.Others, false);
        }
        public BracPair(int leftIn, int rightIn, string nameIn, PairType typeIn, bool underIn)
        {
            this.left = leftIn;
            this.right = rightIn;
            this.name = nameIn;
            this.type = typeIn;
            this.bUnderLine = underIn;
        }
        public BracPair(int leftIn, int rightIn, string nameIn, PairType typeIn, bool underIn, List<HeadParam> prmsIn)
        {
            this.left = leftIn;
            this.right = rightIn;
            this.name = nameIn;
            this.type = typeIn;
            this.bUnderLine = underIn;
            this.prms = Common.CopyList<HeadParam>(prmsIn);
        }

        //判断对象是否有效
        public bool Valid()
        {
            if (left < 0 || right < 0)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {/*
            if(type==PairType.Others)
            {
                return "OtherType";
            }*/
            if (type == PairType.Value)
            {
                return XMLOps.GetCDATAValue(name);
            }
            string strRet = "";
            strRet += "<";
            if (type == PairType.Others)
            {
                strRet += "?";
            }
            else if (type == PairType.End)
            {
                strRet += "/";
            }
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += name;
            if (type == PairType.Complete)
            {
                strRet += "/";
            }
            for (int i = 0; i < prms.Count; i++)
            {
                strRet += prms[i].ToString();
            }
            strRet += ">";
            return strRet;
        }
    }

    /// <summary>
    /// 括号对类型。Head:节点头,End:节点尾,Value:值,Complete:完整（简单）节点,Others:其他类型
    /// </summary>
    public enum PairType { Head, End, Value, Complete, Others };

    /// <summary>
    /// 节点类型。Partial:节点头和节点尾分开;Complete:只有一个完整括号对;Others:其他类型
    /// </summary>
    public enum NodeType { Partial, Complete, Others };

    /// <summary>
    /// 节点头参数结构
    /// </summary>
    public struct HeadParam
    {
        public bool bUnderline;
        public string name;
        public string value;
        public HeadParam(bool underIn, string nameIn, string valueIn)
        {
            this.bUnderline = underIn;
            this.name = nameIn;
            this.value = valueIn;
        }
        public override string ToString()
        {
            string strRet = " ";
            if (bUnderline)
            {
                strRet += "__";
            }
            strRet += name + "=" + "\"" + value + "\"";
            return strRet;
        }
    }

    /// <summary>
    /// XML节点结构体
    /// </summary>
    public class XMLNode
    {
        public string nodeName = "";//节点名称
        public List<HeadParam> prms = new List<HeadParam>();
        public bool bUnderLine = false;//是否有2个下划线
        public NodePos pos = NodePos.notFound;
        public NodeType type = NodeType.Partial;

        public static XMLNode Invalid = new XMLNode("", false, NodeType.Others, NodePos.notFound);

        public XMLNode()
        {

        }
        public XMLNode(string nameIn, bool bUnderLineIn, NodeType typeIn, NodePos posIn)
        {
            this.nodeName = nameIn;
            this.bUnderLine = bUnderLineIn;
            this.pos = posIn;
            this.type = typeIn;
        }
        public XMLNode(string nameIn, bool underIn, NodeType typeIn, NodePos posIn, List<HeadParam> prmsIn)
        {
            this.nodeName = nameIn;
            this.bUnderLine = underIn;
            this.type = typeIn;
            this.pos = posIn;
            this.prms = Common.CopyList<HeadParam>(prmsIn);
        }
        /// <summary>
        /// 把节点头，节点值，节点尾转化成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string strRet = "";
            if (type == NodeType.Partial)
            {
                strRet += ToStringHead();
                strRet += ToStringEnd();
            }
            else if (type == NodeType.Complete)
            {
                strRet = ToStringComplete();
            }
            else
            {
                strRet = "Other type";
            }
            return strRet;
        }

        /// <summary>
        /// 把节点头转换成字符串
        /// </summary>
        /// <returns></returns>
        public string ToStringHead()
        {
            string strRet = "";
            strRet += "<";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += nodeName;
            //节点头参数列表
            for (int i = 0; i < prms.Count; i++)
            {
                strRet += prms[i].ToString();
            }
            strRet += ">";
            return strRet;
        }
        /// <summary>
        /// 把节点尾转换成字符串
        /// </summary>
        /// <returns></returns>
        public string ToStringEnd()
        {
            string strRet = "";
            strRet += "</";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += nodeName + ">";
            return strRet;
        }
        /// <summary>
        /// 完成节点的字符转换
        /// </summary>
        /// <returns></returns>
        public string ToStringComplete()
        {
            if (type != NodeType.Complete)
            {
                return "";
            }
            string strRet = "";
            strRet += "<";
            if (bUnderLine)
            {
                strRet += "__";
            }
            strRet += nodeName;
            for (int i = 0; i < prms.Count; i++)
            {
                strRet += prms[i].ToString();
            }
            strRet += "/>";
            return strRet;
        }

        //判断对象是否有效
        public bool Valid()
        {
            if (pos.startLeft < 0 || pos.startRight < 0)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 节点位置结构体
    /// </summary>
    public struct NodePos
    {
        public int startLeft;
        public int startRight;
        public int endLeft;
        public int endRight;

        public static NodePos notFound = new NodePos(-1, -1, -1, -1);

        public NodePos(int startLeftIn, int startRightIn, int endLeftIn, int endRightIn)
        {
            this.startLeft = startLeftIn;
            this.startRight = startRightIn;
            this.endLeft = endLeftIn;
            this.endRight = endRightIn;
        }
        public NodePos Offset(int offset)
        {
            return new NodePos(startLeft + offset, startRight + offset, endLeft + offset, endRight + offset);
        }

        public static NodePos GetCompletePos(int left, int right)
        {
            return new NodePos(left, right, left, right);
        }

        //判断对象是否有效
        public bool Valid()
        {
            if (startLeft < 0 || startRight < 0)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// XML命令请求返回数据结构
    /// </summary>
    public struct XMLResponse
    {
        public string lastRequest;//请求XML
        public string lastResponse;//回应XML
        public exceptType exception;
        public bool result;
        public string errorDiscription;
        public string successMessage;

        /// <summary>
        /// 返回的XML结构体
        /// </summary>
        /// <param name="lreqIn"></param>
        /// <param name="lresIn"></param>
        /// <param name="typeIn"></param>
        /// <param name="resIn"></param>
        /// <param name="errIn"></param>
        /// <param name="successIn"></param>
        public XMLResponse(string lreqIn, string lresIn, exceptType typeIn, bool resIn, string errIn, string successIn)
        {
            this.lastRequest = lreqIn;
            this.lastResponse = lresIn;
            this.exception = typeIn;
            this.result = resIn;
            this.errorDiscription = errIn;
            this.successMessage = successIn;
        }


        /// <summary>
        /// 判断是否有异常
        /// </summary>
        /// <returns></returns>
        private bool Exception()
        {
            if (exception == exceptType.normal)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        ///获取异常信息
        /// </summary>
        /// <returns></returns>
        private string GetExceptInfor()
        {
            return Common.GetExceptionInfo(exception);
        }


        /// <summary>
        /// 判断请求是否执行成功
        /// </summary>
        /// <returns></returns>
        public bool Success()
        {
            if (Exception() || !result)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取失败信息
        /// </summary>
        /// <returns></returns>
        public string GetErrorInfo()
        {
            if (Exception())
            {
                return GetExceptInfor();
            }
            if (!result)
            {
                return errorDiscription;
            }
            return "";
        }

        /// <summary>
        /// 重置对象
        /// </summary>
        public void Reset()
        {
            this.lastRequest = "";
            this.lastResponse = "";
            this.exception = exceptType.normal;
            this.result = true;
            this.errorDiscription = "";
            this.successMessage = "";
        }

        /// <summary>
        /// 把结果置为false，并设置错误描述
        /// </summary>
        /// <param name="errorDesc">错误描述</param>
        public void SetErrorInfo(string errorDesc)
        {
            this.result = false;
            this.errorDiscription = errorDesc;
        }
    }
}

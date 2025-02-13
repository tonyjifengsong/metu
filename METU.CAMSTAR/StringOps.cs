using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CAMSTAR
{
    /// <summary>
    /// 字符操作类
    /// </summary>
    internal static class StrOps
    {
        //
        /// <summary>
        /// 带双引号包围的字符串
        /// </summary>
        /// <param name="str">Need deal string</param>
        /// <returns></returns>
        public static string AddRefSymbols(string str)
        {
            return "\"" + str + "\"";
        }

        /// <summary>
        /// 在指定位置（该索引号之前）插入指定字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="index">位置</param>
        /// <param name="insert">要插入的字符</param>
        /// <returns></returns>
        public static string InsertAt(string str, int index, string insert)
        {
            return str.Insert(index, insert);
        }

        /// <summary>
        /// 从字符串中查找指定字符串（首次找到的），获取其字首索引号
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="word">查找的字符串</param>
        /// <returns></returns>
        public static int FindWordHeadInString(string str, string word)
        {
            int length = word.Length;
            for (int i = 0; i < str.Length - 1; i++)
            {
                for (int j = 0; j < word.Length; j++)
                {
                    if (str[i + j] != word[j])
                    {
                        break;
                    }
                    if (j == word.Length - 1)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 从字符串中查找指定字符串（首次找到的），获取其字首索引号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int FindWordHeadInString_Strict(string str, string word)
        {
            int length = word.Length;
            int leftCount = 0;
            bool bEnd = false;
            int leftMidBrac = 0;
            int refCount = 0;
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i] == '[')
                {

                    leftMidBrac++;
                    continue;
                }
                if (str[i] == ']')
                {
                    leftMidBrac--;
                    continue;
                }
                if (str[i] == '"')
                {

                    refCount++;
                    continue;
                }
                if (leftMidBrac != 0 || refCount % 2 != 0)
                {
                    continue;
                }
                if (str[i] == '<')
                {
                    char ch = GetNonSpaceAfter(str, i);
                    if (ch == '/')
                    {
                        bEnd = true;
                        leftCount--;
                        continue;
                    }
                    if (ch == '?' || ch == '!')
                    {
                        continue;
                    }
                    leftCount++;
                    bEnd = false;
                    continue;
                }
                else if (str[i] == '>')
                {
                    if (StrOps.GetNonSpaceBefore(str, i) == '/')
                    {
                        leftCount--;
                    }
                    continue;
                }
                if (!((leftCount == 1 && !bEnd) || (leftCount == -1 && bEnd)))
                {
                    continue;
                }
                for (int j = 0; j < word.Length; j++)
                {
                    if (str[i + j] != word[j])
                    {
                        break;
                    }
                    if (j == word.Length - 1)
                    {
                        if (XMLAdditional.CheckNodeName(str, i, i + word.Length - 1))
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 从字符串中查找指定字符串（首次找到的），获取其字尾索引号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int FindWordEndInString(string str, string word)
        {


            int length = word.Length;
            for (int i = 0; i < str.Length - length; i++)
            {
                string strTemp = GetPartString(str, i, i + length - 1);
                if (strTemp == word)
                {
                    return i + length - 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 移除部分字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns></returns>
        public static string RemoveBetween(string str, int start, int end)
        {
            string strRet = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (i < start || i > end)
                {
                    strRet += str[i];
                }
            }
            return strRet;
        }

        /// <summary>
        /// 移除字符串首尾的空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHeadEndSpace(string str)
        {
            string strHeadRemove = RemoveHeadSpace(str);
            string strAllRemove = RemoveEndSpace(strHeadRemove);
            return strAllRemove;
        }

        /// <summary>
        /// 移除字符串起始的空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHeadSpace(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ' ')
                {
                    if (i == 0)
                    {
                        return str;
                    }
                    return str.Remove(0, i);
                    /*
                    else
                    {
                        return GetPartString(str, i);
                    }*/
                }
            }
            return str;
        }

        /// <summary>
        /// 移除字符串末尾的空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveEndSpace(string str)
        {
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i] != ' ')
                {
                    if (i == str.Length - 1)
                    {
                        return str;
                    }
                    return str.Remove(i + 1);

                }
            }
            return str;
        }

        /// <summary>
        /// 截取从起始值的部分字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static string GetPartString(string str, int start)
        {
            return str.Remove(0, start);

        }

        /// <summary>
        /// 截取部分字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string GetPartString(string str, int start, int end)
        {
            if (start < 0)
            {
                start = 0;
            }
            if (end > str.Length - 1)
            {
                end = str.Length - 1;
            }
            if (start > end)
            {
                return "";
            }
            string ret = str;
            ret = ret.Remove(end + 1, str.Length - 1 - end);
            ret = ret.Remove(0, start);
            return ret;
        }

        public static StrIndexPair GetFirstWord(string str)
        {
            string strRet = "";
            int index = 0;
            bool bStart = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (IsAlphabet(str[i]) || IsNumber(str[i]))
                {
                    strRet += str[i];
                    if (!bStart)
                    {
                        bStart = true;
                    }
                }
                else
                {
                    if (bStart)
                    {
                        index = i - 1;
                        break;
                    }
                }
                if (i == str.Length - 1)
                {
                    index = str.Length - 1;
                }
            }
            return new StrIndexPair(strRet, index);
        }

        /// <summary>
        /// 获取值类型括号内的值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetBracPairValue(string str)
        {
            int end = FindWordEndInString(str, "CDATA");
            if (end < 0)
            {
                return Common.notFoundMessage;
            }
            string strRemain = GetPartString(str, end + 1);
            int left = -1;
            int right = -1;
            for (int i = 0; i < strRemain.Length; i++)
            {
                if (strRemain[i] == '[')
                {
                    left = i;
                    break;
                }
            }
            bool bRightBrac = false;
            int iRMidBrac = 0;
            for (int i = strRemain.Length - 1; i >= 0; i--)
            {
                if (strRemain[i] == '>')
                {
                    bRightBrac = true;
                }
                if (strRemain[i] == ']' && bRightBrac)
                {
                    iRMidBrac++;
                    if (iRMidBrac == 2)
                    {
                        right = i;
                        break;
                    }
                }
            }
            if (left >= right || left < 0 || right < 0)
            {
                throw new Exception("值括号内的格式不正确!");
            }
            string valueAll = GetPartString(strRemain, left + 1, right - 1);
            //return RemoveHeadEndSpace(valueAll);
            return valueAll;
        }

        /// <summary>
        /// 获取值类型括号内的值。如果没有CDATA返回完整字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetBracPairValue_Auto(string str)
        {
            string value = GetBracPairValue(str);
            if (Common.IsStringNull(value))
            {
                value = str;
            }
            return value;
        }

        /// <summary>
        /// 判断一个字符是否英文字母
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsAlphabet(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
            {
                return true;
            }
            if (ch >= 'A' && ch <= 'Z')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断一个字符是否数字
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsNumber(char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 获取节点头参数列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<HeadParam> GetHeadParams(string str)
        {
            List<HeadParam> prms = new List<HeadParam>();
            string strRep = str;
            while (true)
            {
                strRep = RemoveHeadEndSpace(strRep);
                bool bUnder = false;
                string name = "";
                string value = "";
                if (strRep.Length < 1 || strRep[0] == '/')
                {
                    return prms;
                }
                int wordStart = 0;
                if (strRep[0] == '_')
                {
                    bUnder = true;
                    wordStart = 2;
                }
                bool bWordStart = false;
                int endIndex = -1;
                for (int i = wordStart; i < strRep.Length; i++)
                {
                    if (strRep[i] == ' ')
                    {
                        continue;
                    }
                    else if (strRep[i] == '=')
                    {
                        bWordStart = false;
                        endIndex = i;
                        break;
                    }
                    else
                    {
                        name += strRep[i];
                        bWordStart = true;
                    }
                }
                if (endIndex < 0)
                {
                    return prms;
                }
                //获取值
                for (int i = endIndex + 1; i < strRep.Length; i++)
                {
                    if (strRep[i] == '"')
                    {
                        if (!bWordStart)
                        {
                            bWordStart = true;
                        }
                        else
                        {
                            bWordStart = false;
                            endIndex = i;
                            break;
                        }
                    }
                    else if (bWordStart)
                    {
                        value += strRep[i];
                    }
                }
                prms.Add(new HeadParam(bUnder, name, value));
                strRep = GetPartString(strRep, endIndex + 1);
            }
        }


        /// <summary>
        /// 获取从指定索引之后的非空格的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static char GetNonSpaceAfter(string str, int start)
        {
            for (int i = start + 1; i < str.Length; i++)
            {
                if (str[i] != ' ')
                {
                    return str[i];
                }
            }
            return ' ';
        }

        /// <summary>
        /// 获取从指定索引之前的非空格的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static char GetNonSpaceBefore(string str, int start)
        {
            for (int i = start - 1; i >= 0; i--)
            {
                if (str[i] != ' ')
                {
                    return str[i];
                }
            }
            return ' ';
        }
    }
}

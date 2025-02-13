using System;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// 数进制转换
    /// </summary>
    public class BarcodeStringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public string Characters
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Length
        {
            get
            {
                if (Characters != null)
                    return Characters.Length;
                else
                    return 0;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public BarcodeStringHelper()
        {
            Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="characters"></param>
        public BarcodeStringHelper(string characters)
        {
            Characters = characters;
        }

        /// <summary>
        /// 数字转换为指定的进制形式字符串
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string ToString(long number)
        {
            List<string> result = new List<string>();
            long t = number;

            while (t > ConstNum.Zero)
            {
                var mod = t % Length;
                t = Math.Abs(t / Length);
                var character = Characters[Convert.ToInt32(mod)].ToString();
                result.Insert(0, character);
            }

            return string.Join("", result.ToArray());
        }
        /// <summary>
        /// 数字转换为指定长度的进制形式字符串
        /// </summary>
        /// <param name="number"></param>
        /// <param name="lg"></param>
        /// <returns></returns>
        public string ConvertToString(long number, int lg = 4)
        {
            string result = ToString(number);
            string str = "";
            if (result.Length < lg)
            {
                for (int i = 0; i < lg - result.Length; i++)
                {
                    str += "0";
                }
            }
            else
            {
                str = str.Substring(result.Length - lg > ConstNum.Zero ? result.Length - lg : 0, lg);
                return str;
            }
            str += result;
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="lg"></param>
        /// <param name="NL"></param>
        /// <returns></returns>
        public string ConvertToString(long number, int lg = 4, int NL = 10)
        {
            Characters = Characters.Substring(0, NL);
            string result = ToString(number);
            string str = "";
            if (result.Length < lg)
            {
                for (int i = 0; i < lg - result.Length; i++)
                {
                    str += "0";
                }
            }
            else
            {
                str = str.Substring(result.Length - lg > ConstNum.Zero ? result.Length - lg : 0, lg);
                return str;
            }
            str += result;
            return str;
        }
        /// <summary>
        /// 指定字符串转换为指定进制的数字形式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public long FromString(string str)
        {
            str = str.ToUpper();
            long result = 0;
            int j = 0;
            foreach (var ch in new string(str.ToCharArray().Reverse().ToArray()))
            {
                if (Characters.Contains(ch))
                {
                    result += Characters.IndexOf(ch) * ((long)Math.Pow(Length, j));
                    j++;
                }
            }
            return result;
        }

    }
}

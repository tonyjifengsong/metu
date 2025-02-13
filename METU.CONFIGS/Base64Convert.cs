using System.IO;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class Base64Convert
    {

        /// <summary>
        /// 文件转换成Base64字符串
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns></returns>
        public static String FileToBase64(string fileName)
        {
            string strRet = null;

            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, bt.Length);
                strRet = Convert.ToBase64String(bt);
                fs.Close();
            }
            catch  
            {
                return "";
                 
            }

            return strRet;
        }

        /// <summary>
        /// Base64字符串转换成文件
        /// </summary>
        /// <param name="strInput">base64字符串</param>
        /// <param name="fileName">保存文件的绝对路径</param>
        /// <returns></returns>
        public static bool Base64ToFileAndSave(string strInput, string fileName)
        {
            bool bTrue = false;

            try
            {
                byte[] buffer = Convert.FromBase64String(strInput);
                FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                bTrue = true;
            }
            catch  
            {
                //FileHelpers.Writelog(ex, "ex");
                return false;
            }

            return bTrue;
        }
    }
}

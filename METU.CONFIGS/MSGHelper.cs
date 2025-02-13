namespace System
{
    /// <summary>
    /// 异常类提示
    /// </summary>
    public static  class MSGBox
    {
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="Msg"></param>
        public static void EXMessage(string Msg,string MSGCode="0")
        {
            throw new DebugException(MSGCode, Msg);
        }

       
    }
}

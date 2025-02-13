using METU.CACHES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace METU.CONFIGS
{
    public class syslogger  
    {
        public string GetFunctionName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().Name;

        }
        public bool WriteDataBase(object obj, bool WriteFlag = false)
        {

            if (!WriteFlag) return false;
            if (obj == null) return false;
            string log = JsonSerializer.Serialize(obj);

            return true;
        }

        /// <summary>
        /// 把字符串写到日志文件中
        /// </summary>
        /// <param name="model">对象参数</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public bool Writelog(object obj, string filepath = null, bool WriteFlag = false)
        {

            if (filepath == null)
            {
                filepath = SysConfig.LOGFILE;
            }

            if (!WriteFlag) return false;
            if (obj == null) return false;
            string log = JsonSerializer.Serialize(obj);
            string servicename = obj.GetType().Name.ToString();
            string logpath = BaseConfig.Config_BaseDirectory + @"Log\";
            if (servicename != null)
            {
                logpath = logpath + servicename + "_Log.txt";
            }

            var isWrite = FileHelper.writefile(log, filepath);
            return isWrite;
        }

        /// <summary>
        /// 把对象写入日志系统
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="writeFunc"></param>
        /// <returns></returns>
        public bool WriteObject(object obj, Func<Dictionary<string, string>, bool> writeFunc)
        {
            var dic = obj.ToDictionary();
            return writeFunc(dic);
        }

        /// <summary>
        /// 把对象写入日志系统
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="writeFunc">方法名称</param>
        /// <returns></returns>
        public bool WriteObject(object obj, Func<object, bool> writeFunc)
        {
            return writeFunc(obj);
        }



        /// <summary>
        /// 把字符串写到日志文件中
        /// </summary>
        /// <param name="model">对象参数</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public bool WriteObject(object obj, string filepath = null, bool WriteFlag = false, bool isWriteDataBase = false)
        {

            if (!WriteFlag) return false;
            var IsWrite = Writelog(obj, filepath, WriteFlag);
            WriteDataBase(obj, isWriteDataBase);

            return IsWrite;
        }
    }
}

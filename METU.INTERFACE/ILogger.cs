using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{

    public interface ILogger
    {
        bool WriteObject(object obj, Func<object, bool> writeFunc);
        bool WriteObject(object obj, Func<Dictionary<string, string>, bool> writeFunc);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="obj">对象参数</param>
        /// <param name="filepath">日志文件地址</param>
        /// <param name="WriteFlag">是否启用写日志功能，默认不写日志</param>
        /// <param name="isWriteDataBase">是否把数据写入数据库（默认不写入数据库）</param>
        /// <returns></returns>
        bool WriteObject(object obj, string filepath = null, bool WriteFlag = false, bool isWriteDataBase = false);
        /// <summary>
        /// 把对象信息序列化后写入数据库
        /// </summary>
        /// <param name="obj">对象参数</param>
        /// <param name="WriteFlag">是否写日志</param>
        /// <returns></returns>
        bool WriteDataBase(object obj, bool WriteFlag = false);
        /// <summary>
        /// 把对象信息序列化后写入文本文件
        /// </summary>
        /// <param name="obj">对象参数</param>
        /// <param name="filepath">日志文件地址</param>
        /// <param name="WriteFlag">是否写入文本文件</param>
        /// <returns></returns>
        bool Writelog(object obj, string filepath = null, bool WriteFlag = false);

    }
}

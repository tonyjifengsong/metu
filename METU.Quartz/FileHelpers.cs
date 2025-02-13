using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace System
{/// <summary>
/// 
/// </summary>
      static class FileHelpers
    {/// <summary>
     /// 将信息写入日志
     /// </summary>
     /// <param name="message">信息参数</param>
     /// <param name="filepath">文件路径</param>
     /// <param name="asyncflag">是否异步</param>
        public static void Writelog(object message, string prefix = null, bool isWriteTime = true, string fileName = null, string filepath = null, bool asyncflag = true)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "/log";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (prefix == null)
                {
                    prefix = "";
                    filepath = $"{path}/{prefix}{fileName}{DateTime.Now.ToString("yyyyMMddHH")}.txt";
                }
                else
                {
                    prefix = prefix + "_";
                    filepath = $"{path}/{prefix}{fileName}{DateTime.Now.ToString("yyyyMMddHH")}.txt";
                }

                if (asyncflag)
                {
                    asyncWriteFile(message, filepath, isWriteTime);
                    return;
                }
                else
                {
                    Writefile(message, filepath, isWriteTime);
                }
            }
            else
            {

                if (asyncflag)
                {
                    asyncWriteFile(message, filepath, isWriteTime);
                    return;
                }
                else
                {
                    Writefile(message, filepath, isWriteTime);
                }
            }
        }
        /// <summary>
        /// 异步模式写日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>

        static async Task<bool> asyncWriteFile(object model, string filepath, bool isWriteTime = true)
        {
            bool r = await Task.Run(() =>
            {
                return Writefile(model, filepath, isWriteTime);
            });

            return r;
        }
        /// <summary>
        /// 把字符串写到日志文件中
        /// </summary>
        /// <param name="model">对象参数</param>
        /// <param name="fileName">完整的文件名称</param>
        /// <returns></returns>
        static bool Writefile(object model, string fileName, bool isWriteTimeflag = true)
        {

            if (fileName.Length == 0) return false;
            string fullFileName = fileName;
            StreamWriter writer = null;
            //事务处理读写文件

            string content = "";
            try
            {
                if (model as string != null)
                {
                    content = model.ToString();
                }
                else
                {
                    content = JsonConvert.SerializeObject(model);
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                if (File.Exists(fullFileName))
                {
                    FileStream fs = new FileStream(fullFileName, FileMode.Append);
                    writer = new StreamWriter(fs);

                    if (isWriteTimeflag)
                    {
                        writer.WriteLine("[" + System.DateTime.Now + "]" + content);

                    }
                    else
                    {
                        writer.WriteLine(content);
                    }
                    writer.Close();
                }
                else
                {
                    writer = new StreamWriter(fullFileName, false, System.Text.Encoding.GetEncoding("UTF-8"));
                    if (isWriteTimeflag)
                    {
                        writer.Write("[" + System.DateTime.Now + "]" + content);
                    }
                    else
                    {
                        writer.WriteLine(content);
                    }
                    writer.Close();
                }

            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return true;
        }
    }
}

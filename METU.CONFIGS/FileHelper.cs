using METU.CACHES;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Created by  tony  2017-9-27  Memo:日志操作基类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="jsonName"></param>
        /// <returns></returns>
       public static bool checkFileExists(string jsonName)
        {
            string str = jsonName;
            string baseDirectory = AppContext.BaseDirectory;
             Writelog("ConfHelper AppContext.BaseDirectory:");
           Writelog(baseDirectory);
            baseDirectory = baseDirectory.Replace("\\", "/");
            string text = baseDirectory + "/" + str;
            int num = baseDirectory.IndexOf("/bin");
            if (num > 0)
            {
                Writelog("ConfHelper Contains /bin:");
                text = baseDirectory.Substring(0, num) + "/" + str;
                Writelog("ConfHelper Contains /bin  file path:");
                Writelog(text);
            }

             Writelog("ConfHelper  constructor:");
            text = text.Replace("\\", "/").Replace("//", "/");
            Writelog(text);
             Writelog("ConfHelper config file path:" + text);
            string _path = text;
            return File.Exists(_path);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="filepath"></param>
        /// <param name="asyncflag"></param>
        public static void writelog(string message, string filepath, bool asyncflag)
        {
            if (!CommonCache.EnabledLog) return;
            if (asyncflag)
            {
                asyncWriteFile(message, filepath);
                return;
            }
            else
            {
                writefile(message, filepath);
            }
        }

        /// <summary>
        /// 异步模式写日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>

        public static async Task<bool> asyncWriteFile(string model, string filepath)
        {
            bool r = await Task.Run(() =>
            {
                for (int i = 0; i < 10000000; i++)
                {
                    writefile(i.ToString(), filepath);
                }
                return writefile(model, filepath);
            });

            return r;
        }
        /// <summary>
        /// 写日志到文件中
        /// </summary>
        /// <param name="model">对象名称</param>
        /// <param name="filepath">完整的日志文件路径</param>
        /// <returns></returns>
        public static bool writefile(object model, string filepath)
        {
            if (model == null) return false;
            if (filepath.Length == 0) return false;

            StreamWriter writer = null;
            string str = JsonConvert.SerializeObject(model); ;
            //事务处理读写文件

            try
            {
                if (File.Exists(filepath))
                {
                    FileStream fs = new FileStream(filepath, FileMode.Append);
                    writer = new StreamWriter(fs);
                    writer.WriteLine(System.DateTime.Now.ToString());
                    writer.WriteLine(str);
                    writer.WriteLine("");
                    writer.Close();

                }
                else
                {
                    writer = new StreamWriter(filepath, false, System.Text.Encoding.GetEncoding("UTF-8"));
                    writer.WriteLine(System.DateTime.Now.ToString());
                    writer.WriteLine(str);
                    writer.WriteLine("");
                    writer.Close();
                }

            }
            catch (IOException ex)
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
        /// <summary>
        /// 把字符串写到日志文件中
        /// </summary>
        /// <param name="model">对象参数</param>
        /// <param name="fileName">完整的文件名称</param>
        /// <returns></returns>
        public static bool writefile(string model, string fileName)
        {
            if (fileName.Length == 0) return false;
            string fullFileName = string.Empty;
            StreamWriter writer = null;
            fullFileName = fileName;
            //事务处理读写文件

            try
            {
                if (File.Exists(fullFileName))
                {
                    FileStream fs = new FileStream(fullFileName, FileMode.Append);
                    writer = new StreamWriter(fs);
                    writer.WriteLine(System.DateTime.Now.ToString());
                    writer.WriteLine(model);
                    writer.WriteLine("");
                    writer.Close();
                }
                else
                {
                    writer = new StreamWriter(fullFileName, false, System.Text.Encoding.GetEncoding("UTF-8"));
                    writer.WriteLine(System.DateTime.Now.ToString());
                    writer.Write(model);
                    writer.WriteLine("");
                    writer.Close();
                }

            }
            catch (IOException ex)
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
        /// <summary>
        /// 读取文件到字符串
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string filepath = null)
        {
            string path = filepath;
            //事务处理读写文件

            if (File.Exists(path))
            {
                StreamReader fs = File.OpenText(path);

                var content = fs.ReadToEnd().ToString();
                fs.Close();

                return content;
            }
            else
            {
                return "FileNotExists!";
            }
        }


        /// <summary>
        /// 将信息写入日志
        /// </summary>
        /// <param name="message">信息参数</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="asyncflag">是否异步</param>
        public static void Writelog(object message, string prefix = null, bool isWriteTime = true, string fileName = null, string filepath = null, bool asyncflag = true)
        {
            if (!CommonCache.EnabledLog) return;
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
            if (fileName.Length == ConstNum.Zero) return false;
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
                        writer.WriteLine("[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-ffff") + "]" + content);

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
                        writer.WriteLine("[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-ffff") + "]" + content);
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

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace METU.CONFIGS
{
    /// <summary>
    /// 异步操作日志
    /// </summary>
    public class asyncLogger
    {
        /// <summary>
        /// 将信息写入日志
        /// </summary>
        /// <param name="message">信息参数</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="asyncflag">是否异步</param>
        public void writelog(string message, string filepath, bool asyncflag = false)
        {
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
        /// 将对象写入日志
        /// </summary>
        /// <param name="message">日志对象</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="asyncflag">是否异步</param>
        public void writelog(object message, string filepath, bool asyncflag = false)
        {
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

        public async Task<bool> asyncWriteFile(string model, string filepath)
        {
            bool r = await Task.Run(() =>
            {
                return writefile(model, filepath);
            });

            return r;
        }
        /// <summary>
        /// 异步模式写日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public async Task<bool> asyncWriteFile(object model, string filepath)
        {
            bool r = await Task.Run(() =>
            {

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
        public bool writefile(object model, string filepath)
        {
            if (model == null) return false;
            if (filepath.Length == 0) return false;

            StreamWriter writer = null;
            string str = JsonSerializer.Serialize(model);
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
        public bool writefile(string model, string fileName)
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
        public string ReadFile(string filepath = null)
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

    }
}

using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace METU.CONFIGS
{
    public static class DLLASMHelper
    {
        /// <summary>
        /// 获取bin下全部dll
        /// </summary>
        public static List<Assembly> GetAllAssemblies()
        {
            try
            {
                return AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
            }catch(Exception ex) {
                FileHelper.Writelog("DLLASMHelper.GetAllAssemblies exception:");
                FileHelper.Writelog(ex.Message);
                return new List<Assembly>(); }
        }
        public static List<Assembly> GetPlugsAllAssemblies(string plugPrefix="plugs.")
        {
            try { 
            var plugins = DependencyContext.Default.RuntimeLibraries.Where(o => o.Name.StartsWith(plugPrefix)).Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();
            if (plugins == null) return new List<Assembly>();
            if (plugins.Length == 0) return new List<Assembly>();
            return plugins.ToList<Assembly>();
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("DLLASMHelper.GetPlugsAllAssemblies exception:");
                FileHelper.Writelog(ex.Message);
                return new List<Assembly>();
            }
        }
        /// <summary>
        /// 获取所有DLL文件信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetDllFullPathWithName(string pathstr = "")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string virpath = "";
            if (pathstr == null) virpath = "";
            if (pathstr.ToString().Trim().Length < 1) virpath = "";
            string dirp = AppDomain.CurrentDomain.BaseDirectory + virpath;
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = (FileInfo)fsi;
                    string x = System.IO.Path.GetDirectoryName(fi.FullName);

                    string s = System.IO.Path.GetExtension(fi.FullName);
                    string DllName = System.IO.Path.GetFileNameWithoutExtension(fi.FullName);
                    string filepath = fi.FullName;
                    if (s != null)
                    {
                        if (s.ToString().ToUpper() == ".DLL")
                        {
                            dic.Add(DllName, filepath);
                        }
                    }
                }
            }

            return dic;
        }


        public static string Config_BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static bool writefile(string model = null, string fileName = null)
        {
            if (fileName == null)
            {
                Config_BaseDirectory = Config_BaseDirectory + "log.txt";

            }
            else
            {
                Config_BaseDirectory = Config_BaseDirectory + fileName;
            }
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
    }
}

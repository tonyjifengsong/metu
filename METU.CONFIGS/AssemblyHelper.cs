using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace METU.CONFIGS
{
    public static class AssemblyHelper
    {/// <summary>
     /// 
     /// </summary>
        static List<string> m_fileList = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="extname"></param>
        /// <returns></returns>
        public static int GetDirectory(string srcPath, string extname = "dll")
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)     //判断是否文件夹
                    {
                        GetDirectory(i.FullName, extname);    //递归调用复制子文件夹
                    }
                    else
                    {
                        if (("tony" + i.Extension.ToString().ToLower()).IndexOf("dll") > 0)
                            m_fileList.Add(i.FullName);
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcpath"></param>
        /// <param name="endwith"></param>
        /// <param name="extname"></param>
        /// <returns></returns>

        public static List<Type> GetALLType(string srcpath, string endwith = "Service", string extname = "dll")
        {
            List<Type> result = new List<Type>();
            m_fileList.Clear();
            GetDirectory(srcpath, extname);
            foreach (var itm in m_fileList)
            {
                Assembly asm = Assembly.LoadFile(itm);
                List<Type> lst = new List<Type>();

                try
                {

                    lst = asm.ExportedTypes.ToList().Where(p => p.Name.EndsWith(endwith)).ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var sitm in lst)
                {
                    if (sitm.IsClass || sitm.IsInterface)
                    {

                        result.Add(sitm);

                    }
                }
            }
            m_fileList.Clear();
            return result;
        }/// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public static List<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable);//排除所有的系统程序集、Nuget下载包
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception ex)
                {

                    // ignored
                }
            }
            return list;
        }
        /// <summary>
        /// 私有变量
        /// </summary>
        private static List<FileInfo> lst = new List<FileInfo>();
        /// <summary>
        /// 获得目录下所有文件或指定文件类型文件(包含所有子文件夹)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="extName">扩展名可以多个 例如 .mp3.wma.rm</param>
        /// <returns>List<FileInfo></returns>
        public static List<FileInfo> getFile(string path, string extName)
        {
            getdir(path, extName);
            return lst;
        }
        /// <summary>
        /// 私有方法,递归获取指定类型文件,包含子文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extName"></param>
        private static void getdir(string path, string extName)
        {
            try
            {
                string[] dir = Directory.GetDirectories(path); //文件夹列表   
                DirectoryInfo fdir = new DirectoryInfo(path);
                FileInfo[] file = fdir.GetFiles();
                //FileInfo[] file = Directory.GetFiles(path); //文件列表   
                if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空                   
                {
                    foreach (FileInfo f in file) //显示当前目录所有文件   
                    {
                        if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
                        {
                            lst.Add(f);
                        }
                    }
                    foreach (string d in dir)
                    {
                        getdir(d, extName);//递归   
                    }
                }
            }
            catch (Exception ex)
            {



            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllEntities()
        {
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var itm in lst)
                {
                    if (itm.IsClass)
                    {
                        if (itm.GetInterface("IEntity") != null)
                        {
                            result.Add(itm);
                        }
                    }
                }
            }
            return result;
        }/// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public static List<Type> GetAllClasses()
        {
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var itm in lst)
                {
                    if (itm.IsClass)
                    {

                        result.Add(itm);

                    }
                }
            }
            return result;
        }/// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public static List<Type> GetAllInterfaces()
        {
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var itm in lst)
                {
                    if (itm.IsInterface)
                    {

                        result.Add(itm);

                    }
                }
            }
            return result;
        }/// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public static List<Type> GetAllGenericTypes()
        {
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var itm in lst)
                {
                    if (itm.IsGenericType)
                    {

                        result.Add(itm);

                    }
                }
            }
            return result;
        }/// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public static List<Type> GetAllGenericTypeDefinitions()
        {
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var itm in lst)
                {
                    if (itm.IsGenericTypeDefinition)
                    {

                        result.Add(itm);

                    }
                }
            }
            return result;
        }/// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public static List<Type> GetAllAbstracts()
        {
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var itm in lst)
                {
                    if (itm.IsAbstract)
                    {

                        result.Add(itm);

                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取指定接口的所有类
        /// </summary>
        /// <param name="interfacename"></param>
        /// <returns></returns>
        public static List<Type> GetAllClass(string interfacename = "IEntity", string endwith = "")
        {
            if (interfacename == null)
            {
                interfacename = "IEntity";
            }
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.Where(t => t.IsInterface && t.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                foreach (var itm in lst)
                {
                    if (itm.IsClass)
                    {
                        if (itm.GetInterface("IEntity") != null)
                        {
                            result.Add(itm);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定接口的所有接口
        /// </summary>
        /// <param name="interfacename"></param>
        /// <returns></returns>
        public static List<Type> GetAllInterface(string endwith = "Service", string interfacename = "IEntity", bool checkinterface = false)
        {
            if (interfacename == null)
            {
                interfacename = "IEntity";
            }
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();

            foreach (Assembly item in lists)
            {
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.Where(t => t.IsInterface && t.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                catch (Exception ex)
                {

                    continue;
                }
                if (checkinterface)
                {
                    if (interfacename == null) if (interfacename.Trim().Length == 0) interfacename = "interfacename";
                    foreach (var itm in lst)
                    {
                        if (itm.IsClass)
                        {
                            if (itm.GetInterface("IEntity") != null)
                            {
                                result.Add(itm);
                            }
                        }
                    }
                }
                else
                {
                    result = lst;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取所有DLL文件信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetDllFullPathWithName(string pathstr = "", bool reload = false)
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
        /// <summary>
        /// 
        /// </summary>

        public static string Config_BaseDirectory = "";

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
                    writer.Dispose();
                }
                else
                {
                    FileStream fs = new FileStream(fullFileName, FileMode.Create);

                    writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));
                    writer.WriteLine(System.DateTime.Now.ToString());
                    writer.Write(model);
                    writer.WriteLine("");
                    writer.Dispose();
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
                    writer.Dispose();
                }
            }


            return true;
        }


        /// <summary>
        /// 加载程序集()  Infrastructure = assemblyHelper.Load(m => m.Name.EndsWith("METU.SysBLL")).FirstOrDefault()
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> Load(Func<CompilationLibrary, bool> predicate = null)
        {
            if (predicate == null)
                return DependencyContext.Default.CompileLibraries.Select(m => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(m.Name))).ToList();

            return DependencyContext.Default.CompileLibraries.Where(predicate).Select(m => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(m.Name))).ToList();
        }

        /// <summary>
        /// 获取当前程序集的名称
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAssemblyName()
        {
            return Assembly.GetCallingAssembly().GetName().Name;
        }
    }
}

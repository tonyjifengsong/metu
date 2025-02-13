using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace System
{/// <summary>
/// 
/// </summary>
    public static class ASMHelper
    {
        /// <summary>  
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>  
        /// <param name="assemblyName">程序集</param>
        public static Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }
                return result;
            }
            return new Dictionary<Type, Type[]>();
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
                //  Writelog(ex, "asmex");


            }
        }


        /// <summary>
        /// 获取bin下全部dll
        /// </summary>
        public static List<Assembly> GetAllAssemblies()
        {
            try
            {
                System.AppDomain _Domain = System.AppDomain.CurrentDomain;
                Assembly[] _AssemblyList = _Domain.GetAssemblies();
                FileHelper.Writelog("GetAllAssemblies");
                return _AssemblyList.ToList();
            }catch(Exception ex)
            {
                FileHelper.Writelog("GetAllAssemblies exception:");
                FileHelper.Writelog(ex.Message);
            }
            return new List<Assembly>();
        }

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
                    // Writelog(ex, "asmex");
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
                    lst = item.ExportedTypes.Where(t => t.IsInterface && t.Name.EndsWith(endwith, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                catch (Exception ex)
                {
                    // Writelog(ex, "asmex");
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
        /// 获取指定接口的所有类
        /// </summary>
        /// <param name="interfacename"></param>
        /// <returns></returns>
        public static List<Type> GetMidwareAllClass(string endwith = "TonyMiddleWare")
        {
            FileHelper.Writelog("GetMidwareAllClass endwith:");
            FileHelper.Writelog(  endwith );
            List<Type> result = new List<Type>();
            var lists = GetAllAssemblies();
            FileHelper.Writelog("GetAllAssemblies is end!");
            if (lists == null)
            {
                FileHelper.Writelog("GetMidwareAllClass lists is null!");
                return new List<Type>(); 
            }
            if (lists.Count<1)
            {
                FileHelper.Writelog("GetMidwareAllClass lists is Zero!");
                return new List<Type>();
            }
            foreach (Assembly item in lists)
            {
               
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.Where(t => t.Name.EndsWith(endwith, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                catch (Exception ex)
                {
                    FileHelper.Writelog("Assembly item in lists");

                    FileHelper.Writelog(item.FullName);
                    FileHelper.Writelog(ex.Message);
                    continue;
                }
                foreach (var itm in lst)
                {
                    // FileHelper.Writelog(itm.Name);
                    try
                    {
                        if (itm.IsClass)
                        {
                            Writelog("Isclass");
                            if (itm.GetInterface("I" + endwith) != null)
                            {
                                //FileHelper.Writelog(itm.Name);
                                //FileHelper.Writelog("result.Add(itm)");
                                result.Add(itm);
                            }
                        }
                        if (itm.IsInterface)
                        {
                            //FileHelper.Writelog("IsInterface");
                            if (itm.Name.EndsWith(endwith, StringComparison.OrdinalIgnoreCase))
                            {
                                FileHelper.Writelog(itm.Name);

                                result.Add(itm);
                            }
                        }
                    }catch(Exception ex)
                    {
                        FileHelper.Writelog("var itm in lst");

                        FileHelper.Writelog(itm.FullName);
                        FileHelper.Writelog(ex.Message);
                        continue;
                    }
                }
            }
            if (result == null)
            {
                FileHelper.Writelog("GetMidwareAllClass result is Null!");
                return new List<Type>();
            }
            if (result.Count < 1)
            {
                FileHelper.Writelog("GetMidwareAllClass result is Zero!");
                return new List<Type>();
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
                    if (fullname.IndexOf("microsoft") >0) continue;
                    lst = item.ExportedTypes.Where(t => t.IsInterface && t.Name.EndsWith(endwith, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                catch (Exception ex)
                {
                    // Writelog(ex, "asmex");
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
        /// 获取bin下全部dll
        /// </summary>
        public static List<Assembly> GetAllAssembly()
        {
            List<Assembly> list = new List<Assembly>();
            foreach (var itm in GetDllFullPathWithName())
            {
                string pLocal = itm.Value;
                try
                {
                    Assembly assembly = Assembly.LoadFrom(pLocal); //加实现类载程序集
                    list.Add(assembly);
                }
                catch (Exception ex) { }
            }
            return list;
        }
        /// <summary>
        /// 获取指定接口的所有类
        /// </summary>
        /// <param name="interfacename"></param>
        /// <returns></returns>
        public static List<Type> GetSpecClasses(string endwith = "Model")
        {

            List<Type> result = new List<Type>();
            var lists = GetAllAssembly();
            foreach (Assembly item in lists)
            {
                // Writelog(item.FullName);
                // Writelog("\n");
                List<Type> lst = new List<Type>();

                try
                {
                    string fullname = "tony" + item.FullName.ToString().ToLower();
                    if (fullname.IndexOf("system.") > 0) continue;
                    if (fullname.IndexOf("microsoft") > 0) continue;
                    lst = item.ExportedTypes.Where(t => t.Name.ToLower().IndexOf(endwith.ToLower()) > 0).ToList();
                }
                catch (Exception ex)
                {
                    // Writelog(ex, "asmex");
                    continue;
                }
                foreach (var itm in lst)
                {
                    // Writelog(itm.FullName);
                    // Writelog("\n");
                    if (itm.IsClass)
                    {
                        // Writelog("Isclass");
                        //if (itm.GetInterface("I" + endwith) != null)
                        //{
                        Writelog(itm.Name);
                        // Writelog("result.Add(itm)");
                        result.Add(itm);
                        //  }
                    }
                    if (itm.IsInterface)
                    {
                        // Writelog("IsInterface");
                        if (itm.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase))
                        {
                            Writelog(itm.Name);

                            result.Add(itm);
                        }
                    }
                }
            }
            return result;
        }

        public static string Config_BaseDirectory = "";

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static bool Writelog(string model = null, string fileName = null)
        {
            if (fileName == null)
            {
                Config_BaseDirectory = Config_BaseDirectory + "log.txt";

            }
            else
            {
                Config_BaseDirectory = Config_BaseDirectory + fileName;
            }
            if (fileName.Length ==0) return false;
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
                // Writelog(ex, "asmex");
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
    }
}

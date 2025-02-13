using METU.CACHES;
using METU.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ConfigureServicesExtentions
    {
        /// <summary>
        /// 添加应用服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="module">DLL命名空间名</param>
        public static void AddApplicationServices(this IServiceCollection services, string module)
        {

            foreach (var item in ASMHelper.GetClassName(module))
            {
                foreach (var typeArray in item.Value)
                {
                    FileHelper.Writelog("AddApplicationServices:");
                    FileHelper.Writelog(typeArray.FullName);
                    FileHelper.Writelog(item.Key.FullName);
                    services.AddScoped(typeArray, item.Key);
                }
            }


        }
        /// <summary>
        /// 从指定程序集中注入单例服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            if (assembly == null)
                return services;

            var serviceTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes().Any(m => m.GetType() == typeof(SingletonAttribute)));

            foreach (var serviceType in serviceTypes)
            {
                FileHelper.Writelog("AddSingletonFromAssembly:");
                FileHelper.Writelog(serviceType.FullName);
                
                services.AddSingleton(serviceType);
            }

            return services;
        }

        /// <summary>
        /// 添加基础工具
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUtils(this IServiceCollection services)
        {
            return services.AddSingletonFromAssembly(Assembly.GetExecutingAssembly());
        }
        /// <summary>
        /// 添加MVC相关基础工具
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUtilsMvc(this IServiceCollection services)
        {
            return services.AddSingletonFromAssembly(Assembly.GetExecutingAssembly());
        }
        /// <summary>
        /// 添加配置信息
        /// </summary>
        /// <param name="services"></param>
        /// <param name="module"></param>
        public static void AddAutoReadDBConfigServices(this IServiceCollection services, string jsonurl = null, string ConfigKey = null)
        {
             ConfHelper conf = null;

            conf = new ConfHelper(jsonurl);

            bool isservice = false;
            try
            {
                isservice = bool.Parse(conf.ReadConfig("dbconfig"));
            }
            catch
            {
                FileHelper.Writelog("the file {"+ jsonurl+"} can not found");
                isservice = false;
            }
            if (!isservice)
            {
                FileHelper.Writelog("dbconfig is false, do not read config");
                
            }
            List<DBConfigString> list = new List<DBConfigString>();
            if (ConfigKey == null)
            {
                FileHelper.Writelog("ConfigKey is null");

                list = conf.ReadList<DBConfigString>("Connections");
            }
            else
            {
                if (ConfigKey.ToString().Trim().Length == 0)
                {
                    FileHelper.Writelog("ConfigKey read default  read node: Connections ");

                    list = conf.ReadList<DBConfigString>("Connections");
                }
                else
                {
                    FileHelper.Writelog("ConfigKey read    node : "+ ConfigKey);

                    list = conf.ReadList<DBConfigString>(ConfigKey);
                }
            }
            if (list == null)
            {
                FileHelper.Writelog("Config  read    Null ");
               
            }
            FileHelper.Writelog("ConfigKey read info: " + list.toJson());
            foreach (var itm in list)
                {
                if (itm.Database == null) { continue; }
                if (itm.Database.ToString().Trim().Length <1) { continue; }
                if (itm.ConnString == null) { continue; }
                if (itm.ConnString.ToString().Trim().Length <1) { continue; }
                bool addflag = true;
                foreach (var citm in CommonCache.ConfigCaches)
                {
                    if (citm.Key.ToUpper().Trim() == itm.Database.ToUpper().Trim())
                    {
                        addflag = false;
                        break;
                    }
                }
                if (addflag)
                {
                    if (itm.Database.ToLower() == "connnectionstring" || itm.Database.ToLower() == "MSSQLConnectionString".ToLower())
                    {
                        CommonCache.MSSQLConnectionString = itm.ConnString;
                    }
                    CommonCache.ConfigCaches.Add(itm.Database.ToUpper(), itm.ConnString);
                }
            }
        }  

        /// <summary>
        /// 添加业务注入
        /// </summary>
        public static void AddServices(this IServiceCollection services, string endwith = "Service")
        {
            try { 
            var types = ASMHelper.GetMidwareAllClass(endwith);
                if (types == null)
                {
                    FileHelper.Writelog("AddServices types  is null！");
                    return  ;
                }
                if (types.Count < 1)
                {
                    FileHelper.Writelog("AddServices types  is Zero！");
                    return  ;
                }
                var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase));
                if (interfaces == null)
                {
                    FileHelper.Writelog("AddServices interfaces  is null！");
                    return  ;
                }
                if (interfaces.Count() < 1)
                {
                    FileHelper.Writelog("AddServices interfaces  is Zero！");
                    return  ;
                }

                foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    FileHelper.Writelog("AddServices Type:" + implementType.FullName);

                    services.AddScoped(interfaceType, implementType);
                }
            }
        }
            catch (Exception ex)
            {
                FileHelper.Writelog("AddServices Exceptionis:" + ex.Message);

            }
}
        /// <summary>
        /// 添加数据库配置
        /// </summary>
        public static void AddDBConfig(this IServiceCollection services, string endwith = "DBHelper")
        {
            try { 
            var types = ASMHelper.GetMidwareAllClass(endwith);
                if (types == null)
                {
                    FileHelper.Writelog("AddDBConfig types  is null！");
                    return;
                }
                if (types.Count < 1)
                {
                    FileHelper.Writelog("AddDBConfig types  is Zero！");
                    return;
                }
                var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase));
                if (interfaces == null)
                {
                    FileHelper.Writelog("AddDBConfig interfaces  is null！");
                    return;
                }
                if (interfaces.Count() < 1)
                {
                    FileHelper.Writelog("AddDBConfig interfaces  is Zero！");
                    return;
                }

                foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    FileHelper.Writelog("AddDBConfig AddSingleton Type:" + implementType.FullName);

                    services.AddSingleton(interfaceType, Activator.CreateInstance(implementType, null));
                }
            }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("AddDBConfig Exceptionis:" + ex.Message);

            }
        }
        public static void AddSingle(this IServiceCollection services, string endwith = "Single")
        {
            try
            {
                var types = ASMHelper.GetMidwareAllClass(endwith);
                if (types == null)
                {
                    FileHelper.Writelog("AddSingle types  is null！");
                    return;
                }
                if (types.Count() < 1)
                {
                    FileHelper.Writelog("AddSingle types  is Zero！");
                    return;
                }
                var interfaces = types.Where(t => t.FullName != null && t.IsClass && t.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase));
                if (interfaces == null)
                {
                    FileHelper.Writelog("AddSingle interfaces  is null！");
                    return;
                }
                if (interfaces.Count() < 1)
                {
                    FileHelper.Writelog("AddSingle interfaces  is Zero！");
                    return;
                }
                foreach (var interfaceType in interfaces)
                {
                    FileHelper.Writelog("AddSingle Type:" + interfaceType.FullName);

                    services.AddSingleton(Activator.CreateInstance(interfaceType, null));

                }
            }catch(Exception ex)
            {
                FileHelper.Writelog("AddSingle Exceptionis:" + ex.Message);

            }

        }
        
        /// <summary>
        /// SPD配置服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoConfig(this IServiceCollection services)
        {
            FileHelper.Writelog("AddAutoReadDBConfigServices config\\appsettings:获取数据配置信息");

            if (FileHelper.checkFileExists("config\\appsettings.json"))
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-config\\appsettings.json");

                services.AddAutoReadDBConfigServices("config\\appsettings.json");//获取数据配置信息
            }
            else
            {
                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices-Default json file");

                FileHelper.Writelog("AddHost:AddAutoReadDBConfigServices");
                services.AddAutoReadDBConfigServices();//获取数据配置信息
            }
            FileHelper.Writelog("AddSingle:添加单例");
            services.AddSingle();//添加单例
           
            FileHelper.Writelog("AddDBConfig:缓存注入");
            services.AddDBConfig("Cache");//缓存注入
            FileHelper.Writelog("AddDBConfig:数据库注入");
            services.AddDBConfig();//数据库注入
            FileHelper.Writelog("AddDBConfig:队列注入");
            services.AddDBConfig("queue");//队列注入
            FileHelper.Writelog("AddServices:服务层接口注入");
            services.AddServices();//服务层接口注入
            services.AddService();
            FileHelper.Writelog("AddAutoService:服务注入自动添加");
            services.AddAutoService();//服务注入自动添加
           
        }
        /// <summary>
        /// 添加业务注入
        /// </summary>
        public static void AddService(this IServiceCollection services, string endwith = "metubll")
        {
            try { 
            var types = ASMHelper.GetMidwareAllClass(endwith);

                FileHelper.Writelog("AddService执行！");
                if (types == null )
                {
                    FileHelper.Writelog("AddService types  is null！");
                    return;
                }
                if (  types.Count < 1)
                {
                    FileHelper.Writelog("AddService types  is Zero！");
                    return;
                }
                var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase));
                if (interfaces == null)
                {
                    FileHelper.Writelog("AddService interfaces  is null！");
                    return;
                }
                if (interfaces.Count() < 1)
                {
                    FileHelper.Writelog("AddService interfaces  is Zero！");
                    return;
                }

                foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    FileHelper.Writelog("AddService AddScoped Type:" + implementType.FullName);

                    services.AddScoped(interfaceType, implementType);
                }
            }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("AddService Exceptionis:" + ex.Message);

            }
        }
        /// <summary>
        /// 自动注入业务服务
        /// </summary>
        public static void AddAutoService(this IServiceCollection services, string endwith = "CoreServices")
        {
            try { 
            var types = ASMHelper.GetMidwareAllClass(endwith);
                if (types == null)
                {
                    FileHelper.Writelog("AddAutoService types  is null！");
                    return;
                }
                if (types.Count < 1)
                {
                    FileHelper.Writelog("AddAutoService types  is Zero！");
                    return;
                }
                var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith(endwith, StringComparison.OrdinalIgnoreCase));
                if (interfaces == null)
                {
                    FileHelper.Writelog("AddAutoService interfaces  is null！");
                    return;
                }
                if (interfaces.Count() < 1)
                {
                    FileHelper.Writelog("AddAutoService interfaces  is Zero！");
                    return;
                }
                foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    FileHelper.Writelog("AddAutoService ICoreServices Type:" + implementType.FullName);

                    ICoreServices svr =(ICoreServices)Activator.CreateInstance(implementType, null);
                    svr.AddService(services);
                }
            }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog("AddAutoService Exceptionis:" + ex.Message);

            }
        }
    }
}

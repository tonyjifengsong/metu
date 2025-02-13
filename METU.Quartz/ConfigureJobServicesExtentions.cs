using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System.Linq;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigureJobServicesExtentions
    {
        
        
       
        /// <summary>
        /// SPD配置服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoJobConfig(this IServiceCollection services)
        {
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//注册ISchedulerFactory的实例。           
            // 这里使用瞬时依赖注入
            services.AddAutoJobStartupService();          
            services.AddAutoJobService();//服务注入自动添加
        }
        /// <summary>
        /// 自动注入业务服务
        /// </summary>
        public static void AddAutoJobStartupService(this IServiceCollection services)
        {

            var types = ASMHelper.GetMidwareAllClass("JobContext");
            var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith("JobContext", StringComparison.OrdinalIgnoreCase));
            foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && !m.IsInterface && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    services.AddSingleton(implementType);
                }
            }

        }
        /// <summary>
        /// 自动注入业务服务
        /// </summary>
        public static void AddAutoJobService(this IServiceCollection services)
        {

            var types = ASMHelper.GetClass();
            var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.Name.ToString().ToLower() != "ijob".ToString().ToLower() && t.FullName.EndsWith("Job", StringComparison.OrdinalIgnoreCase));
            foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && !m.IsInterface && m.FullName.EndsWith("Job", StringComparison.OrdinalIgnoreCase) && interfaceType.IsAssignableFrom(m));
                if (implementType != null)
                {
                    services.AddTransient(implementType);
                }
            }

        }
    }
}

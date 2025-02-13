using METU.MSSQL;
using METU.MSSQL.MSSQL;
using Microsoft.Extensions.DependencyInjection;

namespace System
{
    public static partial class ServiceCollectionExtensions
    {
        #region added by tony 2019-12-31
       
        /// <summary>
        /// 添加核心业务注入
        /// </summary>
        public static void AddBLLCore(this IServiceCollection services)
        {

         services.AddScoped(typeof(BaseBLL<>));
        }
        public static void AddBaseCore(this IServiceCollection services)
        {

            services.AddSingleton<BaseCore>(new BaseCore());
        }
        public static void AddCoreBLL(this IServiceCollection services)
        {

            services.AddSingleton<CoreBLL>(new CoreBLL());
        }
        #endregion

    }
}

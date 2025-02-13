using METU.IDS4Server;
using METU.IDS4Server.Identity.Service;
using METU.IDS4Server.Identity.Service.Validator;
using METU.IDS4Server.Identity.UserApiService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace METU.IDS4Server
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env">环境</param>
        /// <returns></returns>
        public static IServiceCollection AddIDS4(this IServiceCollection services,  IHostEnvironment env)
        {
            //好像是欧盟联盟协议的cookie，要注释
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            services.AddIdentityServer()//Ids4服务
             .AddDeveloperSigningCredential()//添加开发人员签名凭据
             .AddInMemoryIdentityResources(IDS4Config.GetIdentityResources()) //添加内存apiresource
             .AddInMemoryApiResources(IDS4Config.GetApiResources())
             .AddInMemoryClients(IDS4Config.GetClients());//把配置文件的Client配置资源放到内存

          
            return services;
        }
        public static IServiceCollection AddIDS4(this IServiceCollection services)
        {
            //好像是欧盟联盟协议的cookie，要注释
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            services.AddIdentityServer()//Ids4服务
             .AddDeveloperSigningCredential()//添加开发人员签名凭据
             .AddInMemoryIdentityResources(IDS4Config.GetIdentityResources()) //添加内存apiresource
             .AddInMemoryClients(IDS4Config.GetClients());//把配置文件的Client配置资源放到内存


            return services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IServiceCollection AddMETUIDS4(this IServiceCollection services, IHostEnvironment env)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            #region 内存方式
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryApiResources(OAuthMemoryData.GetApiResources())
            //    .AddInMemoryClients(OAuthMemoryData.GetClients())
            //    .AddTestUsers(OAuthMemoryData.GetTestUsers());
            #endregion

            #region 数据库存储方式
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(OAuthMemoryData.GetApiResources())
                //.AddInMemoryClients(OAuthMemoryData.GetClients())
                .AddClientStore<ClientStore>()
                //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddResourceOwnerValidator<RoleTestResourceOwnerPasswordValidator>()
                .AddExtensionGrantValidator<WeiXinOpenGrantValidator>()
                .AddProfileService<UserProfileService>();//添加微信端自定义方式的验证
            #endregion

         
            return services;
        }
    }
}

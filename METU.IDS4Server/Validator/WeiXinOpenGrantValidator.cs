﻿using IdentityServer4.Validation;
using METU.IDS4Server.Identity.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace METU.IDS4Server.Identity.Service.Validator
{
    /// <summary>
    /// 自定义授权验证器
    /// </summary>
    public class WeiXinOpenGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => GrantTypeConstants.ResourceWeixinOpen;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                #region 参数获取 直接把授权后的openId 拿过来授权是不安全的，这里仅仅是一个Demo
                var openId = context.Request.Raw[ParamConstants.OpenId];
                var unionId = context.Request.Raw[ParamConstants.UnionId];
                var userName = context.Request.Raw[ParamConstants.UserName];
                #endregion

                #region 通过openId和unionId 参数来进行数据库的相关验证
                var claimList = await ValidateUserAsync(openId, unionId);
                #endregion

                #region 授权通过
                //授权通过返回
                context.Result = new GrantValidationResult
                (
                    subject: openId,
                    authenticationMethod: "custom",
                    claims: claimList.ToArray()
                );
                #endregion
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = ex.Message
                };
            }
        }

        #region Private Method
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        private async Task<List<Claim>> ValidateUserAsync(string openId, string unionId)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            //TODO 这里可以通过openId 和unionId 来查询用户信息，我这里为了方便测试还是直接写测试的openId 相关信息用户
            var user = OAuthMemoryData.GetWeiXinOpenIdTestUsers();

            if (user == null)
            { 
                //注册用户
            }

            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, $"{openId}"),
            };
        }
        #endregion
    }
}

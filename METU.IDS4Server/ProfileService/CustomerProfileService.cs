using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace METU.IDS4Server.ProfileService
{
    public class CustomerProfileService : IProfileService
    {
        protected readonly UserStore Users;

        /// <summary>
        /// 案例中用户使用的还是 Quickstart.UI 模板提供的用户，所以这里通过DI 容器取到用户资源
        /// </summary>
        /// <param name="users"></param>
        public CustomerProfileService(UserStore users)
        {
            Users = users;
        }

        public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // 通过 用户id 找到用户
           /* var user = Users.FindBySubjectId(context.Subject.GetSubjectId());
            if (user != null)
            {
                context.IssuedClaims.AddRange(user.Claims);
            }*/
            try
            {
                var claims = context.Subject.Claims.ToList();

                context.IssuedClaims = claims.ToList();
            }
            catch { }
           
            return Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}

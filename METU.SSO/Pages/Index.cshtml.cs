using IdentityModel;
using METU.IDS4Server.Identity.Model;
using METU.MODEL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace METU.SSO.Pages
{
    public class IndexModel : PageModel
    {
        public ConfigModel user;
        private readonly ILogger<IndexModel> _logger;
        CoreBLL corebll;
        public IndexModel(ILogger<IndexModel> logger, CoreBLL coreBLL)
        {
            corebll = coreBLL;
               var dt = coreBLL.db.ExecuteDataTable("select * from sysPage");
            _logger = logger;
        }

        public void OnGet()
        {
           
        }

        /// <summary>
        /// 登录post回发处理
        /// </summary>

        public async Task<IActionResult> OnPostAsync(string userName, string password, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
             user = corebll.db.ExecuteDataTable("select * from sysPage").toEntity<ConfigModel>();
            if (user != null)
            {

                AuthenticationProperties props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1))
                };

             
               await HttpContext.SignInAsync(user.ID,user.ToClaimsPrincipal(),props);
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
            }
            return Redirect("./");

        }
        [HttpPost]
        public async Task<IActionResult> OnPostAddAddressAsync(string userName, string password, string returnUrl = null)
        {
            if (returnUrl == null) returnUrl = "./Index";
            ViewData["returnUrl"] = returnUrl;
            //  Customers customers = _userDAL.Login(userName, password);
            //UserDAL userDAL = new UserDAL();  //不予许实例化对象

            AuthenticationProperties props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1)),
            };
            //注意这里应该引用Microsoft.AspNetCore.Http这个下面的

            await HttpContext.SignInAsync("10000", null, props);
            //HttpContext.SignOutAsync();
            if (returnUrl != null)
            {
                Redirect(returnUrl);
                //return Redirect("http://localhost:44396/home/index");
            }

            return null;
        }
    }
}

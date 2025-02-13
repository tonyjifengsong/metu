using METU.Admin.Model;
using METU.BasicCore.Controllers;

namespace METU.Admin.Pages
{
    public class IndexModel : MetuPageBase
    {
         
        CoreBLL corebll;
        public IndexModel(CoreBLL coreBLL)
        {
            corebll = coreBLL;
         
        }

        public void OnGet()
        {
            /* Account md = new Account();
             md.UserName = "tony";
             md.id = Guid.NewGuid().ToString();
             SysAdmin amd = new SysAdmin();
             amd.UserName = "tony";
             amd.id = Guid.NewGuid().ToString();
             corebll.dbh.Add(md);
             corebll.dbh.Add(amd);
             corebll.dbh.SaveChanges();
 */



        }
    }
}

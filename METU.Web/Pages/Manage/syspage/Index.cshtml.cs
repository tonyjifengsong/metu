using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspage
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SysPage> SysPage { get;set; }

        public async Task OnGetAsync()
        {
            SysPage = await _context.dbh.Set<SysPage>().ToListAsync();
        }
    }
}

using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;

namespace METU.Admin.Pages.Manage.syspageconfigs
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SysPageConfigs> SysPageConfig { get;set; }

        public async Task OnGetAsync()
        {
            SysPageConfig = await _context.dbh.Set<SysPageConfigs>().ToListAsync();
        }
    }
}

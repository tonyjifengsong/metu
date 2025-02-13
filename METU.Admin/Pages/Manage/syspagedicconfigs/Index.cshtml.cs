using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspagedicconfigs
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SyspageDicconfigs> SyspageDicconfig { get;set; }

        public async Task OnGetAsync()
        {
            SyspageDicconfig = await _context.dbh.Set<SyspageDicconfigs>().ToListAsync();
        }
    }
}

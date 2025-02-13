using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspageservices
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SysPageService> SysPageService { get;set; }

        public async Task OnGetAsync()
        {
            SysPageService = await _context.dbh.Set<SysPageService>().ToListAsync();
        }
    }
}

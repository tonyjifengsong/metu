using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspagedic
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SyspageDic> SyspageDic { get;set; }

        public async Task OnGetAsync()
        {
            SyspageDic = await _context.dbh.Set<SyspageDic>().ToListAsync();
        }
    }
}

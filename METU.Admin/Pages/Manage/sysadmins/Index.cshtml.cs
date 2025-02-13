using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;

namespace METU.Admin.Pages.Manage.sysadmins
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SysAdmin> SysAdmin { get;set; }

        public async Task OnGetAsync()
        {
            SysAdmin = await _context.dbh.Set<SysAdmin>().ToListAsync();
        }
    }
}

using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.accountroles
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SysaccountRole> SysaccountRole { get;set; }

        public async Task OnGetAsync()
        {
            SysaccountRole = await _context.dbh.Set<SysaccountRole>().ToListAsync();
        }
    }
}

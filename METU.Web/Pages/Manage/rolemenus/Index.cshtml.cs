using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.rolemenus
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<SysroleMenu> SysroleMenu { get;set; }

        public async Task OnGetAsync()
        {
            SysroleMenu = await _context.dbh.Set<SysroleMenu>().ToListAsync();
        }
    }
}

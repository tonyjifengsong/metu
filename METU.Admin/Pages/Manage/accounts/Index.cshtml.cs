using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.accounts
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<Account> Account { get;set; }

        public async Task OnGetAsync()
        {
            Account = await _context.dbh.Set<Account>().ToListAsync();
        }
    }
}

using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4clients
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<Id4client> Id4client { get;set; }

        public async Task OnGetAsync()
        {
            Id4client = await _context.dbh.Set<Id4client>().ToListAsync();
        }
    }
}

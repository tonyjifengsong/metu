using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4server
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<Id4server> Id4server { get;set; }

        public async Task OnGetAsync()
        {
            Id4server = await _context.dbh.Set<Id4server>().ToListAsync();
        }
    }
}

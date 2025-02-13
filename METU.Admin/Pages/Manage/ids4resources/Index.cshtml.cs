using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4resources
{
    public class IndexModel : MetuPageBase
    {
        

        public IndexModel(CoreBLL context)
        {
            _context = context;
        }

        public IList<Id4resource> Id4resource { get;set; }

        public async Task OnGetAsync()
        {
            Id4resource = await _context.dbh.Set<Id4resource>().ToListAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4server
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public Id4server Id4server { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Id4server = await _context.dbh.Set<Id4server>().FirstOrDefaultAsync(m => m.id == id);

            if (Id4server == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4resources
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public Id4resource Id4resource { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Id4resource = await _context.dbh.Set<Id4resource>().FirstOrDefaultAsync(m => m.id == id);

            if (Id4resource == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

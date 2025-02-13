using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4clients
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public Id4client Id4client { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Id4client = await _context.dbh.Set<Id4client>().FirstOrDefaultAsync(m => m.id == id);

            if (Id4client == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

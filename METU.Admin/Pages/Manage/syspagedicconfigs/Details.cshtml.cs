using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspagedicconfigs
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public SyspageDicconfigs SyspageDicconfig { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SyspageDicconfig = await _context.dbh.Set<SyspageDicconfigs>().FirstOrDefaultAsync(m => m.id == id);

            if (SyspageDicconfig == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

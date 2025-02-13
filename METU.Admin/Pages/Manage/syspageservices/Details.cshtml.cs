using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspageservices
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public SysPageService SysPageService { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysPageService = await _context.dbh.Set<SysPageService>().FirstOrDefaultAsync(m => m.id == id);

            if (SysPageService == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;

namespace METU.Admin.Pages.Manage.sysadmins
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public SysAdmin SysAdmin { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysAdmin = await _context.dbh.Set<SysAdmin>().FirstOrDefaultAsync(m => m.id == id);

            if (SysAdmin == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.rolemenus
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public SysroleMenu SysroleMenu { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysroleMenu = await _context.dbh.Set<SysroleMenu>().FirstOrDefaultAsync(m => m.id == id);

            if (SysroleMenu == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

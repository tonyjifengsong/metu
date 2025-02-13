using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.rolemenus
{
    public class DeleteModel : MetuPageBase
    {
        

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysroleMenu = await _context.dbh.Set<SysroleMenu>().FindAsync(id);

            if (SysroleMenu != null)
            {
                _context.dbh.Remove(SysroleMenu);
                await _context.dbh.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;

namespace METU.Admin.Pages.Manage.syspageconfigs
{
    public class DeleteModel : MetuPageBase
    {
        

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
        public SysPageConfigs SysPageConfig { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysPageConfig = await _context.dbh.Set<SysPageConfigs>().FirstOrDefaultAsync(m => m.id == id);

            if (SysPageConfig == null)
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

            SysPageConfig = await _context.dbh.Set<SysPageConfigs>().FindAsync(id);

            if (SysPageConfig != null)
            {
                _context.dbh.Remove(SysPageConfig);
                await _context.dbh.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

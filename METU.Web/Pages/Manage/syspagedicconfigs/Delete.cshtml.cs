using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspagedicconfigs
{
    public class DeleteModel : MetuPageBase
    {
        

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SyspageDicconfig = await _context.dbh.Set<SyspageDicconfigs>().FindAsync(id);

            if (SyspageDicconfig != null)
            {
                _context.dbh.Remove(SyspageDicconfig);
                await _context.dbh.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

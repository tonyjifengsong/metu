using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspageservices
{
    public class DeleteModel : MetuPageBase
    {
        

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysPageService = await _context.dbh.Set<SysPageService>().FindAsync(id);

            if (SysPageService != null)
            {
                _context.dbh.Remove(SysPageService);
                await _context.dbh.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

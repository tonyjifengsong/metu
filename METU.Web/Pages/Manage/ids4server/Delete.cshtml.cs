using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4server
{
    public class DeleteModel : MetuPageBase
    {
        

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
        public Id4server Id4server { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Id4server = await _context.dbh.Set<Id4server>().FirstOrDefaultAsync(m => m.id == id);

            if (Id4server == null)
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

            Id4server = await _context.dbh.Set<Id4server>().FindAsync(id);

            if (Id4server != null)
            {
                _context.dbh.Remove(Id4server);
                await _context.dbh.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

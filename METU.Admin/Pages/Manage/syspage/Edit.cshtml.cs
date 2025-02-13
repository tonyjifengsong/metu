using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspage
{
    public class EditModel : MetuPageBase
    {
        

        public EditModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
        public SysPage SysPage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysPage = await _context.dbh.Set<SysPage>().FirstOrDefaultAsync(m => m.id == id);

            if (SysPage == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.dbh.Attach(SysPage).State = EntityState.Modified;

            try
            {
                await _context.dbh.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysPageExists(SysPage.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SysPageExists(string id)
        {
            return _context.dbh.Set<SysPage>().Any(e => e.id == id);
        }
    }
}

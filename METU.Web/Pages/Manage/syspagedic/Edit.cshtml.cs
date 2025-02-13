using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspagedic
{
    public class EditModel : MetuPageBase
    {
        

        public EditModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
        public SyspageDic SyspageDic { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SyspageDic = await _context.dbh.Set<SyspageDic>().FirstOrDefaultAsync(m => m.id == id);

            if (SyspageDic == null)
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

            _context.dbh.Attach(SyspageDic).State = EntityState.Modified;

            try
            {
                await _context.dbh.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SyspageDicExists(SyspageDic.id))
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

        private bool SyspageDicExists(string id)
        {
            return _context.dbh.Set<SyspageDic>().Any(e => e.id == id);
        }
    }
}

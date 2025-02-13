using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;

namespace METU.Admin.Pages.Manage.syspageconfigs
{
    public class EditModel : MetuPageBase
    {
        

        public EditModel(CoreBLL context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.dbh.Attach(SysPageConfig).State = EntityState.Modified;

            try
            {
                await _context.dbh.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysPageConfigExists(SysPageConfig.id))
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

        private bool SysPageConfigExists(string id)
        {
            return _context.dbh.Set<SysPageConfigs>().Any(e => e.id == id);
        }
    }
}

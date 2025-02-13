using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.rolemenus
{
    public class EditModel : MetuPageBase
    {
        

        public EditModel(CoreBLL context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.dbh.Attach(SysroleMenu).State = EntityState.Modified;

            try
            {
                await _context.dbh.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysroleMenuExists(SysroleMenu.id))
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

        private bool SysroleMenuExists(string id)
        {
            return _context.dbh.Set<SysroleMenu>().Any(e => e.id == id);
        }
    }
}

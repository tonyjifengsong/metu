using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.accounts
{
    public class DeleteModel : MetuPageBase
    {
        

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
        public Account Account { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account = await _context.dbh.Set<Account>().FirstOrDefaultAsync(m => m.id == id);

            if (Account == null)
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

            Account = await _context.dbh.Set<Account>().FindAsync(id);

            if (Account != null)
            {
                _context.dbh.Remove(Account);
                await _context.dbh.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

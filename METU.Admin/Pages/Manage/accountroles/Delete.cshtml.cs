using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.accountroles
{
    public class DeleteModel : MetuPageBase
    {
        

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }

        [BindProperty]
        public SysaccountRole SysaccountRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
           

            SysaccountRole = await  _context.dbh.FindAsync<SysaccountRole>(id);

            if (SysaccountRole == null)
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

            SysaccountRole = await _context.dbh.FindAsync<SysaccountRole>(id);

            if (SysaccountRole != null)
            {
               _context.dbh.Remove(SysaccountRole);
                await _context.dbh.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

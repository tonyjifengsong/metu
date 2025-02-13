using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.accountroles
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public SysaccountRole SysaccountRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SysaccountRole = await _context.dbh.FindAsync<SysaccountRole>( id);

            if (SysaccountRole == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

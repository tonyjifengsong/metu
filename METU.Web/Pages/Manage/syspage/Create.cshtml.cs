using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspage
{
    public class CreateModel : MetuPageBase
    {
        

        public CreateModel(CoreBLL context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SysPage SysPage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.dbh.Add(SysPage);
            await _context.dbh.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

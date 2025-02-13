using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.ids4server
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
        public Id4server Id4server { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.dbh.Add(Id4server);
            await _context.dbh.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

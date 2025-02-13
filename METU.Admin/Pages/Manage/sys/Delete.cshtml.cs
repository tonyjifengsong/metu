using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using System.Data;

namespace METU.Admin.Pages.Manage.sys
{
    public class DeleteModel : MetuPageBase
    {
         

        public DeleteModel(CoreBLL context)
        {
            _context = context;
        }
 
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            pagename = servicename;
            try
            {
                dtCtrols = (DataTable)_context.PageInfo(pagename);
               // dts = _context.CoreService(dics.ServiceName() + "get", dics);
                dts = _context.CommonServicesgets(dics.ServiceName(), dics);
                FileHelper.Writelog("Page Data:");
                FileHelper.Writelog(dts);
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rs =   _context.CoreService("",getFormData());

            if ( !rs.IsSuccess)
            {
                return RedirectToPage("./error");
            }

            return RedirectToPage("./Index");
        }
    }
}

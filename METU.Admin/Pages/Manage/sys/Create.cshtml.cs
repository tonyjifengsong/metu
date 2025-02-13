using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using System.Data;

namespace METU.Admin.Pages.Manage.sys
{
    public class CreateModel : MetuPageBase
    {
 
        public CreateModel(CoreBLL context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            if (appname == null)
            {
                appname = "metu";
            }
            if (version == null)
            {
                version = "0";
            }
            if (cid == null)
            {
                cid = "WMS";
            }
            if (servicename == null)
            {
                servicename = appname;
            }
            getForm();
            try
            {
                dtCtrols =(DataTable) _context.PageInfo(servicename);
                FileHelper.Writelog("Page Control  Data:");
                FileHelper.Writelog(dtCtrols);
                // dtDataList = _context.CoreServicePageDataInfo(servicename, dics);
                FileHelper.Writelog("Page Data:");
                FileHelper.Writelog(dtDataList);
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            dic = new Dictionary<string, object>();
            dic = getFormData();

            try
            {
                dic.AddServiceName("");
                _context.sysconfigServicesadd( dic.ToDictionary());
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);

            }

            return RedirectToPage("./Index");
        }
    }
}

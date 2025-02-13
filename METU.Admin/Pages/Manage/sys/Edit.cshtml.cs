using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using System.Data;
using METU.MODEL;

namespace METU.Admin.Pages.Manage.sys
{
    public class EditModel : MetuPageBase
    {
        

        public EditModel(CoreBLL context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
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
                dtCtrols = (DataTable)_context.PageInfo(servicename);
                // dtCtrols = (DataTable)me.PageInfo(version, cid, appname, servicename);//适用于多库操作
                FileHelper.Writelog("Page Control  Data:");
                FileHelper.Writelog(dtCtrols);
                // dtDataList = _context.CoreServicePageDataInfo(servicename, dics);
                dts = _context.CommonServicesgets(dics.ServiceName(),dics);
                //dts = ((Result)_context.sysconfigServicesgets( dics)).Data.Result();
                FileHelper.Writelog("Page Data:");
                FileHelper.Writelog(dts);
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
           
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            dic = new Dictionary<string, object>();
            dic = getFormData();
           
            try
            {
                 _context.CoreExecuteSQL("",dic.ToDictionary());
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
                
            }

            return RedirectToPage("./Index");
        }

    }
}

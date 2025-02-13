using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using System.Data;

namespace METU.Admin.Pages.Manage.sys
{
    public class DetailsModel : MetuPageBase
    {
         
        public DetailsModel(CoreBLL context)
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
                dts = _context.CommonServicesgets(dics.ServiceName(), dics);
                // dts = _context.CoreService(dics.ServiceName() + "get", dics);
                FileHelper.Writelog("Page Data:");
                FileHelper.Writelog(dts);
                
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
            return Page();
        }
    }
}

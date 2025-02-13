using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
using System.Data;

namespace METU.Admin.Pages.Manage.sys
{
    public class IndexModel : MetuPageBase
    {
       
        public IndexModel(CoreBLL context)
        {

            _context = context;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="appname"></param>
       /// <param name="pagename"></param>
       /// <param name="version"></param>
       /// <param name=""></param>
       /// <returns></returns>
        public async Task OnGetAsync(string appname,string pagename,string version )
        {
      
            if (pagename == null)
            {
                pagename = "WMSservicePage";
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
                //dtCtrols = (DataTable)me.PageInfo(version, cid, appname, servicename);//适用于多库操作
                FileHelper.Writelog("Page Control  Data:");
                FileHelper.Writelog(dtCtrols);
                  dtDataList =  _context.CoreServicePageDataInfo(servicename, dics) ;
                FileHelper.Writelog("Page Data:");
                FileHelper.Writelog(dtDataList);
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
           
        }
    }
}

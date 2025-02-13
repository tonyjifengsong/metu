using System.Data;
using METU.BasicCore.Controllers;

namespace METU.Admin.Pages.Manage.sys
{
    public class ListModel :   MetuPageBase
    {
      

        public ListModel(CoreBLL context)
        {
            _context = context;
        }
        public async Task OnGetAsync(string appname, string pagename, string version)
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
            if (id == null)
            {
                id = "WMS";
            }
            if (servicename == null)
            {
                servicename = appname;
            }
            try
            {
                dtCtrols = (DataTable)me.PageInfo(version, id, appname, servicename);
                FileHelper.Writelog(dtCtrols);
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
           // SysAdmin = await _context.dbh.Set<SysAdmin>().AsNoTracking().ToListAsync();
        }
    }
}

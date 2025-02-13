using METU.CACHES;
using METU.CONFIGS;
using METU.MODEL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace METU.API.Controllers
{
    [EnableCors("cors")]
    [Route("metus/{id}APP{version}")]
    [ApiController]
    public class DebugTestController : ControllerBase
    {
        CoreBLL bll;
        public DebugTestController()
        {

            bll = new CoreBLL();
        }
        
        /// <summary>
        /// 执行业通用务逻辑
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{appname}/{servicename}service/{methodname}api")]
        public object DynCore([FromRoute] string version, [FromRoute] string id, [FromRoute] string appname, [FromRoute] string servicename, [FromRoute] string methodname, [FromBody] Dictionary<string, string> dic)
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'";
                sqlconfig = sqlconfig.Replace("{1}", id);
                sqlconfig = sqlconfig.Replace("{0}", appname);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt.ToDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                return new Result(WebAPIHelper.CallAPI(sqlstr, dic)); ;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
    }
}

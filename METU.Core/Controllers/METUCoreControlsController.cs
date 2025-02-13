using METU.CACHES;
using METU.CONFIGS;
using METU.MODEL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace METU.Main.Controllers
{ /// <summary>
  /// 任意系统接口调用方法
  /// </summary>
 
    [EnableCors("cors")]
    [Route("metuapi/{id}APP")]
    [ApiController]
    public partial class METUCoreControlsController : ControllerBase
    {
        CoreBLL bll;
         public METUCoreControlsController()
        {
            bll = new CoreBLL();
        }
        /// <summary>
        /// 根据数据库中的表名获取页面控件的基本信息（获取实体字段及描述）地址组合：{数据库配置名}APP/{数据库中表名}/getpageinfo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("{tname}/getpageinfo")]
        public dynamic Reports([FromRoute] string id, [FromRoute] string tname)
        {

            bll = new CoreBLL((id).ToUpper());
            string sqlstr = @"SELECT   TOP (200)   
               lower( a.name) AS english, 
           (CASE WHEN  b.name ='datetime'  THEN 'Date'    WHEN  b.name ='bit'  THEN 'switch'  ELSE 'input' END) AS datatype,
                (CASE WHEN a.isnullable = 1 THEN 'True' ELSE 'False' END) AS require,
  (CASE WHEN lower(a.name) ='id' THEN '主键' WHEN lower(a.name) ='cid' THEN '所属企业主键' WHEN lower(a.name) ='updatedate' THEN '更新日期' WHEN lower(a.name) ='createdate' THEN '添加日期' WHEN lower(a.name) ='isenabled' THEN '是否启用' WHEN lower(a.name) ='isdeleted' THEN '是否删除' WHEN lower(a.name) ='updateuserid' THEN '更新人ID' WHEN lower(a.name) ='createuserid' THEN '创建人ID' ELSE cast( ISNULL(g.value, lower(a.name))  as nvarchar(100)) END) AS chinese 
,concat('请输入',cast(ISNULL(g.value, ' ') as nvarchar)) as msg,concat('请输入',cast(ISNULL(g.value, ' ') as nvarchar)) as placeholder,'' as explain
             
FROM      sys.syscolumns AS a LEFT OUTER JOIN
                sys.systypes AS b ON a.xtype = b.xusertype INNER JOIN
                sys.sysobjects AS d ON a.id = d.id AND d.xtype = 'U' AND d.name <> 'dtproperties' LEFT OUTER JOIN
                sys.syscomments AS e ON a.cdefault = e.id LEFT OUTER JOIN
                sys.extended_properties AS g ON a.id = g.major_id AND a.colid = g.minor_id LEFT OUTER JOIN
                sys.extended_properties AS f ON d.id = f.class AND f.minor_id = 0
WHERE   (b.name IS NOT NULL) and d.name='{0}' 
ORDER BY a.id ";
            sqlstr = sqlstr.Replace("{0}", tname);


            return bll.db.ExecuteDataTable(sqlstr.ToLower());
        }

        /// <summary>
        /// 根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中表名}/getlist
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中表名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
          [HttpPost]
     [EnableCors("cors")]
        [Route("{tname}/getlist")]
        public object configServices([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
           


            if (id == null)
            {
                id = "SYS";
            }

            FileHelper.Writelog(dic, "SQL");
            bool configtag = false;
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}'";

            sqlstr = sqlstr.Replace("{0}", tname);
            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }

        /// <summary>
        /// [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        /// <returns></returns>
         [HttpPost]
      [EnableCors("cors")]
        [Route("{tname}/SYSgetpageinfo")]
        public object PageInfo([FromRoute] string id, string tname)
        {
            if (id != null)
            {
                bll = new CoreBLL(id.ToUpper());
            }
            else
            {
                bll = new CoreBLL();
            }

            string sqlstr = @"SELECT   SysPage.pagename, SysPage.objecttype, SysPage.objectname, SysPage.pagenameens, SysPageConfigs.sourcedata, 
                SysPageConfigs.interfaceaddress, SysPageConfigs.sourcedata AS fsourcedata, 
                SysPageConfigs.InterfaceAddress AS finterfaceaddress, SysPage.cid, SysPageConfigs.id, SysPageConfigs.syspageid, 
                SysPageConfigs.ControlName,SysPageConfigs.ControlName as english, SysPageConfigs.ControlCaption as chinese, SysPageConfigs.ControlType as datatype, SysPageConfigs.controlcaption, SysPageConfigs.controltype, 
                SysPageConfigs.placeholder, SysPageConfigs.require, SysPageConfigs.msg, SysPageConfigs.explain, 
                SysPageConfigs.enabled,
                SysPageConfigs.ControlOrder, SysPageConfigs.isgroup, SysPageConfigs.groupfield
FROM      SysPage INNER JOIN
                SysPageConfigs ON SysPage.ID = SysPageConfigs.SysPageID AND SysPage.CID = SysPageConfigs.CID where SysPage.PageNameens='{0}' ";
            sqlstr = sqlstr.Replace("{0}", tname);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return bll.db.ExecuteDataTable(sqlstr.Str2SQL());
        }
        /// <summary>
        ///[页面控件信息内容保存到后端] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/add
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
      [EnableCors("cors")]
        [Route("{tname}/SYSgetpageinfosave")]
        public object sysconfigServicessave([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
            
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}save'";

            sqlstr = sqlstr.Replace("{0}", tname); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }

        /// <summary>
        /// [根据页面控件信息获取对应的数据]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/edit
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
      [EnableCors("cors")]
        [Route("{tname}/SYSgetpageinfoget")]
        public object sysconfigServicesgets([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
            
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}getmodel'";

            sqlstr = sqlstr.Replace("{0}", tname);
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }

        /// <summary>
        /// [列表]根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/sysgetlist
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
      [EnableCors("cors")]
        [Route("{tname}/sysgetlist")]
        public object sysconfigServices([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
             
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                FileHelper.Writelog(sqlconfig, "SQL");
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}'";

            sqlstr = sqlstr.Replace("{0}", tname);
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }


        /// <summary>
        ///[添加] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/add
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
        [Route("{tname}/add")]
       [EnableCors("cors")]
       public object sysconfigServicesadd([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
            
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}add'";

            sqlstr = sqlstr.Replace("{0}", tname); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }

        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/edit
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
        [Route("{tname}/edit")]
     [EnableCors("cors")]
         public object sysconfigServicesedit([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        { 
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}edit'";

            sqlstr = sqlstr.Replace("{0}", tname); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");


                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }
        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/edit
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
        [Route("{tname}/UPDATE")]
      [EnableCors("cors")]
        public object sysconfigServicesupdate([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
            
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}update'";

            sqlstr = sqlstr.Replace("{0}", tname); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString().ToLower();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = cbll.db.Execute(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除（逻辑删除）指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
          [HttpPost]
        [Route("{tname}/delete")]
      [EnableCors("cors")]
       public object sysconfigServicesdelete([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
            
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}delete'";

            sqlstr = sqlstr.Replace("{0}", tname); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除(物理删除)指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
        [Route("{tname}/removebykey")]
      [EnableCors("cors")]
        public object sysconfigServicesremovebykey([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
           
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}removebykey'";

            sqlstr = sqlstr.Replace("{0}", tname); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.Execute(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }
        /// <summary>
        /// [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/get
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
        [Route("{tname}/get")]
      [EnableCors("cors")]
        public object sysconfigServicesget([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
            
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}get'";

            sqlstr = sqlstr.Replace("{0}", tname);
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }

        /// <summary>
        /// [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/get
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
         [HttpPost]
        [Route("{tname}/list")]
    [EnableCors("cors")]
          public object sysconfigServiceslist([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        {
            
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]=N'{0}list'";

            sqlstr = sqlstr.Replace("{0}", tname);
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }
        /// <summary>
        /// 根据数据库配置，服务配置 执行对应的业务逻辑；地址组合：{数据库配置名}APP/{数据库中表SysPageService中字段servicename服务名}Service
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中表SysPageService中字段servicename服务名</param>
        /// <param name="dic">SQL需要传入的参数</param>
        /// <returns></returns>
       [EnableCors("cors")]
        [HttpPost]
        [Route("{tname}Service")]
        public object Services([FromRoute] string id, [FromRoute]  string tname, [FromBody] Dictionary<string, string> dic)
        { 
            if (id == null)
            {
                id = "SYS";
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
                string sqlconfig = "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSPageDICConfig] aa, [dbo].[SYSPageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return "未找到相关配置数据";
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}'";

            sqlstr = sqlstr.Replace("{0}", tname); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = cbll.db.ExecuteDataTable(sqlstr.ToLower());
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
                                        else
                                        {

                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt;
                            }
                            else
                            {
                                if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                                {
                                    FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                    var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                    return rsdt;
                                }
                                else
                                {
                                    if (dt.Rows[0][1].ToString() == "WEBAPI")
                                    {
                                        return WebAPIHelper.CallAPI(sqlstr, dic);
                                    }
                                    else
                                    {
                                        FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                        var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                        return rsdt;
                                    }
                                }

                            }
                        }
            return true;
        }

        /// <summary>
        /// 不同系统模块间服务的调用
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="ctrl">控制器名</param>
        /// <param name="actname">方法名</param>
        /// <param name="model">需要传入的参数对象</param>
        /// <returns></returns>
       [EnableCors("cors")]
        [HttpPost]
        [Route("{ctrl}/{actname}")]
        public object Login([FromRoute] string id, string ctrl, string actname, Dictionary<string, object> model)
        {
            CommonCache.ConfigCaches[id.ToUpper()].validatedIsString("请确认是否已经配置好对应的接口地址！");
            string urlroot = CommonCache.ConfigCaches[id.ToUpper()].ToString();
            urlroot = urlroot + ctrl + "/" + actname;

            var rs = WebAPIHelper.CallAPI(urlroot, model);
            return rs;
        }
        /// <summary>
        /// 外部接口地址的调用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
       [EnableCors("cors")]
        [HttpPost]
        [Route("{ctrl}/webservice")]
        public object Loginwebservice([FromRoute] string id, string ctrl, Dictionary<string, object> model)
        {
            CommonCache.ConfigCaches[id.ToUpper()].validatedIsString("请确认是否已经配置好对应的接口地址！");
            string urlroot = CommonCache.ConfigCaches[id.ToUpper()].ToString();
            urlroot = urlroot + ctrl.Replace("--", "/");

            var rs = WebAPIHelper.CallAPI(urlroot, model);
            return rs;
        }

        /// <summary>
        /// 外部接口地址的调用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
       [EnableCors("cors")]
        [HttpPost]
        [Route("{ctrl}API")]
        public object Callwebservice([FromRoute] string id, string ctrl, Dictionary<string, object> model)
        {
            CommonCache.ConfigCaches[id.ToUpper()].validatedIsString("请确认是否已经配置好对应的接口地址！");
            string urlroot = CommonCache.ConfigCaches[id.ToUpper()].ToString();
            urlroot = urlroot + ctrl.Replace("--", "/");

            var rs = WebAPIHelper.CallAPI(urlroot, model);

            return rs;
        }
     
        /// <summary>
        /// 多ＳＱＬ语句执行，非事务；服务名：{ctrl}bntservice
        /// </summary>
        /// <returns></returns>
       [EnableCors("cors")]
        [HttpPost]
        [Route("{ctrl}/bntservice")]
        public Result BatServiceNotTransRecords([FromRoute] string id, [FromRoute]string ctrl, Dictionary<string, object> model)
        {
          
            if (model == null)
            {
                MSGBox.EXMessage("传入参数不可以为空！");
            }
            var dic = model.ToDictionary();

            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}bntservice'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = bll.db.ExecuteDataTable(sqlstr.ToLower());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero)
                {
                    List<string> sqltrans = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][0] != null) if (dt.Rows[i][0].ToString().Trim().Length > 10)
                            {
                                sqlstr = dt.Rows[i][0].ToString();

                                if (dic != null) if (dic.Count > ConstNum.Zero)
                                    {
                                        foreach (var itm in dic)
                                        {
                                            if (itm.Value != null)
                                            {
                                                sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }

                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                if (rsdt < 0)
                                {
                                    result = "业务执行失败！";
                                }


                            }
                    }



                }

         
            return new Result (result);

        }

       
        /// <summary>
        /// SQL事务执行；服务名：{ctrl}sqlexecute
        /// </summary>
        /// <returns></returns>
       [EnableCors("cors")]
        [HttpPost]
        [Route("{ctrl}/sqlexecute")]
        public Result ExecuteSQLServiceNotTransRecords([FromRoute] string id, [FromRoute]string ctrl, Dictionary<string, object> model)
        {
            

            if (model == null)
            {
                MSGBox.EXMessage("传入参数不可以为空！");
            }
            var dic = model.ToDictionary();

            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = bll.db.ExecuteDataTable(sqlstr.ToLower());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero)
                {

                    if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
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
                                        else
                                        {

                                        }
                                    }
                                }

                            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                            var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                            if (rsdt < 0)
                            {
                                result = "业务执行失败！";
                            }


                        }



                }

            return new Result(result);

        }
      
        /// <summary>
        /// 通用导出功能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ctrl"></param>
        /// <param name="file"></param>
        /// <returns></returns>
       [EnableCors("cors")]
        [HttpPost]
        [Route("{ctrl}template")]
        public  Result ReplaceTemplate([FromRoute] string id, [FromRoute]string ctrl, Dictionary<string, string> file)
        {

           

            string path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\";
            string localPath = path + id.ToString().Replace("-", "\\") + "\\";
            if (!Directory.Exists(Path.GetDirectoryName(localPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localPath));
            }

            string filenamestr = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + ".docx";

            var filePath = localPath + filenamestr;
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";
           sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = bll.db.ExecuteDataTable(sqlstr.ToLower());

            
            return new Result(dt);

        }

        [HttpGet]
       [EnableCors("cors")]

        [Route("{ctrl}download")]
        public IActionResult Download([FromRoute] string id, [FromRoute]string ctrl, Dictionary<string, string> file)
        {

            string filepath = null;
            if (string.IsNullOrEmpty(filepath)) filepath = "D:\\123.docx";
            var provider = new FileExtensionContentTypeProvider();
            FileInfo fileInfo = new FileInfo(filepath);
            var ext = fileInfo.Extension;
            new FileExtensionContentTypeProvider().Mappings.TryGetValue(ext, out var contenttype);
            return File(System.IO.File.ReadAllBytes(filepath), contenttype ?? "application/octet-stream", fileInfo.Name);
        }

    }
}
